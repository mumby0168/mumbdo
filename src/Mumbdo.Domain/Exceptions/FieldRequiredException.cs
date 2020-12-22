using Mumbdo.Shared.Codes;

namespace Mumbdo.Domain.Exceptions
{
    public class FieldRequiredException : DomainException
    {
        public override string ErrorCode => CommonCodes.FieldRequired;

        public FieldRequiredException(string fieldName) : base($"{fieldName} is required")
        {
            
        }
    }
}