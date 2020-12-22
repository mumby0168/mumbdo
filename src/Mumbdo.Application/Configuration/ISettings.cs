using Microsoft.IdentityModel.Tokens;

namespace Mumbdo.Application.Configuration
{
    public interface ISettings
    {
        SymmetricSecurityKey JwtKey { get; set; }
    }
}