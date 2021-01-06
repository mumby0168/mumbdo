using System.Net;
using System.Net.Http;

namespace Mumbdo.Shared
{
    public static class Extensions
    {
        public static bool IsOk(this HttpResponseMessage message) => message.StatusCode == HttpStatusCode.OK;
        
        public static bool IsBad(this HttpResponseMessage message) => message.StatusCode == HttpStatusCode.BadRequest;
        
        public static bool IsUnauthorised(this HttpResponseMessage message) => message.StatusCode == HttpStatusCode.Unauthorized;
        
        
    }
}