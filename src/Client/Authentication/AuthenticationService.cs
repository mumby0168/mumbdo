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
        private readonly IAuthenticationProxy _authenticationProxy;
        private readonly IUserContext _userContext;

        public AuthenticationService(ITokenManager tokenManager, IAuthenticationProxy authenticationProxy, IUserContext userContext)
        {
            _tokenManager = tokenManager;
            _authenticationProxy = authenticationProxy;
            _userContext = userContext;
        }
        
        
        public async Task SignInAsync(JwtTokenDto tokenDto)
        {
            _userContext.SignedInUser = new SignedInUser(tokenDto.Token);
            await _tokenManager.SaveTokenAsync(tokenDto);
            AuthenticationStateUpdated?.Invoke(null, EventArgs.Empty);
        }

        public async Task<bool> IsUserSignedInAsync()
        {
            if (_userContext.SignedInUser is not null)
            {
                if (_userContext.SignedInUser.Expiry > DateTime.Now)
                {
                    return true;
                }
                await SignOutAsync();
                return false;
            }

            var token = await _tokenManager.GetTokenAsync();
            if (token is not null)
            {
                _userContext.SignedInUser = new SignedInUser(token.Token);
                if (_userContext.SignedInUser.Expiry < DateTime.Now)
                {
                    var newToken = await _authenticationProxy.RefreshAsync(token.Refresh, _userContext.SignedInUser.Email);
                    if (newToken is null)
                    {
                        Console.WriteLine("Failed to use refresh token");
                        _userContext.SignedInUser = null;
                        await _tokenManager.RemoveTokenAsync();
                        AuthenticationStateUpdated?.Invoke(null, EventArgs.Empty);
                        return false;
                    }

                    _userContext.SignedInUser = new SignedInUser(token.Token);
                    await _tokenManager.SaveTokenAsync(token);
                    
                }
                AuthenticationStateUpdated?.Invoke(null, EventArgs.Empty);
                return true;
            }

            return false;
        }

        public SignedInUser User => _userContext.SignedInUser;
        

        public Task SignOutAsync()
        {
            _userContext.SignedInUser = null;
            _tokenManager.RemoveTokenAsync();
            AuthenticationStateUpdated?.Invoke(null, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public string EmailAddress => _userContext.SignedInUser?.Email;
        
        public string Role => _userContext.SignedInUser?.Role;
        
        public Guid Id => _userContext.SignedInUser?.Id ?? Guid.Empty;
        
        public event EventHandler AuthenticationStateUpdated;
    }
}