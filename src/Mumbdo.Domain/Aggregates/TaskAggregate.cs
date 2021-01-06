using System;
using Mumbdo.Domain.Entities;
using Mumbdo.Domain.Exceptions;

namespace Mumbdo.Domain.Aggregates
{
    public class TaskAggregate : ITaskAggregate
    {
        public ITaskEntity Create(string name, Guid userId, Guid? groupId = null, DateTime? deadline = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new FieldRequiredException("name");
            
            return new TaskEntity(Guid.NewGuid(), DateTime.Now, false, deadline, userId, groupId, name);
        }
    }
}