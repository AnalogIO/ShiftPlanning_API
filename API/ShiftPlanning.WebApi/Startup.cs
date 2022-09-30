using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NSwag;
using Serilog;
using ShiftPlanning.Common.Configuration;
using ShiftPlanning.Model;
using ShiftPlanning.WebApi.Exceptions;
using ShiftPlanning.WebApi.Helpers.Authorization;
using ShiftPlanning.WebApi.Helpers.Mappers;
using ShiftPlanning.WebApi.Repositories;
using ShiftPlanning.WebApi.Services;
using ShiftPlanning.WebApi.Token;

namespace ShiftPlanning.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            
            // Setup database connection
            var databaseSettings = Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
            services.AddDbContext<ShiftPlannerDataContext>(opt =>
                opt.UseSqlServer(databaseSettings.ConnectionString,
                    c => c.MigrationsHistoryTable("__EFMigrationsHistory", databaseSettings.SchemaName)));
            services.AddScoped<IShiftPlannerDataContext, ShiftPlannerDataContext>(e => e.GetService<ShiftPlannerDataContext>());
            
            // Setup Dependency injection
            //Repositories
            services.AddScoped<ICheckInRepository, CheckInRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IFriendshipRepository, FriendshipRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IShiftRepository, ShiftRepository>();
            //Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IFriendshipService, FriendshipService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IShiftService, ShiftService>();
            //Helpers
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IPhotoMapper, PhotoMapper>();
            services.AddScoped<IOpeningHoursMapper, OpeningHoursMapper>();
            services.AddScoped<IVolunteerMapper, EmployeeMapper>();
            
            // Setup Swagger
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Cafe Analog ShiftPlanning API";
                    document.Info.Description = "ASP.NET Core web API for the coffee bar Cafe Analog";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new OpenApiContact
                    {
                        Name = "AnalogIO",
                        Email = "admin@analogio.dk",
                        Url = "https://github.com/analogio"
                    };
                    document.Info.License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = "https://github.com/AnalogIO/analog-core/blob/master/LICENSE"
                    };
                };
            });
            
            // Setup Json Serializing
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            //Error handling so the end user never sees the exceptions on the server
            services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilter()));

            services.AddCors(options => options.AddDefaultPolicy(builder =>
                builder.WithOrigins("https://localhost:8001", "https://localhost:44326", "https://shifty.analogio.dk", "https://shiftplanning.cafeanalog.dk").AllowAnyMethod().AllowAnyHeader()
                    .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization)));

            // Setup Authentication
            var identitySettings = Configuration.GetSection("IdentitySettings").Get<IdentitySettings>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "bearer";
                options.DefaultChallengeScheme = "bearer";
            }).AddJwtBearer("bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identitySettings.TokenKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero //the default for this setting is 5 
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            context.Response.Headers.Add("Token-Expired", "true");

                        return Task.CompletedTask;
                    }
                };
            });
            
            services.UseConfigurationValidation();
            services.ConfigureValidatableSetting<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            services.ConfigureValidatableSetting<FtpSettings>(Configuration.GetSection("FtpSettings"));
            services.ConfigureValidatableSetting<IdentitySettings>(Configuration.GetSection("IdentitySettings"));
            services.ConfigureValidatableSetting<PodioSettings>(Configuration.GetSection("PodioSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("logs/log.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}