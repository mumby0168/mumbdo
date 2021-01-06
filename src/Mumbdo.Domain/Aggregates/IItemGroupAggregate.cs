using System;
using Mumbdo.Domain.Entities;

namespace Mumbdo.Domain.Aggregates
{
    public interface IItemGroupAggregate
    {
        IItemGroupEntity Create(Guid userId, string name, string description, string imageUri);
    }
}