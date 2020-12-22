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

        public IUser AsUser()
        {
            return new User(Id, Email, Mumbdo.Domain.ValueObjects.Password.UnPack(Password));
        }
    }

    public static class UserDocumentExtensions
    {
        public static UserDocument AsDocument(this IUser user)
        {
            return new UserDocument(user.Id, user.Email, user.Password.Pack(), user.Role);
        }
    }
}