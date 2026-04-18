using ATM.Kiosk.Services.Interfaces;
using ATM.Shared.DTOs.Auth;

namespace ATM.Kiosk.Services.Implementations
{
    /// <summary>
    /// Cliente HTTP para el flujo de autenticación.
    /// Implementa los tres pasos: obtener datos, verificar PIN, crear sesión.
    /// </summary>
    public class AuthApiClient : BaseApiClient, IAuthApiClient
    {
        private readonly LocalLogger _logger = LocalLogger.Instance;

        /// <inheritdoc/>
        public AuthStartResponse GetAccountForAuth(int accountId)
        {
            _logger.LogInfo($"Iniciando autenticación para cuenta {accountId}");

            var response = Get<AuthStartResponse>(
                $"auth/start/{accountId}");

            _logger.LogInfo(
                $"Datos de cuenta obtenidos. Titular: {response.HolderName}, " +
                $"Estado: {response.Status}",
                accountId: accountId);

            return response;
        }

        /// <inheritdoc/>
        public VerifyPinResponse VerifyPin(VerifyPinRequest request)
        {
            // El PIN ya viene hasheado desde la capa de negocio.
            // Esta capa solo lo transporta — nunca ve el PIN plano.
            var response = Post<VerifyPinResponse>("auth/verify-pin", request);

            _logger.LogInfo("PIN verificado correctamente", accountId: request.AccountId);

            return response;
        }

        /// <inheritdoc/>
        public CreateSessionResponse CreateSession(CreateSessionRequest request)
        {
            var response = Post<CreateSessionResponse>("auth/create-session", request);

            // Guardar el JWT para que todos los requests posteriores
            // lo incluyan automáticamente en el header Authorization.
            SetSessionToken(response.Token);

            _logger.LogInfo(
                $"Sesión creada. SessionId: {response.SessionId}",
                accountId: request.AccountId,
                sessionId: response.SessionId);

            return response;
        }

        /// <inheritdoc/>
        public void CloseSession(CloseSessionRequest request)
        {
            try
            {
                PostVoid("auth/close-session", request);
                _logger.LogInfo(
                    $"Sesión cerrada. Razón: {request.TerminationReason}",
                    sessionId: request.SessionId);
            }
            finally
            {
                // Limpiar el token siempre, aunque el servidor falle.
                // El ATM no debe quedarse con un token de una sesión cerrada.
                ClearSessionToken();
            }
        }
    }
}
