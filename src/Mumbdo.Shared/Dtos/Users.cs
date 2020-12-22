using System;

namespace Mumbdo.Shared.Dtos
{
    public record CreateUserDto(string Email, string Password);

    public record JwtTokenDto
    {
        public string Token { get; }
        
        public string Refresh { get; }
        
        public JwtTokenDto(string token, string refresh)
        {
            Token = token;
            Refresh = refresh;
        }
    }

    public record SignInDto(string Email, string Password);
}