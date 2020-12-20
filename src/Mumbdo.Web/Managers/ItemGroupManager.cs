
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

        public TaskGroupManager()
        {
            _itemGroups = new List<ItemGroupDto>
            {
                new(Guid.NewGuid(), "Shopping", "A list to capture my shopping", "imgs/supermarket.jpg"),
                new(Guid.NewGuid(), "Christmas Shopping", "A list to capture my christmas shopping",
                    "imgs/christmas.jpg")
            };
        }
        
        public Task<IEnumerable<ItemGroupDto>> GetAllGroupsAsync() => Task.FromResult(_itemGroups.AsEnumerable());

        public async Task<string> AddGroupAsync(ItemGroupDto dto)
        {
            _itemGroups.Add(dto);
            return string.Empty;
        }
    }
}