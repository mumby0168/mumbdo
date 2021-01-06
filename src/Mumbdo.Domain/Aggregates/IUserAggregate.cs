using Mumbdo.Domain.Entities;
using Mumbdo.Domain.ValueObjects;

namespace Mumbdo.Domain.Aggregates
{
    public interface IUserAggregate
    {
        IUserEntity Create(string email, Password password);
    }
}