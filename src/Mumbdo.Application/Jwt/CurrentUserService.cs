using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Mumbdo.Application.Jwt
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        
        public CurrentUser GetCurrentUser()
        {
            var email = _httpContext.HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
            var idString = _httpContext.HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = _httpContext.HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;

            return new CurrentUser(idString is null ? Guid.Empty : Guid.Parse(idString), email, role);
        }
    }
}