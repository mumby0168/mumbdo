using Mumbdo.Shared;
using Mumbdo.Web.Interfaces.Authentication;

namespace Mumbdo.Web.Authentication
{
    public class UserContext : IUserContext
    {
        public SignedInUser SignedInUser { get; set; }
    }
}