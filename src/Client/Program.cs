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
using Mumbdo.Web.Interfaces.Authentication;
using Mumbdo.Web.Interfaces.Managers;
using Mumbdo.Web.Interfaces.Settings;
using Mumbdo.Web.Managers;

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
            builder.Services.AddSingleton<IItemGroupManager, ItemGroupManager>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

            var settings = new Settings.Settings()
            {
                BaseUrl = builder.HostEnvironment.BaseAddress + "api"
            };

            builder.Services.AddSingleton<ISettings>(settings);

            var host =  builder.Build();
            await host.RunAsync();
        }
    }
}
