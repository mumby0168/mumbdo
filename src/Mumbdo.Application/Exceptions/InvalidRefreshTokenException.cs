using System.Net;
using Mumbdo.Shared.Codes;

namespace Mumbdo.Application.Exceptions
{
    public class InvalidRefreshTokenException : ApplicationException
    {
        public override string ErrorCode => UserCodes.InvalidRefreshToken;
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public InvalidRefreshTokenException() : base("The token provided may be have expired or be invalid")
        {
            
        }
    }
}