using System.Text.Json;
using System.Threading.Tasks;
using Mumbdo.Web.Interfaces.Wrappers;

namespace Mumbdo.Web.Wrappers
{
    public class Json : IJson
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        public Task<string> SerializeAsync(object obj)
        {
            var json = JsonSerializer.Serialize(obj, _options);
            return Task.FromResult(json);
        }

        public Task<T> DeserializeAsync<T>(string json)
        {
            var result = JsonSerializer.Deserialize<T>(json, _options);
            return Task.FromResult(result);
        }
    }
}