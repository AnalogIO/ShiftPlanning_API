using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using ShiftPlanning.Shifty.Authentication;
using ShiftPlanning.Shifty.Repositories;
using ShiftPlanning.Shifty.Services;

namespace ShiftPlanning.Shifty
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMudServices();
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();

            
            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("ShiftPlanning"));
            services.AddScoped<IShiftRepository, ShiftRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<CustomAuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(s => s.GetService<CustomAuthStateProvider>());
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<RequestAuthenticationHandler>();
            services.AddHttpClient("ShiftPlanning",
                    client => client.BaseAddress = new Uri("https://shifty.analogio.dk/shiftplanning/"))
                .AddHttpMessageHandler<RequestAuthenticationHandler>();
        }
    }
}