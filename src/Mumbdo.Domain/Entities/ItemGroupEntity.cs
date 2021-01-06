using System;

namespace Mumbdo.Domain.Entities
{
    public class ItemGroupEntity : IItemGroupEntity
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Name { get; }
        public string Description { get; }
        public string ImageUri { get; }

        public ItemGroupEntity(Guid id, Guid userId, string name, string description, string imageUri)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Description = description;
            ImageUri = imageUri;
        }
    }
}