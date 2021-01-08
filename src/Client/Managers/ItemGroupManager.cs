
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudBlazor;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;
using Mumbdo.Web.Interfaces.Managers;
using Mumbdo.Web.Interfaces.Proxies;

namespace Mumbdo.Web.Managers
{
    public class ItemGroupManager : IItemGroupManager
    {
        private readonly IGroupProxy _groupProxy;
        private List<ItemGroupDto> _itemGroups;

        private List<TaskDto> _tasks = new List<TaskDto>()
        {
            new (Guid.NewGuid(), "Item 1", DateTime.Now.AddDays(-1), false, null, DateTime.Now.AddDays(3)),
            new (Guid.NewGuid(),"Item 1", DateTime.Now.AddDays(-1), true, null),
        };

        public ItemGroupManager(IGroupProxy groupProxy)
        {
            _groupProxy = groupProxy;
            _itemGroups = new List<ItemGroupDto>
            {
                new(Guid.NewGuid(), "Shopping", "A list to capture my shopping", "imgs/supermarket.jpg", _tasks),
                new(Guid.NewGuid(), "Christmas Shopping", "A list to capture my christmas shopping", "imgs/christmas.jpg", new List<TaskDto>())
            };
        }

        public async Task<IEnumerable<ItemGroupDto>> GetAllGroupsAsync()
        {
            var groups = await _groupProxy.GetGroupsAsync(true);
            var error = _groupProxy.ErrorMessage;
            
            if (error != string.Empty)
            {
                return null;
            }

            return groups;
        }

        public async Task<string> AddGroupAsync(ItemGroupDto dto)
        {
            await _groupProxy.CreateAsync(dto.Name, dto.Description, dto.Image);
            var error = _groupProxy.ErrorMessage;
            return error != string.Empty ? error : string.Empty;
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