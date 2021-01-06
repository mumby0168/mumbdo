using Mumbdo.Domain.Entities;

namespace Mumbdo.Domain.Aggregates
{
    public interface ITaskAggergate
    {
        ITaskEntity Create();
    }
}