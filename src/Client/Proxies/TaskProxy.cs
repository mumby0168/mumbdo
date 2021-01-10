using System;
using System.Collections.Generic;
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

        public async Task<TaskDto> UpdateAsync(Guid id, string name, bool isComplete, Guid? groupId, DateTime? deadline)
        {
            await AuthoriseAsync(_client, _authenticationService);
            var response = await _client.PutAsync<UpdateTaskDto, TaskDto>(TaskUrls.UpdateTaskUrl,
                new UpdateTaskDto(id, name, isComplete, groupId, deadline));
            return await _proxyHelper.ProcessResponseAsync(response, this);
        }

        public async Task DeleteAsync(Guid taskId)
        {
            await AuthoriseAsync(_client, _authenticationService);
            var response = await _client.DeleteAsync(TaskUrls.DeleteTaskUrl(taskId));
            await _proxyHelper.ProcessResponseAsync(response, this);
        }

        public async Task<IEnumerable<TaskDto>> GetUngroupedTasksAsync(bool completedTasks = false)
        {
            await AuthoriseAsync(_client, _authenticationService);
            var response = await _client.GetAsync<IEnumerable<TaskDto>>(TaskUrls.UngroupedTasksUrl(completedTasks));
            return await _proxyHelper.ProcessResponseAsync(response, this);
        }
    }
}