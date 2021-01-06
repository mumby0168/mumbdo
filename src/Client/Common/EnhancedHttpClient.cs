using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Interfaces.Wrappers;

namespace Mumbdo.Web.Common
{
    public class EnhancedHttpClient : IHttpClient
    {
        private readonly IJson _json;
        private readonly HttpClient _client;

        public EnhancedHttpClient(IJson json, HttpClient client)
        {
            _json = json;
            _client = client;
        }

        private HttpContent Json(string json) 
            => new StringContent(json, Encoding.UTF8, "application/json");

        public async Task<IHttpResponse<TReturn>> PostAsync<T, TReturn>(string url, T data) where TReturn : class
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _client.PostAsync(url, Json(await _json.SerializeAsync(data)));
            }
            catch (HttpRequestException)
            {
                return new HttpResponse<TReturn>(null, null, true);
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return new HttpResponse<TReturn>(response, await _json.DeserializeAsync<TReturn>(content));
            }

            return new HttpResponse<TReturn>(response, null);
        }

        public async Task<IHttpResponse> PostAsync<T>(string url, T data)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _client.PostAsync(url, Json(await _json.SerializeAsync(data)));
            }
            catch (HttpRequestException)
            {
                return new HttpResponse(response, true);
            }

            return new HttpResponse(response);
        }

        public async Task<IHttpResponse<T>> GetAsync<T>(string url) where T : class
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _client.GetAsync(url);
            }
            catch (HttpRequestException)
            {
                return new HttpResponse<T>(response, null, true);
            }
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return new HttpResponse<T>(response, await _json.DeserializeAsync<T>(content));
            }

            return new HttpResponse<T>(response, null);
        }

        public async Task<IHttpResponse> GetAsync(string url)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _client.GetAsync(url);
            }
            catch (HttpRequestException)
            {
                return new HttpResponse(response, true);
            }

            return new HttpResponse(response);
        }

        public async Task<IHttpResponse<TReturn>> PutAsync<T, TReturn>(string url, T data) where TReturn : class
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _client.PutAsync(url, Json(await _json.SerializeAsync(data)));
            }
            catch (HttpRequestException)
            {
                return new HttpResponse<TReturn>(null, null, true);
            }

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return new HttpResponse<TReturn>(response, await _json.DeserializeAsync<TReturn>(content));
            }

            return new HttpResponse<TReturn>(response, null);
        }

        public async Task<IHttpResponse> PutAsync<T>(string url, T data)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _client.PutAsync(url, Json(await _json.SerializeAsync(data)));
            }
            catch (HttpRequestException)
            {
                return new HttpResponse(response, true);
            }

            return new HttpResponse(response);
        }

        public async Task<IHttpResponse<T>> DeleteAsync<T>(string url) where T : class
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _client.DeleteAsync(url);
            }
            catch (HttpRequestException)
            {
                return new HttpResponse<T>(response, null, true);
            }
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return new HttpResponse<T>(response, await _json.DeserializeAsync<T>(content));
            }

            return new HttpResponse<T>(response, null);
        }

        public async Task<IHttpResponse> DeleteAsync(string url)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _client.DeleteAsync(url);
            }
            catch (HttpRequestException)
            {
                return new HttpResponse(response, true);
            }

            return new HttpResponse(response);
        }

        public void AddHeader(string key, string value) 
            => _client.DefaultRequestHeaders.Add(key, value);

        public void AddAuthorizationHeader(string value) 
            => AddHeader("Authorization", value);

        public void AddBearerToken(string token) 
            => AddAuthorizationHeader(token);

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}