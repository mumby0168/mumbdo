using System.Net;
using Mumbdo.Shared.Codes;

namespace Mumbdo.Application.Exceptions
{
    public class InvalidUserCredentialsException : ApplicationException
    {
        public override string ErrorCode => UserCodes.InvalidCredentials;
        public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;

        public InvalidUserCredentialsException() : base("The credentials provided do not match our records")
        {
            
        }
    }
}