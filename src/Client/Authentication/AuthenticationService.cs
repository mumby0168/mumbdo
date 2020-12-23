using System;
using System.Threading.Tasks;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;
using Mumbdo.Web.Interfaces.Authentication;
using Mumbdo.Web.Interfaces.Managers;

namespace Mumbdo.Web.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenManager _tokenManager;

        public AuthenticationService(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }
        
        private SignedInUser _user = null;
        
        public async Task SignInAsync(JwtTokenDto tokenDto)
        {
            _user = new SignedInUser(tokenDto.Token);
            await _tokenManager.SaveTokenAsync(tokenDto);
            AuthenticationStateUpdated?.Invoke(null, EventArgs.Empty);
        }

        public async Task<bool> IsUserSignedInAsync()
        {
            if (_user is not null)
            {
                if (_user.Expiry > DateTime.Now)
                {
                    return true;
                }
                await SignOutAsync();
                return false;
            }

            var token = await _tokenManager.GetTokenAsync();
            if (token is not null)
            {
                _user = new SignedInUser(token.Token);
                if (_user.Expiry > DateTime.Now)
                {
                       
                }
                AuthenticationStateUpdated?.Invoke(null, EventArgs.Empty);
            }

            return false;
        }

        public SignedInUser User => _user;
        

        public Task SignOutAsync()
        {
            _user = null;
            _tokenManager.RemoveTokenAsync();
            AuthenticationStateUpdated?.Invoke(null, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public string EmailAddress => _user?.Email;
        
        public string Role => _user?.Role;
        
        public Guid Id => _user?.Id ?? Guid.Empty;
        
        public event EventHandler AuthenticationStateUpdated;
    }
}