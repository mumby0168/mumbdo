using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mumbdo.Application.Services;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Api.Controllers
{
    [Route("api/groups")]
    [Authorize]
    public class ItemGroupController : ControllerBase
    {
        private readonly ILogger<ItemGroupController> _logger;
        private readonly IItemGroupService _itemGroupService;

        public ItemGroupController(ILogger<ItemGroupController> logger, IItemGroupService itemGroupService)
        {
            _logger = logger;
            _itemGroupService = itemGroupService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemGroupDto dto)
        {
            await _itemGroupService.CreateAsync(dto);
            return Ok();
        }

        [HttpGet("{includeTasks}")]
        public async Task<IActionResult> GetAllForUserAsync([FromRoute] bool includeTasks = true)
        {
            var groups = await _itemGroupService.GetAllForUserAsync(includeTasks);
            return Ok(groups);
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupAsync([FromQuery] Guid groupId, [FromQuery] bool includeTasks)
        {
            ItemGroupDto groupDto = await _itemGroupService.GetAsync(groupId, includeTasks);
            return Ok(groupDto);
        }
    }
}