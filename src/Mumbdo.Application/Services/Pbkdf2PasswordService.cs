

using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Mumbdo.Domain.ValueObjects;

namespace Mumbdo.Application.Services
{
    public class Pbkdf2PasswordService : IPasswordService
    {
        public bool IsStrongPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[_!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z]).{8,}$");
        }

        public Password HashPassword(string password, string salt = null, int? numberOfIterations = null)
        {
            salt ??= GenerateSalt();

            int iterations = numberOfIterations ?? new Random(DateTime.Now.Millisecond).Next(1000, 10000);
            var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), iterations);

            return new Password(
                Convert.ToBase64String(pbkdf2.GetBytes(PasswordSettings.HashLength)),
                Convert.ToBase64String(pbkdf2.Salt),
                Convert.ToBase64String(BitConverter.GetBytes(iterations)));
        }

        public bool CheckPassword(string plainTextPassword, Password encryptedPassword)
        {
            int iterations = BitConverter.ToInt32(Convert.FromBase64String(encryptedPassword.Iterations));

            var pbkdf2 = new Rfc2898DeriveBytes(plainTextPassword, Convert.FromBase64String(encryptedPassword.Salt),
                iterations);
            return Convert.ToBase64String(pbkdf2.GetBytes(PasswordSettings.HashLength)) == encryptedPassword.Hash;
        }

        public string GenerateSalt(int saltLength = PasswordSettings.SaltLength)
        {
            byte[] salt = new byte[saltLength];
            using (RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }
    }
}