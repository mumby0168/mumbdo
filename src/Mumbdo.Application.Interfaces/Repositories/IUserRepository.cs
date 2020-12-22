using System.Threading.Tasks;
using Mumbdo.Domain.Entities;

namespace Mumbdo.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> IsEmailInUseAsync(string email);

        Task SaveAsync(IUser user);
        Task<IUser> GetByEmailAsync(string email);
    }
}