using System;
using System.Threading.Tasks;

namespace Mumbdo.Application.Interfaces.Utilities
{
    public interface IEntityValidator
    {
        Task<bool> IsGroupIdValidAsync(Guid groupId);
    }
}