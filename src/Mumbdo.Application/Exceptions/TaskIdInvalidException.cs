using System.Net;
using Mumbdo.Shared.Codes;

namespace Mumbdo.Application.Exceptions
{
    public class TaskIdInvalidException : ApplicationException
    {
        public override string ErrorCode => TaskCodes.InvalidTaskId;
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public TaskIdInvalidException() : base("The task could not be found")
        {
            
        }
    }
}