using System.Net.Http;

namespace Mumbdo.Web.Interfaces.Common
{
    public interface IHttpResponse
    {
        public HttpResponseMessage Message { get; }
        
        bool RefusedConnection { get; }
    }
    
    
    public interface IHttpResponse<out T> : IHttpResponse
    {
        public T Data { get; }
    }
}