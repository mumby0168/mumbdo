using System;
using System.Threading.Tasks;

namespace Mumbdo.Web.Interfaces.Common
{
    public interface IHttpClient : IDisposable
    {
        Task<IHttpResponse<TReturn>> PostAsync<T, TReturn>(string url, T data) where TReturn : class;

        Task<IHttpResponse> PostAsync<T>(string url, T data);

        Task<IHttpResponse<T>> GetAsync<T>(string url) where T : class;

        Task<IHttpResponse> GetAsync(string url);

        Task<IHttpResponse<TReturn>> PutAsync<T, TReturn>(string url, T data)  where TReturn : class;

        Task<IHttpResponse> PutAsync<T>(string url, T data);

        Task<IHttpResponse<T>> DeleteAsync<T>(string url) where T : class;

        Task<IHttpResponse> DeleteAsync(string url);

        void AddHeader(string key, string value);

        void AddAuthorizationHeader(string value);

        void AddBearerToken(string token);
    }
}