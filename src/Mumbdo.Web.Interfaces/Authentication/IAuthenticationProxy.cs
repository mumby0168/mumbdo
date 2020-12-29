using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;
using Mumbdo.Web.Interfaces.Common;

namespace Mumbdo.Web.Interfaces.Authentication
{
    public interface IAuthenticationProxy : IProxy
    {
        Task<JwtTokenDto> RefreshAsync(string refreshToken, string email);

        Task<JwtTokenDto> SignInAsync(string username, string password);
    }
}