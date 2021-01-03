using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;
using Mumbdo.Shared.Urls;
using Mumbdo.Web.Interfaces.Authentication;
using Mumbdo.Web.Interfaces.Common;

namespace Mumbdo.Web.Authentication
{
    public class AuthenticationProxy : IAuthenticationProxy
    {
        private readonly IMumbdoHttpClient _httpClient;

        private string _errorMessage = string.Empty;

        public AuthenticationProxy(IMumbdoHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string ErrorMessage
        {
            get
            {
                string error = _errorMessage;
                _errorMessage = string.Empty;
                return error;
            }
        }

        public async Task<JwtTokenDto> RefreshAsync(string refreshToken, string email)
        {
            try
            {
                var (result, token) = await _httpClient.GetAsync<JwtTokenDto>(AuthenticationUrls.Refresh(refreshToken, email));
                if (token is not null)
                    return token;
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                    await ProcessErrorAsync(result);
            }
            catch (Exception)
            {
                // ignored
            }

            _errorMessage = _httpClient.ConnectionRefusedErrorMessage;
            return null;
        }

        public async Task<JwtTokenDto> SignInAsync(string username, string password)
        {
            try
            {
                var (result, token) =
                    await _httpClient.PostDataAsync<SignInDto, JwtTokenDto>(AuthenticationUrls.SignIn,
                        new SignInDto(username, password));
                
                if (token is not null)
                    return token;

                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await ProcessErrorAsync(result);
                    return null;
                }
                    
            }
            catch (Exception)
            {
                /* ignored*/
            }

            _errorMessage = _httpClient.ConnectionRefusedErrorMessage;
            return null;
        }

        public async Task SignUpAsync(string email, string password)
        {
            try
            {
                var result = await _httpClient.PostAsync(AuthenticationUrls.SignUp, new CreateUserDto(email, password));
                
                if(result.StatusCode == HttpStatusCode.OK)
                    return;
                
                if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    await ProcessErrorAsync(result);
                    return;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            _errorMessage = _httpClient.ConnectionRefusedErrorMessage;
        }

        private async Task ProcessErrorAsync(HttpResponseMessage responseMessage)
        {
            var error = await _httpClient.ParseErrorAsync(responseMessage);
            Console.WriteLine($"Signing user in error {error?.Code ?? "no_code"}");
            _errorMessage = error?.Message ?? _httpClient.ConnectionRefusedErrorMessage;
            Console.WriteLine($"Error: {_errorMessage}");
        }
    }
}