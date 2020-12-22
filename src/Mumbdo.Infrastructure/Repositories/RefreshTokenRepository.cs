using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Infrastructure.Documents;

namespace Mumbdo.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoRepository<RefreshTokenDocument, Guid> _repository;

        public RefreshTokenRepository(IMongoRepository<RefreshTokenDocument, Guid> repository) => _repository = repository;

        public Task SaveTokenAsync(Guid userId, string token, DateTime expires) => 
            _repository.AddAsync(new RefreshTokenDocument(Guid.NewGuid(), token, userId, DateTime.Now.AddDays(1)));

        public Task<bool> IsTokenValid(Guid userId, string token)
        {
            throw new NotImplementedException();
        }
    }
}