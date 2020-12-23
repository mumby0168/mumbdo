using System.Text.Json;
using System.Threading.Tasks;

namespace Mumbdo.Web.Interfaces.Wrappers
{
    public interface IJson
    {
        Task<string> SerializeAsync(object obj);

        Task<T> DeserializeAsync<T>(string json);
        
    }
}