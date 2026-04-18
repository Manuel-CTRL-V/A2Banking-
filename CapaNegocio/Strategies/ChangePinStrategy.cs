using System.Security.Cryptography;
using ATM.Kiosk.Business.Auth;
using ATM.Kiosk.Business.Exceptions;
using ATM.Kiosk.Services.Implementations;
using ATM.Kiosk.Services.Interfaces;
using ATM.Shared.DTOs.Transactions;

namespace ATM.Kiosk.Business.Strategies
{
    /// <summary>
    /// Cambia el PIN de la cuenta activa.
    ///
    /// Flujo:
    ///   1. Usa el PIN_Salt guardado en sesión (descargado durante el login)
    ///   2. Hashea el PIN actual con ese salt para validar
    ///   3. Genera un salt nuevo para el nuevo PIN
    ///   4. Hashea el nuevo PIN con el salt nuevo
    ///   5. Envía todo al servidor → sp_ChangePin valida y actualiza
    /// </summary>
    public class ChangePinStrategy
    {
        private readonly ITransactionApiClient _apiClient;
        private readonly LocalLogger           _logger;

        public ChangePinStrategy(ITransactionApiClient apiClient)
        {
            _apiClient = apiClient;
            _logger    = LocalLogger.Instance;
        }

        public ChangePinResponse Execute(string oldPin, string newPin)
        {
            var session = SessionManager.Instance.RequireActiveSession();

            // Validaciones locales
            if (string.IsNullOrEmpty(oldPin) || oldPin.Length < 4)
                throw new BusinessException(
                    BusinessErrorCode.InvalidAmount,
                    "El PIN actual debe tener al menos 4 dígitos.");

            if (string.IsNullOrEmpty(newPin) || newPin.Length < 4)
                throw new BusinessException(
                    BusinessErrorCode.InvalidAmount,
                    "El nuevo PIN debe tener al menos 4 dígitos.");

            if (oldPin == newPin)
                throw new BusinessException(
                    BusinessErrorCode.InvalidAmount,
                    "El nuevo PIN debe ser diferente al actual.");

            // Hashear PIN actual usando el salt de la sesión activa
            var oldPinHash = PinHasher.Hash(oldPin, session.PIN_Salt);

            // Generar salt nuevo para el nuevo PIN
            var newSalt    = GenerateSalt();
            var newPinHash = PinHasher.Hash(newPin, newSalt);

            var request = new ChangePinRequest
            {
                AccountId   = session.AccountId,
                OldPIN_Hash = oldPinHash,
                NewPIN_Hash = newPinHash,
                NewPIN_Salt = newSalt,
                SessionId   = session.SessionId
            };

            var response = _apiClient.ChangePin(request);

            // Actualizar el salt en sesión para que futuros cambios de PIN
            // dentro de la misma sesión funcionen correctamente
            session.PIN_Salt = newSalt;

            _logger.LogInfo(
                "PIN cambiado exitosamente.",
                accountId: session.AccountId,
                sessionId: session.SessionId);

            return response;
        }

        private static byte[] GenerateSalt()
        {
            var salt = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);
            return salt;
        }
    }
}
