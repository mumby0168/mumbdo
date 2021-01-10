using System;

namespace Mumbdo.Domain.Entities
{
    public interface ITaskEntity
    {
        Guid Id { get; }
        
        DateTime Created { get; }
        
        bool IsComplete { get; }
        
        DateTime? Deadline { get; }
        
        Guid UserId { get; }
        
        Guid? GroupId { get; }
        string Name { get;  }

        void Update(string name, bool isComplete, Guid? groupId, DateTime? deadline);
    }
}