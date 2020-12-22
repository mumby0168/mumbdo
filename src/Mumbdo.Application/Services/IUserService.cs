using System.Threading.Tasks;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Services
{
    public interface IUserService
    {
        Task CreateAsync(CreateUserDto dto);
    }
}