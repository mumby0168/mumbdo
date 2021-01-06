using System;
using Convey.Types;
using Mumbdo.Domain.Entities;

namespace Mumbdo.Infrastructure.Documents
{
    public class ItemGroupDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageUri { get; private set; }

        public ItemGroupDocument()
        {
            
        }

        public ItemGroupDocument(Guid id, Guid userId, string name, string description, string imageUri)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Description = description;
            ImageUri = imageUri;
        }

        public IItemGroupEntity AsDomain() 
            => new ItemGroupEntity(Id, UserId, Name, Description, ImageUri);
    }

    public static class ItemGroupDocumentExtensions
    {
        public static ItemGroupDocument AsDocument(this IItemGroupEntity itemGroupEntity)
        {
            return new ItemGroupDocument(itemGroupEntity.Id, itemGroupEntity.UserId, itemGroupEntity.Name, itemGroupEntity.Description,
                itemGroupEntity.ImageUri);
        }
    }
    
}