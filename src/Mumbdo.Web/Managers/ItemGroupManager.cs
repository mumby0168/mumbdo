
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudBlazor;
using Mumbdo.Shared;
using Mumbdo.Web.Interfaces.Managers;

namespace Mumbdo.Web.Managers
{
    public class TaskGroupManager : ITaskGroupManager
    {
        private List<ItemGroupDto> _itemGroups;

        private List<TaskDto> _tasks = new List<TaskDto>()
        {
            new ("Item 1", DateTime.Now.AddDays(-1), false, DateTime.Now.AddDays(3)),
            new ("Item 1", DateTime.Now.AddDays(-1), true),
        };

        public TaskGroupManager()
        {
            _itemGroups = new List<ItemGroupDto>
            {
                new(Guid.NewGuid(), "Shopping", "A list to capture my shopping", "imgs/supermarket.jpg", _tasks),
                new(Guid.NewGuid(), "Christmas Shopping", "A list to capture my christmas shopping", "imgs/christmas.jpg", new List<TaskDto>())
            };
        }
        
        public Task<IEnumerable<ItemGroupDto>> GetAllGroupsAsync() => Task.FromResult(_itemGroups.AsEnumerable());

        public async Task<string> AddGroupAsync(ItemGroupDto dto)
        {
            _itemGroups.Add(dto);
            return string.Empty;
        }

        public Task<ItemGroupDto> GetAsync(Guid id)
        {
            var item = _itemGroups.First();
            if (item is not null && !item.Tasks.Any())
            {
                item.Tasks.AddRange(_tasks);
            }
            return Task.FromResult(item);
        }
    }
}