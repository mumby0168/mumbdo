using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Mumbdo.Shared.Dtos;
using Mumbdo.Web.Interfaces.Managers;
using Mumbdo.Web.Interfaces.Wrappers;

namespace Mumbdo.Web.Managers
{
    public class TokenManager : ITokenManager
    {
        private readonly IJson _json;
        private readonly ILocalStorageManager _localStorageManager;
        private const string TokenStorageKey = "mumbdo-jwt-897654657890&^%$^^&*()_+(*&^%^";

        public TokenManager(IJson json, ILocalStorageManager localStorageManager)
        {
            _json = json;
            _localStorageManager = localStorageManager;
        }
        
        public async Task SaveTokenAsync(JwtTokenDto tokenDto)
        {
            var data = await _json.SerializeAsync(tokenDto);
            await _localStorageManager.Set(TokenStorageKey, data);
        }

        public async Task<JwtTokenDto> GetTokenAsync()
        {
            var json = await _localStorageManager.Get(TokenStorageKey);
            if (json is null || string.IsNullOrWhiteSpace(json))
                return null;
            
            return await _json.DeserializeAsync<JwtTokenDto>(json);
        }

        public async Task RemoveTokenAsync() 
            => await _localStorageManager.Remove(TokenStorageKey);
    }
}