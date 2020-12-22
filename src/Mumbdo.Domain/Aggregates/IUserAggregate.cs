using Mumbdo.Domain.Entities;
using Mumbdo.Domain.ValueObjects;

namespace Mumbdo.Domain.Aggregates
{
    public interface IUserAggregate
    {
        IUser Create(string email, Password password);
    }
}