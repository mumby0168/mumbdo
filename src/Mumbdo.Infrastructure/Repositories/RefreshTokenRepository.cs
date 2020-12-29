using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Infrastructure.Documents;

namespace Mumbdo.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoRepository<RefreshTokenDocument, Guid> _repository;

        public RefreshTokenRepository(IMongoRepository<RefreshTokenDocument, Guid> repository) => _repository = repository;

        public async Task SaveTokenAsync(Guid userId, string token, DateTime expires)
        {
            var existing = await _repository.FindAsync(o => o.UserId == userId);

            if (existing.Any())
            {
                foreach (var refreshTokenDocument in existing)
                {
                    await _repository.DeleteAsync(refreshTokenDocument.Id);
                }
            }
            
            await _repository.AddAsync(new RefreshTokenDocument(Guid.NewGuid(), token, userId, expires));
        }
            

        public Task<bool> IsTokenValid(Guid userId, string token)
        {
            throw new NotImplementedException();
        }

        public Task RevokeTokenAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tuple<Guid, DateTime>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}