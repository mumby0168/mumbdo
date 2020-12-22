using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Services
{
    public interface IItemGroupService
    {
        Task CreateAsync(CreateItemGroupDto dto);
    }
}