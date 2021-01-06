using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;
using Mumbdo.Shared.Urls;
using Mumbdo.Web.Interfaces.Authentication;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Interfaces.Proxies;
using Mumbdo.Web.Interfaces.Wrappers;

namespace Mumbdo.Web.Authentication
{
    public class AuthenticationProxy : ProxyBase, IAuthenticationProxy
    {
        private readonly IHttpClient _httpClient;
        private readonly IProxyHelper _proxyHelper;


        public AuthenticationProxy(IHttpClient httpClient, IJson json, IProxyHelper proxyHelper) : base(json)
        {
            _httpClient = httpClient;
            _proxyHelper = proxyHelper;
        }
        
        public async Task<JwtTokenDto> RefreshAsync(string refreshToken, string email)
        {
            var result = await _httpClient.GetAsync<JwtTokenDto>(AuthenticationUrls.Refresh(refreshToken, email));
            return await _proxyHelper.ProcessResponseAsync(result, this);
        }

        public async Task<JwtTokenDto> SignInAsync(string username, string password)
        {
            var result = await _httpClient.PostAsync<SignInDto, JwtTokenDto>(AuthenticationUrls.SignIn,new SignInDto(username, password));
            return await _proxyHelper.ProcessResponseAsync(result, this);
        }

        public async Task SignUpAsync(string email, string password)
        {
            var result = await _httpClient.PostAsync(AuthenticationUrls.SignUp, new CreateUserDto(email, password));
            await _proxyHelper.ProcessResponseAsync(result, this);
        }
        
    }
}