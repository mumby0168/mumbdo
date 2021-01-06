using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Mumbdo.Application.Exceptions;
// ReSharper disable PossibleNullReferenceException

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
            try
            {
                var email = _httpContext.HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Email).Value;
                var idString = _httpContext.HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier).Value;
                var role = _httpContext.HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role).Value;
                return new CurrentUser(Guid.Parse(idString), email, role);
            }
            catch (NullReferenceException)
            {
                throw new MumbdoNotAuthorisedException();
            }
        }
    }
}