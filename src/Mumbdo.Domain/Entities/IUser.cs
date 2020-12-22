using System;
using Mumbdo.Domain.ValueObjects;

namespace Mumbdo.Domain.Entities
{
    public static class Roles
    {
        public const string User = "user";
    }
    public interface IUser
    {
        Guid Id { get; }
        
        string Email { get; }
        
        Password Password { get; }
        
        string Role { get; }
    }
}