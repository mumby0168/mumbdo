using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;
using Mumbdo.Web.Interfaces.Authentication;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Interfaces.Wrappers;

namespace Mumbdo.Web.Interfaces.Proxies
{
    public abstract class ProxyBase
    {
        private readonly IJson _json;
        private string _errorMessage = string.Empty;


        protected ProxyBase(IJson json)
        {
            _json = json;
        }
        
        public ProxyBase() {}

        public virtual string SetErrorMessage(string error) => _errorMessage = error;

        public string ErrorMessage
        {
            get
            {
                string error = _errorMessage;
                _errorMessage = string.Empty;
                return error;
            }
        }

        protected async Task AuthoriseAsync(IHttpClient client, IAuthenticationService authenticationService)
        {
            if (await authenticationService.IsUserSignedInAsync())
            {
                client.AddBearerToken(authenticationService.Token);
            }
        }

        public virtual void SetConnectionRefusedError() => SetErrorMessage(CommonNames.ConnectionRefusedError);

        public virtual async Task ProcessErrorAsync(HttpResponseMessage responseMessage)
        {
            var error = await ParseErrorAsync(responseMessage);
            Console.WriteLine($"Signing user in error {error?.Code ?? "no_code"}");
            var message = SetErrorMessage(error?.Message ?? CommonNames.ConnectionRefusedError);
            Console.WriteLine($"Error: {message}");
        }
        
        private async Task<MumbdoErrorDto> ParseErrorAsync(HttpResponseMessage responseMessage)
        {
            var json = await responseMessage.Content.ReadAsStringAsync();
            try
            {
                return await _json.DeserializeAsync<MumbdoErrorDto>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}