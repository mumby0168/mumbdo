using System.Threading.Tasks;
using Mumbdo.Shared;

namespace Mumbdo.Application.Services
{
    public interface IUserService
    {
        Task CreateAsync(CreateUserDto dto);
    }
}