using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimCoRetailManager_WASM.Auth;
using TimCoRetailManager_WPF.Library;
using TimCoRetailManager_WPF.Library.Models;
using TimCoRetailManager_WPF.Library.Services;

namespace TimCoRetailManager_WASM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<AuthenticationStateProvider, StateProvider>();

            builder.Services.AddSingleton<IApi, Api>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<ISaleService, SaleService>();

            builder.Services.AddSingleton<IUser, User>();  // app-wide user

            await builder.Build().RunAsync();
        }
    }
}
