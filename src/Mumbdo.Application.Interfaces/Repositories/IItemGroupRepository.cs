using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mumbdo.Domain.Entities;

namespace Mumbdo.Application.Interfaces.Repositories
{
    public interface IItemGroupRepository
    {
        Task CreateAsync(IItemGroupEntity itemGroupEntity);

        Task<IEnumerable<IItemGroupEntity>> GetGroupsForUserAsync(Guid userId);
        Task<bool> ExistsAsync(Guid groupId);
        Task<IItemGroupEntity> GetAsync(Guid userId, Guid groupId);
    }
}   