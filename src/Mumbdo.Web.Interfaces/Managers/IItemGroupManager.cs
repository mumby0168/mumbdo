using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Web.Interfaces.Managers
{
    public interface IItemGroupManager
    {
        Task<IEnumerable<ItemGroupDto>> GetAllGroupsAsync();

        Task<string> AddGroupAsync(ItemGroupDto dto);

        Task<ItemGroupDto> GetAsync(Guid id);
    }
}