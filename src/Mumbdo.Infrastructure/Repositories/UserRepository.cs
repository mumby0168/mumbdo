using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Domain.Entities;
using Mumbdo.Infrastructure.Documents;

namespace Mumbdo.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public UserRepository(IMongoRepository<UserDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public Task<bool> IsEmailInUseAsync(string email) => _repository.ExistsAsync(d => d.Email == email);


        public Task SaveAsync(IUserEntity userEntity) => _repository.AddAsync(userEntity.AsDocument());

        public async Task<IUserEntity> GetByEmailAsync(string email)
        {
            var userDoc = await _repository.GetAsync(ud => ud.Email == email);
            return userDoc?.AsUser();
        }
    }
}