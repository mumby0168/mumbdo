using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mumbdo.Web.Interfaces.Common;

namespace Mumbdo.Web.Interfaces.Proxies
{
    public interface IProxyHelper
    {
        Task ProcessResponseAsync(IHttpResponse response, ProxyBase proxy);

        Task<T> ProcessResponseAsync<T>(IHttpResponse<T> response, ProxyBase proxy) where T : class;
    }
}