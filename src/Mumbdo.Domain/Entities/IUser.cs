using System;
using Mumbdo.Domain.ValueObjects;

namespace Mumbdo.Domain.Entities
{
    public interface IUser
    {
        Guid Id { get; }
        
        string Email { get; }
        
        Password Password { get; }
    }
}