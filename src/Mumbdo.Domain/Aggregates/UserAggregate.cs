using System;
using System.ComponentModel.DataAnnotations;
using Mumbdo.Domain.Entities;
using Mumbdo.Domain.Exceptions;
using Mumbdo.Domain.ValueObjects;

namespace Mumbdo.Domain.Aggregates
{
    public class UserAggregate : IUserAggregate
    {
        public IUser Create(string email, Password password)
        {
            if (!new EmailAddressAttribute().IsValid(email))
                throw new EmailInvalidException(email);
                
            return new User(Guid.NewGuid(), email, password);
        }
    }
}