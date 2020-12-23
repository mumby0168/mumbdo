using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Web.Interfaces.Managers
{
    public interface ITokenManager
    {
        Task SaveTokenAsync(JwtTokenDto tokenDto);

        Task<JwtTokenDto> GetTokenAsync();

        Task RemoveTokenAsync();
    }
}