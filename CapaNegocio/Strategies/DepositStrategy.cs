using ATM.Kiosk.Business.Auth;
using ATM.Kiosk.Business.Biometric;
using ATM.Kiosk.Business.Exceptions;
using ATM.Kiosk.Services.Implementations;
using ATM.Kiosk.Services.Interfaces;
using ATM.Shared.DTOs.Transactions;

namespace ATM.Kiosk.Business.Strategies
{
    /// <summary>
    /// Realiza un depósito a la cuenta activa.
    /// Requiere verificación biométrica antes de ejecutar.
    ///
    /// Validaciones locales (antes de llamar al servidor):
    ///   - Sesión activa y no expirada
    ///   - Monto mayor a cero
    ///   - Huella verificada
    ///
    /// El servidor valida adicionalmente:
    ///   - Cuenta activa (Status == Active)
    ///   - Atomicidad de la operación (UPDATE + INSERT en una transacción)
    /// </summary>
    public class DepositStrategy : ITransactionStrategy
    {
        private readonly ITransactionApiClient _apiClient;
        private readonly IBiometricService     _biometric;
        private readonly LocalLogger           _logger;

        public DepositStrategy(ITransactionApiClient apiClient,
                               IBiometricService     biometric)
        {
            _apiClient = apiClient;
            _biometric = biometric;
            _logger    = LocalLogger.Instance;
        }

        public TransactionResult Execute(TransactionParameters parameters)
        {
            // ── 1. Validar sesión ─────────────────────────────────────
            var session = SessionManager.Instance.RequireActiveSession();

            // ── 2. Validar monto ──────────────────────────────────────
            if (parameters.Amount <= 0)
                throw new BusinessException(
                    BusinessErrorCode.InvalidAmount,
                    "El monto del depósito debe ser mayor a cero.");

            _logger.LogInfo(
                "Iniciando depósito. Monto: " + parameters.Amount.ToString("N2") + " RD$",
                accountId: session.AccountId,
                sessionId: session.SessionId);

            // ── 3. Verificación biométrica ────────────────────────────
            bool biometricOk = _biometric.VerifyFingerprint(session.FingerprintTemplate);

            if (!biometricOk)
            {
                _logger.LogWarning(
                    "Depósito rechazado: verificación biométrica fallida.",
                    accountId: session.AccountId,
                    sessionId: session.SessionId);

                throw new BusinessException(
                    BusinessErrorCode.BiometricFailed,
                    "Verificación de identidad fallida. Intente de nuevo.");
            }

            // ── 4. Ejecutar depósito en servidor ──────────────────────
            var request = new DepositRequest
            {
                AccountId      = session.AccountId,
                Amount         = parameters.Amount,
                SessionId      = session.SessionId,
                ATM_Identifier = session.ATM_Identifier
            };

            var response = _apiClient.Deposit(request);

            // ── 5. Actualizar estado local ────────────────────────────
            SessionManager.Instance.UpdateBalance(response.NewBalance);

            _logger.LogInfo(
                "Depósito completado. TxId: " + response.TransactionId +
                ", Nuevo saldo: " + response.NewBalance.ToString("N2") + " RD$",
                accountId: session.AccountId,
                sessionId: session.SessionId);

            return new TransactionResult
            {
                Success       = true,
                TransactionId = response.TransactionId,
                NewBalance    = response.NewBalance,
                Message       = "Depósito realizado exitosamente."
            };
        }
    }
}
