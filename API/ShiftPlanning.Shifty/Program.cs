using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using ShiftPlanning.Shifty.Repositories;
using ShiftPlanning.Shifty.Services;
using ShiftPlanning.Shifty.States;

namespace ShiftPlanning.Shifty
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001") }); //builder.HostEnvironment.BaseAddress
            builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<LoginState>();

            builder.Services.AddMudServices();

            await builder.Build().RunAsync();
        }
    }
}