using System;
using Convey.Types;
using Mumbdo.Domain.Entities;

namespace Mumbdo.Infrastructure.Documents
{
    public class UserDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; private set; }
        
        public string Email { get; private set; }
        
        public string Password { get; private set; }

        public string Role { get; private set; }

        public UserDocument(Guid id, string email, string password, string role)
        {
            Id = id;
            Email = email;
            Password = password;
            Role = role;
        }

        public UserDocument()
        {
            
        }

        public IUserEntity AsUser()
        {
            return new UserEntity(Id, Email, Mumbdo.Domain.ValueObjects.Password.UnPack(Password));
        }
    }

    public static class UserDocumentExtensions
    {
        public static UserDocument AsDocument(this IUserEntity userEntity)
        {
            return new UserDocument(userEntity.Id, userEntity.Email, userEntity.Password.Pack(), userEntity.Role);
        }
    }
}