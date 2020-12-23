using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Mumbdo.Web.Interfaces.Authentication;

namespace Mumbdo.Web.Authentication
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthenticationService _authenticationService;

        public JwtAuthenticationStateProvider(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            Console.WriteLine("Checking auth state");
            if (await _authenticationService.IsUserSignedInAsync())
            {
                var identity = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Role, _authenticationService.Role), 
                    new(ClaimTypes.Email, _authenticationService.EmailAddress),
                    new (ClaimTypes.NameIdentifier, _authenticationService.Id.ToString())
                });

                var user = new ClaimsPrincipal(identity);
                var state = new AuthenticationState(user);
                Console.WriteLine("Starting acceptance");
                return state;
            }

            NotifyAuthenticationStateChanged(Invalid());
            return await Invalid();
        }
        
        private Task<AuthenticationState> Invalid()
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
        } 
    }
}