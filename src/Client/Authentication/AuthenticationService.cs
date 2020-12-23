using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Mumbdo.Shared;
using Mumbdo.Web.Interfaces.Authentication;

namespace Mumbdo.Web.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private SignedInUser _user = null;
        
        public void SignIn(SignedInUser user)
        {
            _user = user;
        }

        public async Task<bool> IsUserSignedInAsync()
        {
            if (_user is not null)
            {
                if (_user.Expiry > DateTime.Now)
                {
                    Console.WriteLine("User authorized");
                    return true;
                }
                Console.WriteLine(_user.Expiry.ToShortTimeString());
                SignOut();
                return false;
            }
            return false;
        }

        public void SignOut()
        {
            _user = null;
            AuthenticationStateUpdated?.Invoke(null, null);
        }

        public string EmailAddress => _user?.Email;
        
        public string Role => _user?.Role;
        
        public Guid Id => _user?.Id ?? Guid.Empty;
        
        public event EventHandler AuthenticationStateUpdated;
    }
}