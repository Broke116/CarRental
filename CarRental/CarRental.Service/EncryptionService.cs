using System;
using System.Security.Cryptography;
using System.Text;
using CarRental.Service.Interfaces;

namespace CarRental.Service
{
    public class EncryptionService : IEncryptionService
    {
        public string CreateSalt()
        {
            var data = new byte[0x10];
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                cryptoServiceProvider.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }

        public string EncryptPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = $"{salt}{password}";
                byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordBytes));
            }
        }
    }
}
