using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Web.Interfaces.Proxies
{
    public interface ITaskProxy : IProxyBase
    {
        Task CreateAsync(string name, Guid? groupId = null, DateTime? deadline = null);

        Task<TaskDto> UpdateAsync(Guid id, string name, bool isComplete, Guid? groupId, DateTime? deadline);

        Task DeleteAsync(Guid taskId);

        Task<IEnumerable<TaskDto>> GetUngroupedTasksAsync(bool completedTasks = false);
    }
}