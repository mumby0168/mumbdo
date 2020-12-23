using System;
using System.Threading.Tasks;
using Mumbdo.Shared;

namespace Mumbdo.Web.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        void SignIn(SignedInUser user);

        Task<bool> IsUserSignedInAsync();

        void SignOut();
        
        string EmailAddress { get; }
        
        string Role { get; }
        
        Guid Id { get; }

        event EventHandler AuthenticationStateUpdated;
    }
}