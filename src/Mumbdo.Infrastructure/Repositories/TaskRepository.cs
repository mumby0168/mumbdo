using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Domain.Entities;
using Mumbdo.Infrastructure.Documents;

namespace Mumbdo.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IMongoRepository<TaskDocument, Guid> _repository;

        public TaskRepository(IMongoRepository<TaskDocument, Guid> repository)
        {
            _repository = repository;
        }
        
        public async Task<IEnumerable<ITaskEntity>> GetUnCompleteInGroupAsync(Guid groupId, bool complete = true)
        {
            var results = await _repository.FindAsync(t => t.GroupId == groupId && t.IsComplete == complete);
            return results.Select(task => task.AsDomain());
        }

        public Task CreateAsync(ITaskEntity task) => _repository.AddAsync(task.AsDocument());
        
        public async Task<IEnumerable<ITaskEntity>> GetUngroupedTasksForUserAsync(Guid userId, bool complete = false)
        {
            var tasks = await _repository.FindAsync(t => t.UserId == userId && t.IsComplete == complete);
            return tasks.Select(t => t.AsDomain());
        }
    }

    public static class TaskEntityExtensions
    {
        public static TaskDocument AsDocument(this ITaskEntity task) => new(task.Id, task.Created,
            task.IsComplete, task.Deadline, task.UserId, task.GroupId, task.Name);
    }
}