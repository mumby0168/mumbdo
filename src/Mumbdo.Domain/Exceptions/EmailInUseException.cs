using Mumbdo.Shared.Codes;

namespace Mumbdo.Domain.Exceptions
{
    public class EmailInUseException : DomainException
    {
        public override string ErrorCode => UserCodes.EmailInUse;

        public EmailInUseException(string email) :  base($"{email} is already in use")
        {
            
        }
    }
}