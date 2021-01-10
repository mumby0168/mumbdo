using Mumbdo.Domain.Entities;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Transport
{
    public interface ITransferDataService
    {
        TaskDto AsTaskDto(ITaskEntity entity);
    }
}