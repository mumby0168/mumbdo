using System.Threading.Tasks;
using Mumbdo.Domain.Entities;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Jwt
{
    public interface ITokenService
    {
        string CreateToken(IUser user);
    }
}