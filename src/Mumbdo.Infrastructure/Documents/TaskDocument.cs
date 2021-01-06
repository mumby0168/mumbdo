using System;
using Convey.Types;
using Mumbdo.Domain.Entities;

namespace Mumbdo.Infrastructure.Documents
{
    public class TaskDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; private set; }
        public DateTime Created { get; private set; }
        public bool IsComplete { get; private set; }
        public DateTime? Deadline { get; private set; }
        public Guid UserId { get; private set; }
        public Guid? GroupId { get; private set; }
        public string Name { get; private set; }

        public TaskDocument(Guid id, DateTime created, bool isComplete, DateTime? deadline, Guid userId, Guid? groupId, string name)
        {
            Id = id;
            Created = created;
            IsComplete = isComplete;
            Deadline = deadline;
            UserId = userId;
            GroupId = groupId;
            Name = name;
        }

        public ITaskEntity AsDomain()
        {
            return new TaskEntity(Id, Created, IsComplete, Deadline, UserId, GroupId, Name);
        }

        public TaskDocument()
        {
            
        }
    }
}