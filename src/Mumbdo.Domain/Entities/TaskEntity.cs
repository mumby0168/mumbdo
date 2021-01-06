using System;

namespace Mumbdo.Domain.Entities
{
    public class TaskEntity : ITaskEntity
    {
        public Guid Id { get; }
        public DateTime Created { get; }
        public bool IsComplete { get; }
        public DateTime? Deadline { get; }
        public Guid UserId { get; }
        public Guid? GroupId { get; }
        public string Name { get; }

        public TaskEntity(Guid id, DateTime created, bool isComplete, DateTime? deadline, Guid userId, Guid? groupId, string name)
        {
            Id = id;
            Created = created;
            IsComplete = isComplete;
            Deadline = deadline;
            UserId = userId;
            GroupId = groupId;
            Name = name;
        }
    }
}