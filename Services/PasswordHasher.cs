using Konscious.Security.Cryptography;
using ShoppingCart.Interfaces.IApp;
using System.Security.Cryptography;
using System.Text;

namespace ShoppingCart.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int _keySize = 32;
        private const int _saltSize = 16;
        private const int _iterations = 3;
        private const int _memory = 65536;
        private const int _parallelism = 2;

        public bool CompareHashes(string currentPasswordHash, string password)
        {
            byte[] hashAndSalt = Convert.FromBase64String(currentPasswordHash);

            byte[] hash = new byte[_keySize];
            byte[] salt = new byte[_saltSize];

            Buffer.BlockCopy(hashAndSalt, 0, hash, 0, _keySize);
            Buffer.BlockCopy(hashAndSalt, _keySize, salt, 0, _saltSize);

            Argon2id argon = CreateArgon(salt, password);

            byte[] unknownHash = argon.GetBytes(_keySize);

            return CryptographicOperations.FixedTimeEquals(unknownHash, hash);
        }

        public string CreateHash(string password)
        {
            if (password == null) return string.Empty;

            byte[] salt = RandomNumberGenerator.GetBytes(_saltSize);

            var argon = CreateArgon(salt, password);

            var hash = argon.GetBytes(_keySize);

            byte[] hashAndSalt = new byte[_keySize + _saltSize];

            Buffer.BlockCopy(hash, 0, hashAndSalt, 0, _keySize);
            Buffer.BlockCopy(salt, 0, hashAndSalt, _keySize, _saltSize);

            return  Convert.ToBase64String(hashAndSalt);
        }

        private Argon2id CreateArgon(byte[] salt, string password)
        {
            return new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                DegreeOfParallelism = _parallelism,
                MemorySize = _memory,
                Iterations = _iterations,
                Salt = salt
            };
        }
    }
}
