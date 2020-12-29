using Mumbdo.Shared;

namespace Mumbdo.Web.Interfaces.Authentication
{
    public interface IUserContext
    {
        SignedInUser SignedInUser { get; set; }
        
    }
}