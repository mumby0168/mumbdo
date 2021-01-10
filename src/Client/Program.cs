using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;
using Mumbdo.Shared;
using Mumbdo.Web.Authentication;
using Mumbdo.Web.Common;
using Mumbdo.Web.Interfaces.Authentication;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Interfaces.Managers;
using Mumbdo.Web.Interfaces.Proxies;
using Mumbdo.Web.Interfaces.Settings;
using Mumbdo.Web.Interfaces.Wrappers;
using Mumbdo.Web.Managers;
using Mumbdo.Web.Proxies;
using Mumbdo.Web.Wrappers;

namespace Mumbdo.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api") });
            builder.Services.AddMudBlazorDialog();
            builder.Services.AddMudBlazorSnackbar();
            builder.Services.AddMudBlazorResizeListener();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<IItemGroupManager, ItemGroupManager>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<ILocalStorageManager, LocalStorageManager>();
            builder.Services.AddSingleton<IJson, Json>();
            builder.Services.AddScoped<IHttpClient, EnhancedHttpClient>();
            builder.Services.AddSingleton<ITokenManager, TokenManager>();
            builder.Services.AddSingleton<IUserContext, UserContext>();
            builder.Services.AddTransient<IProxyHelper, ProxyHelper>();
            builder.Services.AddScoped<IAuthenticationProxy, AuthenticationProxy>();
            builder.Services.AddScoped<IGroupProxy, GroupProxy>();
            builder.Services.AddScoped<ITaskProxy, TaskProxy>();
            builder.Services.AddScoped<IConfirmationService, ConfirmationService>();


            var host =  builder.Build();

            var sp = host.Services;

            await host.RunAsync();
        }
    }
}
