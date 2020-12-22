using System;
using Mumbdo.Domain.Entities;
using Mumbdo.Domain.Exceptions;

namespace Mumbdo.Domain.Aggregates
{
    public class ItemGroupAggregate : IItemGroupAggregate
    {
        public IItemGroup Create(Guid userId, string name, string description, string imageUri)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new FieldRequiredException("Name");
            }

            if (string.IsNullOrWhiteSpace(imageUri))
            {
                throw new FieldRequiredException("Image URL");
            }
            
            return new ItemGroup(Guid.NewGuid(), userId, name, description, imageUri);
        }
    }
}