using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using DnsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mumbdo.Application.Services;
using Mumbdo.Infrastructure.Documents;
using Mumbdo.Shared;

namespace Mumbdo.Api.Controllers
{
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public UsersController(IUserService userService, ILogger<UsersController> logger, IMongoRepository<UserDocument, Guid> repository)
        {
            _userService = userService;
            _logger = logger;
            _repository = repository;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] CreateUserDto dto)
        {
            _logger.LogInformation($@"Attempting to sign up user with email {dto.Email}");
            await _userService.CreateAsync(dto);
            _logger.LogInformation($"Successfully signed up user with email {dto.Email}");
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _repository.FindAsync(u => u.Email.Contains("@"));
            return Ok(users);
        }
    }
}