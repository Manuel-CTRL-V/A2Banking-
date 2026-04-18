using System;
using System.Security.Cryptography;
using System.Text;
using ATM.Shared.DTOs.BackOffice;
using BackOffice.Services.Implementations;

namespace Business.Auth
{
    public class AdminAuthService
    {
        private readonly BackOfficeApiClient _client;

        public AdminAuthService(BackOfficeApiClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Login en dos pasos:
        ///   1. Obtiene el salt del servidor
        ///   2. Hashea la contraseña y la verifica
        /// </summary>
        public void Login(string username, string password)
        {
            // Paso 1: obtener salt
            var saltResponse = _client.GetAdminSalt(username);
            if (saltResponse?.PasswordSalt == null)
                throw new BackOfficeApiException("Usuario no encontrado.");

            // Paso 2: hashear contraseña con el salt
            var hash = HashPassword(password, saltResponse.PasswordSalt);

            // Paso 3: enviar hash al servidor
            var request = new AdminLoginRequest
            {
                Username     = username,
                PasswordHash = hash,
                PasswordSalt = saltResponse.PasswordSalt
            };

            var response = _client.AdminLogin(request);

            // Abrir sesión en memoria
            AdminSessionManager.Instance.Open(response);
        }

        public void Logout()
        {
            AdminSessionManager.Instance.Close();
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            var passBytes = Encoding.UTF8.GetBytes(password);
            var combined  = new byte[passBytes.Length + salt.Length];
            Buffer.BlockCopy(passBytes, 0, combined, 0,              passBytes.Length);
            Buffer.BlockCopy(salt,      0, combined, passBytes.Length, salt.Length);

            using (var sha = SHA256.Create())
                return sha.ComputeHash(combined);
        }
    }
}
