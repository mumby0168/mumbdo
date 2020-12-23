using System.Threading.Tasks;
using Microsoft.JSInterop;
using Mumbdo.Web.Interfaces.Managers;

namespace Mumbdo.Web.Managers
{
    public class LocalStorageManager : ILocalStorageManager
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageManager(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        
        public ValueTask Set(string key, string value)
            => _jsRuntime.InvokeVoidAsync("saveToLocalStorage", key, value);

        public ValueTask<string> Get(string key) 
            => _jsRuntime.InvokeAsync<string>("getFromLocalStorage", key);

        public ValueTask Remove(string key) 
            => _jsRuntime.InvokeVoidAsync("removeFromLocalStorage", key);
    }
}