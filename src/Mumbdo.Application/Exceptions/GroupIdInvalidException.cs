using System.Net;
using Mumbdo.Shared.Codes;

namespace Mumbdo.Application.Exceptions
{
    public class GroupIdInvalidException : ApplicationException
    {
        public override string ErrorCode => ItemGroupCodes.InvalidGroupId;
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public GroupIdInvalidException() : base("The group cannot be found") { } 
    }
}