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
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://shifty.analogio.dk/shiftplanning/") }); //builder.HostEnvironment.BaseAddress
            builder.Services.AddScoped<IShiftRepository, ShiftRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetService<CustomAuthStateProvider>());
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddMudServices();

            await builder.Build().RunAsync();
        }
    }
}