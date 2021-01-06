using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Mumbdo.Application.Exceptions;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Application.Interfaces.Utilities;
using Mumbdo.Application.Jwt;
using Mumbdo.Domain.Aggregates;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskAggregate _taskAggregate;
        private readonly ITaskRepository _taskRepository;
        private readonly IEntityValidator _entityValidator;
        private readonly ICurrentUserService _currentUserService;

        public TaskService(ITaskAggregate taskAggregate, ITaskRepository taskRepository, IEntityValidator entityValidator, ICurrentUserService currentUserService)
        {
            _taskAggregate = taskAggregate;
            _taskRepository = taskRepository;
            _entityValidator = entityValidator;
            _currentUserService = currentUserService;
        }
        
        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            if (dto.GroupId is not null && !await _entityValidator.IsGroupIdValidAsync(dto.GroupId.Value)) 
                throw new GroupIdInvalidException();

            var userId = _currentUserService.GetCurrentUser().Id;

            var task = _taskAggregate.Create(dto.Name, userId, dto.GroupId, dto.Deadline);

            await _taskRepository.CreateAsync(task);

            return new(task.Id, task.Name, task.Created, task.IsComplete, task.GroupId, task.Deadline);
        }
        

        public async Task<IEnumerable<TaskDto>> GetUngroupedTasksAsync(bool completed = false)
        {
            var userId = _currentUserService.GetCurrentUser().Id;
            var tasks = await _taskRepository.GetUngroupedTasksForUserAsync(userId, completed);
            return tasks.Select(t => new TaskDto(t.Id, t.Name, t.Created, t.IsComplete, t.GroupId, t.Deadline));
        }
    }
}