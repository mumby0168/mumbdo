using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mumbdo.Shared;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Interfaces.Proxies;

namespace Mumbdo.Web.Proxies
{
    public class ProxyHelper : IProxyHelper
    {
        public async Task ProcessResponseAsync(IHttpResponse response, ProxyBase proxy)
        {
            if (response.RefusedConnection)
            {
                proxy.SetConnectionRefusedError();
                return;
            }
            
            if(response.Message.IsSuccessStatusCode)
                return;

            if (response.Message.IsBad())
            {
                await proxy.ProcessErrorAsync(response.Message);
                return;
            }

            if (response.Message.IsUnauthorised())
            {
                await proxy.ProcessErrorAsync(response.Message);
                return;
            }
            
            proxy.SetConnectionRefusedError();
        }

        public async Task<T> ProcessResponseAsync<T>(IHttpResponse<T> response, ProxyBase proxy) where T : class
        {
            if (response.RefusedConnection)
            {
                proxy.SetConnectionRefusedError();
                return null;
            }

            if (response.Message.IsSuccessStatusCode)
                return response.Data;

            if (response.Message.IsBad())
            {
                await proxy.ProcessErrorAsync(response.Message);
                return null;
            }
            
            if (response.Message.IsUnauthorised())
            {
                await proxy.ProcessErrorAsync(response.Message);
                return null;
            }
            
            proxy.SetConnectionRefusedError();
            return null;
        }
    }
}