using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Interfaces.Proxies;
using Mumbdo.Web.Interfaces.Wrappers;

namespace Mumbdo.Web.Common
{
    public class MumbdoHttpClient 
    {
        private readonly HttpClient _httpClient;
        private readonly IJson _json;
        private const string ApplicationJson = "application/json";

        public MumbdoHttpClient(HttpClient httpClient, IJson json)
        {
            _httpClient = httpClient;
            _json = json;
        }
        
        
        
        public async Task<MumbdoErrorDto> ParseErrorAsync(HttpResponseMessage responseMessage)
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
        
        private StringContent JsonContent(string json) => new StringContent(json, Encoding.UTF8, ApplicationJson);

        public void SetBearerToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public async Task<Tuple<HttpResponseMessage, R>> PostDataAsync<T, R>(string url, T data) where R : class
        {
            var json = await _json.SerializeAsync(data);
            var response = await _httpClient.PostAsync(url, JsonContent(json));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var returnJson = await response.Content.ReadAsStringAsync();
                return new Tuple<HttpResponseMessage, R>(response, await _json.DeserializeAsync<R>(returnJson));    
            }

            return new Tuple<HttpResponseMessage, R>(response, null);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string url, T data)
        {
            var json = await _json.SerializeAsync(data);
            return await _httpClient.PostAsync(url, JsonContent(json));
        }

        public async Task<Tuple<HttpResponseMessage, T>> GetAsync<T>(string url) where T : class
        {
            var result = await _httpClient.GetAsync(url);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return
                    new Tuple<HttpResponseMessage, T>(result, await _json
                        .DeserializeAsync<T>(await result.Content.ReadAsStringAsync()));
            }

            return new Tuple<HttpResponseMessage, T>(result, null);
        }

        public Task<T> ProcessResponse<T>(HttpResponseMessage response, T dataResponse, ProxyBase proxy)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}