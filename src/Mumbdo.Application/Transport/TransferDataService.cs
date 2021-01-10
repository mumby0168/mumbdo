using Mumbdo.Domain.Entities;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Transport
{
    public class TransferDataService : ITransferDataService
    {
        public TaskDto AsTaskDto(ITaskEntity entity) => new(entity.Id, entity.Name, entity.Created, entity.IsComplete,
            entity.GroupId, entity.Deadline);
    }
}