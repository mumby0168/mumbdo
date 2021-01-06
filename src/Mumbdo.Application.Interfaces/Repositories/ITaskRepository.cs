using System;
using System.Threading.Tasks;
using Mumbdo.Domain.Entities;

namespace Mumbdo.Infrastructure.Repositories
{
    public interface ITaskRepository
    {
        Task<ITaskEntity> GetInGroupAsync(Guid groupId);
    }
}