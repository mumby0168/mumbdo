using System;
using System.Runtime.Versioning;

namespace Mumbdo.Application.Jwt
{
    public record CurrentUser(Guid Id, string Email, string Role);
    
    public interface ICurrentUserService
    {
        CurrentUser GetCurrentUser();
    }
}