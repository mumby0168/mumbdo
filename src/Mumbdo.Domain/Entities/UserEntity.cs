using System;
using Mumbdo.Domain.ValueObjects;

namespace Mumbdo.Domain.Entities
{
    public class UserEntity : IUserEntity
    {
        public Guid Id { get; }
        public string Email { get; }
        public Password Password { get; }
        public string Role { get; }

        public UserEntity(Guid id, string email, Password password)
        {
            Id = id;
            Email = email;
            Password = password;
            Role = Roles.User;
        }
        
        private UserEntity(){}
    }
}