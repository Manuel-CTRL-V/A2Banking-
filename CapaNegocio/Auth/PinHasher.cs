using System;
using System.Security.Cryptography;
using System.Text;

namespace ATM.Kiosk.Business.Auth
{
    /// <summary>
    /// Hashea el PIN del cliente antes de enviarlo al servidor.
    /// El PIN nunca viaja en texto plano — ni en la red ni en logs.
    ///
    /// Flujo en el ATM:
    ///   1. sp_GetAccountForAuth (via ApiClient) devuelve PIN_Salt
    ///   2. PinHasher.Hash(pinIngresado, salt) computa el hash
    ///   3. El hash se envía a POST /api/auth/verify-pin
    ///   4. El servidor compara contra lo almacenado en BD
    ///
    /// El salt vive en el servidor — el ATM lo recibe solo
    /// durante el login y lo descarta después.
    /// </summary>
    public static class PinHasher
    {
        private const int SaltSize = 32;  // 256 bits

        /// <summary>
        /// Genera un salt nuevo. Solo lo usa el BackOffice
        /// durante el enrollment — no el ATM durante el login.
        /// Está aquí porque PinHasher es el único lugar del
        /// sistema que conoce la estrategia de hash.
        /// </summary>
        public static byte[] GenerateSalt()
        {
            var salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        /// <summary>
        /// Computa SHA-256(PIN_bytes + salt_bytes).
        /// El PIN se convierte a UTF-8 antes de concatenar.
        /// </summary>
        public static byte[] Hash(string pin, byte[] salt)
        {
            if (string.IsNullOrEmpty(pin))
                throw new ArgumentException("El PIN no puede estar vacío.", "pin");
            if (salt == null || salt.Length == 0)
                throw new ArgumentException("El salt no puede estar vacío.", "salt");

            var pinBytes = Encoding.UTF8.GetBytes(pin);
            var combined = new byte[pinBytes.Length + salt.Length];

            Buffer.BlockCopy(pinBytes, 0, combined, 0,             pinBytes.Length);
            Buffer.BlockCopy(salt,     0, combined, pinBytes.Length, salt.Length);

            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(combined);
            }
        }

        /// <summary>
        /// Compara dos hashes en tiempo constante para evitar
        /// ataques de timing. Nunca uses == para comparar hashes.
        /// </summary>
        public static bool Verify(string pin, byte[] salt, byte[] storedHash)
        {
            var computed = Hash(pin, salt);
            return ConstantTimeEquals(computed, storedHash);
        }

        private static bool ConstantTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];

            return diff == 0;
        }
    }
}
