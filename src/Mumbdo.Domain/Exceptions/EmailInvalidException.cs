using Mumbdo.Shared.Codes;

namespace Mumbdo.Domain.Exceptions
{
    public class EmailInvalidException : DomainException
    {
        public override string ErrorCode => UserCodes.EmailInvalid;

        public EmailInvalidException(string invalidEmail) : base($"{invalidEmail} is not a valid email address")
        {
            
        }
    }
}