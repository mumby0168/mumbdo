using System;
using System.Threading.Tasks;

namespace Mumbdo.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task SaveTokenAsync(Guid userId, string token, DateTime expires);

        Task<bool> IsTokenValid(Guid userId, string token);
    }
}