using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mumbdo.Application.Exceptions;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Application.Jwt;
using Mumbdo.Domain.Aggregates;
using Mumbdo.Domain.Entities;
using Mumbdo.Domain.Exceptions;
using Mumbdo.Shared;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordService _passwordService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IUserAggregate _userAggregate;
        private readonly ITokenService _tokenService;

        public UserService(IPasswordService passwordService, IUserRepository userRepository, ILogger<UserService> logger, IUserAggregate userAggregate, ITokenService tokenService)
        {
            _passwordService = passwordService;
            _userRepository = userRepository;
            _logger = logger;
            _userAggregate = userAggregate;
            _tokenService = tokenService;
        }
        
        public async Task CreateAsync(CreateUserDto dto)
        {
            if (await _userRepository.IsEmailInUseAsync(dto.Email))
                throw new EmailInUseException(dto.Email);
            
            if (!_passwordService.IsStrongPassword(dto.Password))
                throw new PasswordToWeakException();

            var user = _userAggregate.Create(dto.Email, _passwordService.HashPassword(dto.Password));
            await _userRepository.SaveAsync(user);
        }

        public async Task<JwtTokenDto> SignInAsync(SignInDto dto)
        {
            IUser user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user is null)
                throw new InvalidUserCredentialsException();

            if (!_passwordService.CheckPassword(dto.Password, user.Password))
                throw new InvalidUserCredentialsException();

            var token = _tokenService.CreateToken(user);
            return new JwtTokenDto(token, "");
        }
    }
}