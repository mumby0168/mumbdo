using System;
using Mumbdo.Domain.ValueObjects;

namespace Mumbdo.Domain.Entities
{
    public class User : IUser
    {
        public Guid Id { get; }
        public string Email { get; }
        public Password Password { get; }

        internal User(Guid id, string email, Password password)
        {
            Id = id;
            Email = email;
            Password = password;
        }
        
        private User(){}
    }
}