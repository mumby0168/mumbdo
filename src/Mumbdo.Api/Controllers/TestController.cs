using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mumbdo.Domain.Entities;

namespace Mumbdo.Api.Controllers
{
    [Route("test")]
    public class TestController : ControllerBase
    {
        [Authorize(Roles = Roles.User)]
        [HttpGet("auth")]
        public IActionResult Auth()
        {
            return Ok($"You have a token {HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}");
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult Get()
        {
            return Ok("Hi there no auth needed here");
        }
        
    }
}