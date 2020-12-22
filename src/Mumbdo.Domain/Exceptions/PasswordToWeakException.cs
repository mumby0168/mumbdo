using Mumbdo.Shared.Codes;

namespace Mumbdo.Domain.Exceptions
{
    public class PasswordToWeakException : DomainException
    {
        public override string ErrorCode => UserCodes.PasswordToWeak;

        public PasswordToWeakException() : base("The password provided does not meet our security requirements"){}
    }
}