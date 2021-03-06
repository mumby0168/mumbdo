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
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICurrentUserService _currentUserService;

        public UserService(
            IPasswordService passwordService, 
            IUserRepository userRepository, 
            ILogger<UserService> logger, 
            IUserAggregate userAggregate, 
            ITokenService tokenService, 
            IRefreshTokenRepository refreshTokenRepository, 
            ICurrentUserService currentUserService)
        {
            _passwordService = passwordService;
            _userRepository = userRepository;
            _logger = logger;
            _userAggregate = userAggregate;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
            _currentUserService = currentUserService;
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
            IUserEntity userEntity = await _userRepository.GetByEmailAsync(dto.Email);
            if (userEntity is null)
                throw new InvalidUserCredentialsException();

            if (!_passwordService.CheckPassword(dto.Password, userEntity.Password))
                throw new InvalidUserCredentialsException();

            var token = _tokenService.CreateToken(userEntity);
            var refresh = _passwordService.GenerateSalt();
            await _refreshTokenRepository.SaveTokenAsync(userEntity.Id, refresh, DateTime.Now.AddHours(3));
            return new JwtTokenDto(token, refresh);
        }
        
        public async Task<JwtTokenDto> RefreshAsync(string refreshToken, string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user is null)
                throw new InvalidRefreshTokenException();

            if (!await _refreshTokenRepository.IsTokenValid(user.Id, refreshToken))
                throw new InvalidRefreshTokenException();
            
            var newToken = _tokenService.CreateToken(user);
            return new JwtTokenDto(newToken, refreshToken);
        }
    }
}