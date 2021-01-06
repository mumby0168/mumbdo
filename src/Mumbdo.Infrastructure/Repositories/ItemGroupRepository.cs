using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Domain.Entities;
using Mumbdo.Infrastructure.Documents;

namespace Mumbdo.Infrastructure.Repositories
{
    public class ItemGroupRepository : IItemGroupRepository
    {
        private readonly IMongoRepository<ItemGroupDocument, Guid> _repository;

        public ItemGroupRepository(IMongoRepository<ItemGroupDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public Task CreateAsync(IItemGroupEntity itemGroupEntity)
        {
            return _repository.AddAsync(itemGroupEntity.AsDocument());
        }

        public async Task<IEnumerable<IItemGroupEntity>> GetGroupsForUserAsync(Guid userId)
        {
            var groups = await _repository.FindAsync(g => g.UserId == userId);

            return groups.Select(g => g.AsDomain());
        }

        public Task<bool> ExistsAsync(Guid groupId) => _repository.ExistsAsync(gi => gi.Id == groupId);
    }
}