using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mumbdo.Domain.Entities;

namespace Mumbdo.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<ITaskEntity>> GetUnCompleteInGroupAsync(Guid groupId, bool complete = true);

        Task CreateAsync(ITaskEntity task);
        
        Task<IEnumerable<ITaskEntity>> GetUngroupedTasksForUserAsync(Guid userId, bool complete = false);
    }
}