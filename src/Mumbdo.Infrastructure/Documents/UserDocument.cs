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

        public UserDocument(Guid id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }

        public UserDocument()
        {
            
        }
    }

    public static class UserDocumentExtensions
    {
        public static UserDocument AsDocument(this IUser user)
        {
            return new UserDocument(user.Id, user.Email, user.Password.Pack());
        }
    }
}