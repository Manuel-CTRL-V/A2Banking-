using ATM.Kiosk.Business.Auth;
using ATM.Kiosk.Business.Biometric;
using ATM.Kiosk.Business.Exceptions;
using ATM.Kiosk.Services.Implementations;
using ATM.Kiosk.Services.Interfaces;
using ATM.Shared.DTOs.Auth;
using ATM.Kiosk.Services.Configuration;

namespace ATM.Kiosk.Business.Context
{
    /// <summary>
    /// Orquesta el flujo completo de autenticación del ATM.
    /// Coordina los tres pasos: obtener datos → verificar PIN → biométrico.
    ///
    /// La UI llama:
    ///   1. StartAuth(accountId)      → obtiene nombre del titular
    ///   2. VerifyPin(pin)            → valida contra servidor
    ///   3. VerifyBiometric()         → valida huella local
    ///   4. CompleteAuth()            → crea sesión en servidor
    ///
    /// La separación en pasos permite que la UI muestre pantallas
    /// distintas para cada fase y maneje cada error específicamente.
    ///
    /// Estado intermedio (entre pasos):
    ///   AuthService guarda el AuthStartResponse entre Step 1 y Step 4
    ///   para no volver a pedirlo al servidor.
    /// </summary>
    public class AuthService
    {
        private readonly IAuthApiClient    _authClient;
        private readonly IBiometricService _biometric;
        private readonly LocalLogger       _logger;

        // Estado intermedio entre pasos del flujo
        private AuthStartResponse _pendingAuth;

        public AuthService(IAuthApiClient    authClient,
                           IBiometricService biometric)
        {
            _authClient = authClient;
            _biometric  = biometric;
            _logger     = LocalLogger.Instance;
        }

        // ── Paso 1: obtener datos de la cuenta ────────────────────────

        /// <summary>
        /// Consulta el servidor para obtener el nombre del titular,
        /// el PIN_Salt y el FingerprintTemplate.
        /// Devuelve el nombre del titular para mostrarlo en la UI.
        /// Lanza ApiException si la cuenta no existe o no está activa.
        /// </summary>
        public string StartAuth(int accountId)
        {
            _pendingAuth = null;

            _logger.LogInfo("Iniciando autenticación para cuenta: " + accountId);

            var response = _authClient.GetAccountForAuth(accountId);

            if (response.Status != ATM.Shared.Enums.AccountStatus.Active)
                throw new BusinessException(
                    BusinessErrorCode.AccountNotActive,
                    "La cuenta no está activa. Contacte al banco.");

            _pendingAuth = response;

            _logger.LogInfo(
                "Cuenta encontrada. Titular: " + response.HolderName,
                accountId: accountId);

            return response.HolderName;
        }

        // ── Paso 2: verificar PIN ─────────────────────────────────────

        /// <summary>
        /// Hashea el PIN con el salt recibido en Step 1 y lo
        /// envía al servidor para comparación.
        /// Lanza BusinessException con WrongPin si no coincide.
        /// </summary>
        public void VerifyPin(string pin)
        {
            EnsurePendingAuth();

            var hash = PinHasher.Hash(pin, _pendingAuth.PIN_Salt);

            var request = new VerifyPinRequest
            {
                AccountId = _pendingAuth.AccountId,
                PIN_Hash  = hash
            };

            // ApiException con IsWrongPIN=true si el servidor rechaza
            _authClient.VerifyPin(request);

            _logger.LogInfo("PIN verificado.", accountId: _pendingAuth.AccountId);
        }

        // ── Paso 3: verificar huella ──────────────────────────────────

        /// <summary>
        /// Captura y verifica la huella contra el template descargado.
        /// La verificación es local — el SDK compara sin enviar datos
        /// biométricos al servidor.
        /// </summary>
        public void VerifyBiometric()
        {
            EnsurePendingAuth();

            bool ok = _biometric.VerifyFingerprint(_pendingAuth.FingerprintTemplate);

            if (!ok)
            {
                _logger.LogWarning(
                    "Verificación biométrica fallida.",
                    accountId: _pendingAuth.AccountId);

                throw new BusinessException(
                    BusinessErrorCode.BiometricFailed,
                    "Verificación de identidad fallida. Intente de nuevo.");
            }

            _logger.LogInfo("Huella verificada.", accountId: _pendingAuth.AccountId);
        }

        // ── Paso 4: completar autenticación ───────────────────────────

        /// <summary>
        /// Notifica al servidor que PIN + biométrico fueron exitosos,
        /// crea la sesión en BD y abre la sesión local en memoria.
        /// Después de esto, SessionManager.Instance.Current tiene datos.
        /// </summary>
        public void CompleteAuth()
        {
            EnsurePendingAuth();

            var sessionRequest = new CreateSessionRequest
            {
                AccountId      = _pendingAuth.AccountId,
                ATM_Identifier = KioskConfig.ATM_Identifier
            };

            var sessionResponse = _authClient.CreateSession(sessionRequest);

            SessionManager.Instance.Open(
                sessionResponse,
                _pendingAuth,
                KioskConfig.ATM_Identifier);

            _logger.LogInfo(
                "Sesión abierta. SessionId: " + sessionResponse.SessionId,
                accountId: _pendingAuth.AccountId,
                sessionId: sessionResponse.SessionId);

            // Limpiar datos intermedios — ya no se necesitan
            _pendingAuth = null;
        }

        // ── Logout ────────────────────────────────────────────────────

        /// <summary>
        /// Cierra la sesión activa. Notifica al servidor y limpia
        /// el estado local incluyendo el JWT y el template en memoria.
        /// </summary>
        public void Logout(string reason = "UserLogout")
        {
            var session = SessionManager.Instance.Current;
            if (session == null) return;

            try
            {
                _authClient.CloseSession(new CloseSessionRequest
                {
                    SessionId         = session.SessionId,
                    TerminationReason = reason
                });
            }
            finally
            {
                // Cerrar siempre en local aunque el servidor falle
                SessionManager.Instance.Close();
                _logger.LogInfo("Sesión cerrada. Razón: " + reason);
            }
        }

        // ── Privados ──────────────────────────────────────────────────

        private void EnsurePendingAuth()
        {
            if (_pendingAuth == null)
                throw new BusinessException(
                    BusinessErrorCode.GeneralError,
                    "El flujo de autenticación no fue iniciado correctamente.");
        }
    }
}
