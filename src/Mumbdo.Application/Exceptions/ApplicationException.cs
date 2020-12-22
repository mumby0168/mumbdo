using System;
using System.Net;

namespace Mumbdo.Application.Exceptions
{
    public abstract class ApplicationException : Exception
    {
        public abstract string ErrorCode { get; }
        
        public abstract HttpStatusCode StatusCode { get; }
        
        protected ApplicationException() {}

        protected ApplicationException(string message) : base(message)
        {
            
        }
    }
}