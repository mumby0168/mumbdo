using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Mumbdo.Application.Exceptions;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Application.Jwt;
using Mumbdo.Domain.Aggregates;
using Mumbdo.Domain.Entities;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Services
{
    public class ItemGroupService : IItemGroupService
    {
        private readonly IItemGroupRepository _itemGroupRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IItemGroupAggregate _itemGroupAggregate;
        private readonly ICurrentUserService _currentUserService;

        public ItemGroupService(IItemGroupAggregate itemGroupAggregate, ICurrentUserService currentUserService, IItemGroupRepository itemGroupRepository, ITaskRepository taskRepository)
        {
            _itemGroupRepository = itemGroupRepository;
            _taskRepository = taskRepository;
            _itemGroupAggregate = itemGroupAggregate;
            _currentUserService = currentUserService;
        }
        
        public async Task CreateAsync(CreateItemGroupDto dto)
        {
            var userId = _currentUserService.GetCurrentUser().Id;
            
            var group = _itemGroupAggregate.Create(userId, dto.Name, dto.Description, dto.Image);

            await _itemGroupRepository.CreateAsync(group);
        }

        public async Task<IEnumerable<ItemGroupDto>> GetAllForUserAsync(bool includeTasks = true)
        {
            var userId = _currentUserService.GetCurrentUser().Id;
            var groups = await _itemGroupRepository.GetGroupsForUserAsync(userId);
            var groupDtos = new List<ItemGroupDto>();
            foreach (var group in groups)
            {
                var taskDtos = new List<TaskDto>();
                
                if (includeTasks)
                {
                    var tasks = await _taskRepository.GetTasksInGroupAsync(group.Id);

                    foreach (var task in tasks)
                    {
                        taskDtos.Add(new(task.Id, task.Name, task.Created, task.IsComplete, task.GroupId, task.Deadline));
                    }    
                }
                
                groupDtos.Add(new (group.Id, group.Name, group.Description, group.ImageUri, taskDtos));
            }

            return groupDtos;
        }

        public async Task<ItemGroupDto> GetAsync(Guid groupId, bool includeTasks = true)
        {
            var userId = _currentUserService.GetCurrentUser().Id;
            
            var group = await _itemGroupRepository.GetAsync(userId, groupId);
            
            if (group is null)
                   throw new GroupIdInvalidException();

            var taskDtos = new List<TaskDto>();

            if (includeTasks)
            {
                var tasks = await _taskRepository.GetTasksInGroupAsync(groupId);
                foreach (var taskEntity in tasks)
                {
                    taskDtos.Add(new TaskDto(taskEntity.Id, taskEntity.Name,taskEntity.Created, taskEntity.IsComplete, taskEntity.GroupId));
                }
            }

            var groupDto = new ItemGroupDto(group.Id, group.Name, group.Description, group.ImageUri, taskDtos);

            return groupDto;
        }
    }
}