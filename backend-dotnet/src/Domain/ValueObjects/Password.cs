using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class Password
    {
        public string Hash { get; private set; }

        private Password(string hash) => Hash = hash;

        public static Password CreateHashed(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentException("Contraseña vacía");

            // Hash PBKDF2
            using var rng = new Rfc2898DeriveBytes(plainPassword, 16, 10000, HashAlgorithmName.SHA256);
            var salt = rng.Salt;
            var hash = rng.GetBytes(32);
            var hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 32);
            return new Password(Convert.ToBase64String(hashBytes));
        }

        public bool Verify(string plainPassword)
        {
            var hashBytes = Convert.FromBase64String(Hash);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            using var rng = new Rfc2898DeriveBytes(plainPassword, salt, 10000, HashAlgorithmName.SHA256);
            var hash2 = rng.GetBytes(32);
            for (int i = 0; i < 32; i++)
                if (hashBytes[i + 16] != hash2[i]) return false;
            return true;
        }
    }
}
