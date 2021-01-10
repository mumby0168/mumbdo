using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mumbdo.Domain.Entities;

namespace Mumbdo.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<ITaskEntity>> GetTasksInGroupAsync(Guid groupId, bool complete = false);

        Task CreateAsync(ITaskEntity task);
        
        Task<IEnumerable<ITaskEntity>> GetUngroupedTasksForUserAsync(Guid userId, bool complete = false);
        Task<ITaskEntity> GetAsync(Guid id, Guid userId);

        Task UpdateAsync(ITaskEntity task);
        Task DeleteAsync(Guid id);
    }
}