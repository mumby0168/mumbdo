using System;
using System.Threading.Tasks;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Application.Interfaces.Utilities;

namespace Mumbdo.Infrastructure.Utilities
{
    public class EntityValidator : IEntityValidator
    {
        private readonly IItemGroupRepository _itemGroupRepository;

        public EntityValidator(IItemGroupRepository itemGroupRepository)
        {
            _itemGroupRepository = itemGroupRepository;
        }
        
        public Task<bool> IsGroupIdValidAsync(Guid groupId)
        {
            return _itemGroupRepository.ExistsAsync(groupId);
        }
    }
}