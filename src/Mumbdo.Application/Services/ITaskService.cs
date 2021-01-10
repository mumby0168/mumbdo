using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Services
{
    public interface ITaskService
    {
        Task<TaskDto> CreateAsync(CreateTaskDto dto);

        Task<IEnumerable<TaskDto>> GetUngroupedTasksAsync(bool completed = false);
        Task<TaskDto> UpdateAsync(UpdateTaskDto dto);
        Task DeleteAsync(Guid id);
    }
}