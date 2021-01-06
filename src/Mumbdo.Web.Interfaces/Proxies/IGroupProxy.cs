using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Web.Interfaces.Proxies
{
    public interface IGroupProxy : IProxyBase
    {
        Task<IEnumerable<ItemGroupDto>> GetGroupsAsync(bool includeTasks = false);

        Task<ItemGroupDto> GetAsync(Guid id, bool includeTasks = false);

        Task CreateAsync(string name, string description, string image);
    }
}