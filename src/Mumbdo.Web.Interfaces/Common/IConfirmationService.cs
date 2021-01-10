using System.Reflection;
using System.Threading.Tasks;

namespace Mumbdo.Web.Interfaces.Common
{
    public interface IConfirmationService
    {
        Task<bool> ConfirmAsync(string confirmationMessage);
    }
}