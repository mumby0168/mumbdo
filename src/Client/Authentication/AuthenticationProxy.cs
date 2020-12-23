using System;
using System.Net;
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

        public Task<JwtTokenDto> RefreshAsync(string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<JwtTokenDto> SignInAsync(string username, string password)
        {
            try
            {
                var (result, token) = await _httpClient.PostDataAsync<SignInDto, JwtTokenDto>(AuthenticationUrls.SignIn, new SignInDto(username, password));
                if (token is not null)
                {
                    return token;
                }

                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var error = await _httpClient.ParseErrorAsync(result);
                    Console.WriteLine($"Signing user in error {error?.Code ?? "no_code"}");
                    _errorMessage = error?.Message ?? _httpClient.ConnectionRefusedErrorMessage;
                }
            }
            catch (Exception) { /* ignored*/ }
                
            _errorMessage = _httpClient.ConnectionRefusedErrorMessage;
            return null;
        }
    }
}