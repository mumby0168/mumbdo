using System.Net;
using Mumbdo.Shared.Codes;

namespace Mumbdo.Application.Exceptions
{
    public class InvalidCredentialsException : ApplicationException
    {
        public override string ErrorCode => UserCodes.InvalidCredentials;
        public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
    }
}