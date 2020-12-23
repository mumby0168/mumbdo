using System;
using System.Threading.Tasks;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Web.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task SignInAsync(JwtTokenDto tokenDto);

        Task<bool> IsUserSignedInAsync();
        
        SignedInUser User { get; }

        Task SignOutAsync();
        
        string EmailAddress { get; }
        
        string Role { get; }
        
        Guid Id { get; }

        event EventHandler AuthenticationStateUpdated;
    }
}