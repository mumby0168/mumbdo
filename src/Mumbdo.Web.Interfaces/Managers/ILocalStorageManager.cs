using System.Threading.Tasks;
using System.Xml;

namespace Mumbdo.Web.Interfaces.Managers
{
    public interface ILocalStorageManager
    {
        ValueTask Set(string key, string value);

        ValueTask<string> Get(string key);

        ValueTask Remove(string key);
    }
}