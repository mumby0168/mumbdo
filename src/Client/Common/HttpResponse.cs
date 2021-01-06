using System.Net.Http;
using Mumbdo.Web.Interfaces.Common;

namespace Mumbdo.Web.Common
{
    public class HttpResponse<T> : IHttpResponse<T> where T : class
    {
        public HttpResponseMessage Message { get; }
        
        public bool RefusedConnection { get; }
        
        public T Data { get; }

        public HttpResponse(HttpResponseMessage message, T data, bool refusedConnection = false)
        {
            Message = message;
            RefusedConnection = refusedConnection;
            Data = data;
        }
    }

    public class HttpResponse : IHttpResponse
    {
        public HttpResponseMessage Message { get; }
        
        public bool RefusedConnection { get; }

        public HttpResponse(HttpResponseMessage message, bool refusedConnection = false)
        {
            Message = message;
            RefusedConnection = refusedConnection;
        }
    }
}