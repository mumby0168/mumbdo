using System;
using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;
using Mumbdo.Shared.Urls;
using Mumbdo.Web.Interfaces.Authentication;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Interfaces.Proxies;
using Mumbdo.Web.Interfaces.Wrappers;

namespace Mumbdo.Web.Proxies
{
    public class TaskProxy : ProxyBase, ITaskProxy
    {
        private readonly IHttpClient _client;
        private readonly IAuthenticationService _authenticationService;
        private readonly IProxyHelper _proxyHelper;

        public TaskProxy(IHttpClient client, IAuthenticationService authenticationService, IProxyHelper proxyHelper, IJson json) : base(json)
        {
            _client = client;
            _authenticationService = authenticationService;
            _proxyHelper = proxyHelper;
        }
        
        public async Task CreateAsync(string name, Guid? groupId = null, DateTime? deadline = null)
        {
            await AuthoriseAsync(_client, _authenticationService);
            var response = await _client.PostAsync<CreateTaskDto, TaskDto>(TaskUrls.CreateTaskUrl,
                new CreateTaskDto(name, groupId, deadline));
            await _proxyHelper.ProcessResponseAsync(response, this);
        }
    }
}