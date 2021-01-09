using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Services
{
    public interface IItemGroupService
    {
        Task CreateAsync(CreateItemGroupDto dto);

        Task<IEnumerable<ItemGroupDto>> GetAllForUserAsync(bool includeTasks = true);
        Task<ItemGroupDto> GetAsync(Guid groupId, bool includeTasks = true);
    }
}