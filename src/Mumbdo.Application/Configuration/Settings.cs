using Microsoft.IdentityModel.Tokens;

namespace Mumbdo.Application.Configuration
{
    public class Settings : ISettings
    {
        public SymmetricSecurityKey JwtKey { get; set; }
    }
}