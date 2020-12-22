using System;
using Mumbdo.Domain.ValueObjects;

namespace Mumbdo.Domain.Entities
{
    public class User : IUser
    {
        public Guid Id { get; }
        public string Email { get; }
        public Password Password { get; }
        public string Role { get; }

        public User(Guid id, string email, Password password)
        {
            Id = id;
            Email = email;
            Password = password;
            Role = Roles.User;
        }
        
        private User(){}
    }
}