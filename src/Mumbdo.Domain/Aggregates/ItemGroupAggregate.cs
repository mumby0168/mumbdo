using System;
using Mumbdo.Domain.Entities;
using Mumbdo.Domain.Exceptions;

namespace Mumbdo.Domain.Aggregates
{
    public class ItemGroupAggregate : IItemGroupAggregate
    {
        public IItemGroupEntity Create(Guid userId, string name, string description, string imageUri)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new FieldRequiredException("Name");
            }

            if (string.IsNullOrWhiteSpace(imageUri))
            {
                throw new FieldRequiredException("Image URL");
            }
            
            return new ItemGroupEntity(Guid.NewGuid(), userId, name, description, imageUri);
        }
    }
}