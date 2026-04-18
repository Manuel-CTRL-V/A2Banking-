using ATM.Kiosk.Business.Auth;
using ATM.Kiosk.Business.Biometric;
using ATM.Kiosk.Business.Exceptions;
using ATM.Kiosk.Services.Implementations;
using ATM.Kiosk.Services.Interfaces;
using ATM.Shared.DTOs.Transactions;

namespace ATM.Kiosk.Business.Strategies
{
    /// <summary>
    /// Realiza un retiro de la cuenta activa.
    /// Requiere verificación biométrica antes de ejecutar.
    ///
    /// Validación de fondos:
    ///   Se hace una verificación local preliminar comparando
    ///   contra el saldo cacheado en sesión, para dar feedback
    ///   inmediato al usuario sin ir al servidor. El servidor
    ///   hace la validación definitiva de forma atómica.
    ///   Esto no reemplaza la validación del servidor — es solo
    ///   para mejorar la experiencia de usuario.
    /// </summary>
    public class WithdrawStrategy : ITransactionStrategy
    {
        private readonly ITransactionApiClient _apiClient;
        private readonly IBiometricService     _biometric;
        private readonly LocalLogger           _logger;

        public WithdrawStrategy(ITransactionApiClient apiClient,
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
                    "El monto del retiro debe ser mayor a cero.");

            // Verificación local preliminar de fondos
            // (el saldo cacheado puede estar desactualizado si hubo
            // transacciones desde otro canal — el servidor decide)
            if (session.CurrentBalance > 0 &&
                parameters.Amount > session.CurrentBalance)
            {
                throw new BusinessException(
                    BusinessErrorCode.InvalidAmount,
                    "Fondos insuficientes. Su saldo disponible es " +
                    session.CurrentBalance.ToString("N2") + " RD$.");
            }

            _logger.LogInfo(
                "Iniciando retiro. Monto: " + parameters.Amount.ToString("N2") + " RD$",
                accountId: session.AccountId,
                sessionId: session.SessionId);

            // ── 3. Verificación biométrica ────────────────────────────
            bool biometricOk = _biometric.VerifyFingerprint(session.FingerprintTemplate);

            if (!biometricOk)
            {
                _logger.LogWarning(
                    "Retiro rechazado: verificación biométrica fallida.",
                    accountId: session.AccountId,
                    sessionId: session.SessionId);

                throw new BusinessException(
                    BusinessErrorCode.BiometricFailed,
                    "Verificación de identidad fallida. Intente de nuevo.");
            }

            // ── 4. Ejecutar retiro en servidor ────────────────────────
            var request = new WithdrawRequest
            {
                AccountId      = session.AccountId,
                Amount         = parameters.Amount,
                SessionId      = session.SessionId,
                ATM_Identifier = session.ATM_Identifier
            };

            var response = _apiClient.Withdraw(request);

            // ── 5. Actualizar estado local ────────────────────────────
            SessionManager.Instance.UpdateBalance(response.NewBalance);

            _logger.LogInfo(
                "Retiro completado. TxId: " + response.TransactionId +
                ", Nuevo saldo: " + response.NewBalance.ToString("N2") + " RD$",
                accountId: session.AccountId,
                sessionId: session.SessionId);

            return new TransactionResult
            {
                Success       = true,
                TransactionId = response.TransactionId,
                NewBalance    = response.NewBalance,
                Message       = "Retiro realizado exitosamente."
            };
        }
    }
}
