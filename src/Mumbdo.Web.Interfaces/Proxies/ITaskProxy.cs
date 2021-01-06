using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mumbdo.Web.Interfaces.Proxies
{
    public interface ITaskProxy
    {
        Task CreateAsync(string name, Guid? groupId = null, DateTime? deadline = null);
    }
}