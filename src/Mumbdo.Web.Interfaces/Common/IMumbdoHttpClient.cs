using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Web.Interfaces.Common
{
    public interface IMumbdoHttpClient : IDisposable
    {
        Task<Tuple<HttpResponseMessage, R>> PostDataAsync<T, R>(string url, T data) where R : class;

        Task<HttpResponseMessage> PostAsync<T>(string url, T data);

        string ConnectionRefusedErrorMessage => CommonNames.ConnectionRefusedError;

        Task<MumbdoErrorDto> ParseErrorAsync(HttpResponseMessage responseMessage);

        Task<Tuple<HttpResponseMessage, JwtTokenDto>> GetAsync<T>(string url) where T : class;
    }
}