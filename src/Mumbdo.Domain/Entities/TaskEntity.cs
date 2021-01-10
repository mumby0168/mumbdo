using System;

namespace Mumbdo.Domain.Entities
{
    public class TaskEntity : ITaskEntity
    {
        public Guid Id { get; }
        public DateTime Created { get; }
        public bool IsComplete { get; private set; }
        public DateTime? Deadline { get; private set; }
        public Guid UserId { get; }
        public Guid? GroupId { get; private set; }
        public string Name { get; private set; }

        public void Update(string name, bool isComplete, Guid? groupId, DateTime? deadline)
        {
            Name = name;
            IsComplete = isComplete;
            GroupId = groupId;
            Deadline = deadline;
        }

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