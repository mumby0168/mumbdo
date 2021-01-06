using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;
using Mumbdo.Shared.Urls;
using Mumbdo.Web.Interfaces.Authentication;
using Mumbdo.Web.Interfaces.Common;
using Mumbdo.Web.Interfaces.Proxies;
using Mumbdo.Web.Interfaces.Wrappers;

namespace Mumbdo.Web.Proxies
{
    public class GroupProxy : ProxyBase, IGroupProxy
    {
        private readonly IHttpClient _client;
        private readonly IAuthenticationService _authenticationService;
        private readonly IProxyHelper _proxyHelper;

        public GroupProxy(IHttpClient client, IAuthenticationService authenticationService, IJson json, IProxyHelper proxyHelper) : base(json)
        {
            _client = client;
            _authenticationService = authenticationService;
            _proxyHelper = proxyHelper;
        }

        public async Task<IEnumerable<ItemGroupDto>> GetGroupsAsync(bool includeTasks = false)
        {
            await AuthoriseAsync(_client, _authenticationService);
            var response = await _client.GetAsync<IEnumerable<ItemGroupDto>>(GroupUrls.GetGroupsUrl(includeTasks));
            return await _proxyHelper.ProcessResponseAsync(response, this);
        }

        public async Task<ItemGroupDto> GetAsync(Guid id, bool includeTasks = false)
        {
            await AuthoriseAsync(_client, _authenticationService);
            var response = await _client.GetAsync<ItemGroupDto>(GroupUrls.GetGroupsUrl(includeTasks));
            return await _proxyHelper.ProcessResponseAsync(response, this);
        }

        public async Task CreateAsync(string name, string description, string image)
        {
            await AuthoriseAsync(_client, _authenticationService);
            var response = await _client.PostAsync(GroupUrls.CreateGroupUrl, new CreateItemGroupDto(name, description, image));
            await _proxyHelper.ProcessResponseAsync(response, this);
        }
    }
}