using System;
using Mumbdo.Domain.Entities;

namespace Mumbdo.Domain.Aggregates
{
    public interface IItemGroupAggregate
    {
        IItemGroup Create(Guid userId, string name, string description, string imageUri);
    }
}