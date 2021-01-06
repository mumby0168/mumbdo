using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MudBlazor.Utilities;
using Mumbdo.Application.Services;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Api.Controllers
{
    [Route("api/tasks")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var task = await _taskService.CreateAsync(dto);
            return Ok(task);
        }

        [HttpGet("ungrouped/{complete}")]
        public async Task<IActionResult> GetUngroupedAsync([FromRoute]bool complete)
        {
            var tasks = await _taskService.GetUngroupedTasksAsync(complete);
            return Ok(tasks);
        }
    }
}