using System;
using System.Net.Http;
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
using Mumbdo.Web.Managers;

namespace Mumbdo.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMudBlazorDialog();
            builder.Services.AddMudBlazorSnackbar();
            builder.Services.AddMudBlazorResizeListener();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddSingleton<IItemGroupManager, ItemGroupManager>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

            var host =  builder.Build();

            var authenticationService = host.Services.GetRequiredService<IAuthenticationService>();
            authenticationService.SignIn(new SignedInUser("eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJiNGRmYmZlOC04ZWFiLTQyZjktYjNiZS04MTBkYTJlNGYyM2EiLCJlbWFpbCI6Indqbm0ubXVtYnkxQGdtYWlsLmNvbSIsInJvbGUiOiJ1c2VyIiwibmJmIjoxNjA4NzQwMjE2LCJleHAiOjE2MDg3NTEwMTYsImlhdCI6MTYwODc0MDIxNn0.P6ONa6o4MnXJpjQXCeTQTUF8VVQMD6r4w-kiEPkXo_8pAmgCTeYtkuHqIBn0JVtTpCCaJOKKif1e0uhd7Q_xAg"));

            await host.RunAsync();
        }
    }
}
