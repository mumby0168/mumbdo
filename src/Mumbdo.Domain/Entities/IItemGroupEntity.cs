using System;

namespace Mumbdo.Domain.Entities
{
    public interface IItemGroupEntity
    {
        Guid Id { get; }
        
        Guid UserId { get; }
        
        string Name { get; }
        
        string Description { get; }
        
        string ImageUri { get; }
    }
}