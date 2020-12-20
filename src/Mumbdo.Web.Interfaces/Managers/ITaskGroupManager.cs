using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mumbdo.Shared;

namespace Mumbdo.Web.Interfaces.Managers
{
    public interface ITaskGroupManager
    {
        Task<IEnumerable<ItemGroupDto>> GetAllGroupsAsync();

        Task<string> AddGroupAsync(ItemGroupDto dto);
    }
}