using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Mumbdo.Application.Configuration;
using Mumbdo.Application.Exceptions;
using Mumbdo.Application.Interfaces.Repositories;
using Mumbdo.Domain.Entities;
using Mumbdo.Shared.Dtos;

namespace Mumbdo.Application.Jwt
{
    public class TokenService : ITokenService
    {
        private readonly ISettings _settings;

        public TokenService(ISettings settings)
        {
            _settings = settings;
        }
        
        public string CreateToken(IUserEntity userEntity)
        {
            var descriptor = new SecurityTokenDescriptor();
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userEntity.Id.ToString()),
                new (ClaimTypes.Email, userEntity.Email),
                new (ClaimTypes.Role, userEntity.Role)
            };
            var identity = new ClaimsIdentity(claims);
            descriptor.Subject = identity;
            descriptor.Expires = DateTime.Now.AddHours(3);
            descriptor.SigningCredentials =
                new SigningCredentials(_settings.JwtKey, SecurityAlgorithms.HmacSha512Signature);
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        
    }
}