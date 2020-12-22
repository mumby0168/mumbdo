

using Mumbdo.Domain.ValueObjects;

namespace Mumbdo.Application.Services
{
    public interface IPasswordService
        {
            bool IsStrongPassword(string password);
            Password HashPassword(string password, string salt = null, int? numberOfIterations = null);
            bool CheckPassword(string plainTextPassword, Password encryptedPassword);
            string GenerateSalt(int saltLength = 32);
        }
}