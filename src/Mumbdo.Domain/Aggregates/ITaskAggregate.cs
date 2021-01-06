using System;
using Mumbdo.Domain.Entities;

namespace Mumbdo.Domain.Aggregates
{
    public interface ITaskAggregate
    {
        ITaskEntity Create(string name, Guid userId, Guid? groupId = null, DateTime? deadline = null);
    }
}