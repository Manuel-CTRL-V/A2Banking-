using System;
using System.Security.Cryptography;
using System.Text;

namespace BankAPI.DataAccess.Helpers
{
    /// <summary>
    /// Algoritmo de hash de PIN — DEBE ser idéntico al del ATM.
    /// Si este algoritmo cambia, el ATM no podrá autenticarse.
    ///
    /// SHA256( UTF8(pin) + salt )
    /// </summary>
    public static class PinHasher
    {
        public static byte[] GenerateSalt()
        {
            var salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);
            return salt;
        }

        public static byte[] Hash(string pin, byte[] salt)
        {
            if (string.IsNullOrEmpty(pin))
                throw new ArgumentException("El PIN no puede estar vacío.");
            if (salt == null || salt.Length == 0)
                throw new ArgumentException("El salt no puede estar vacío.");

            var pinBytes = Encoding.UTF8.GetBytes(pin);
            var combined = new byte[pinBytes.Length + salt.Length];
            Buffer.BlockCopy(pinBytes, 0, combined, 0,             pinBytes.Length);
            Buffer.BlockCopy(salt,     0, combined, pinBytes.Length, salt.Length);

            using (var sha = SHA256.Create())
                return sha.ComputeHash(combined);
        }

        public static bool Verify(string pin, byte[] salt, byte[] storedHash)
        {
            var computed = Hash(pin, salt);
            return ConstantTimeEquals(computed, storedHash);
        }

        private static bool ConstantTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}
