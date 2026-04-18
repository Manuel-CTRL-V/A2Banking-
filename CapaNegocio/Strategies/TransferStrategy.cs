using ATM.Kiosk.Business.Auth;
using ATM.Kiosk.Business.Biometric;
using ATM.Kiosk.Business.Exceptions;
using ATM.Kiosk.Services.Implementations;
using ATM.Kiosk.Services.Interfaces;
using ATM.Shared.DTOs.Transactions;

namespace ATM.Kiosk.Business.Strategies
{
    /// <summary>
    /// Realiza una transferencia entre cuentas.
    /// Requiere verificación biométrica.
    ///
    /// La comisión del 0.15% la calcula el servidor via SQL CLR
    /// (fn_CalculateCommission). El ATM muestra el resultado
    /// en TransactionResult.CommissionApplied para que la UI
    /// pueda informar al cliente cuánto se cobró.
    /// </summary>
    public class TransferStrategy : ITransactionStrategy
    {
        private readonly ITransactionApiClient _apiClient;
        private readonly IBiometricService     _biometric;
        private readonly LocalLogger           _logger;

        public TransferStrategy(ITransactionApiClient apiClient,
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

            // ── 2. Validaciones locales ───────────────────────────────
            if (parameters.Amount <= 0)
                throw new BusinessException(
                    BusinessErrorCode.InvalidAmount,
                    "El monto de la transferencia debe ser mayor a cero.");

            if (parameters.ToAccountId <= 0)
                throw new BusinessException(
                    BusinessErrorCode.InvalidAmount,
                    "Debe ingresar una cuenta de destino válida.");

            if (parameters.ToAccountId == session.AccountId)
                throw new BusinessException(
                    BusinessErrorCode.SameAccountTransfer,
                    "La cuenta de destino no puede ser la misma que la cuenta de origen.");

            _logger.LogInfo(
                "Iniciando transferencia. Destino: " + parameters.ToAccountId +
                ", Monto: " + parameters.Amount.ToString("N2") + " RD$",
                accountId: session.AccountId,
                sessionId: session.SessionId);

            // ── 3. Verificación biométrica ────────────────────────────
            bool biometricOk = _biometric.VerifyFingerprint(session.FingerprintTemplate);

            if (!biometricOk)
            {
                _logger.LogWarning(
                    "Transferencia rechazada: verificación biométrica fallida.",
                    accountId: session.AccountId,
                    sessionId: session.SessionId);

                throw new BusinessException(
                    BusinessErrorCode.BiometricFailed,
                    "Verificación de identidad fallida. Intente de nuevo.");
            }

            // ── 4. Ejecutar transferencia en servidor ─────────────────
            var request = new TransferRequest
            {
                FromAccountId  = session.AccountId,
                ToAccountId    = parameters.ToAccountId,
                Amount         = parameters.Amount,
                SessionId      = session.SessionId,
                ATM_Identifier = session.ATM_Identifier
            };

            var response = _apiClient.Transfer(request);

            // ── 5. Actualizar estado local ────────────────────────────
            SessionManager.Instance.UpdateBalance(response.NewBalance);

            var commissionMsg = response.CommissionCharged
                ? ", Comisión aplicada: " + response.CommissionApplied.ToString("N2") + " RD$"
                : " (sin comisión — mismo titular)";

            _logger.LogInfo(
                "Transferencia completada. TxId: " + response.TransactionId +
                commissionMsg + ", Nuevo saldo: " + response.NewBalance.ToString("N2") + " RD$",
                accountId: session.AccountId,
                sessionId: session.SessionId);

            return new TransactionResult
            {
                Success          = true,
                TransactionId    = response.TransactionId,
                NewBalance       = response.NewBalance,
                CommissionApplied = response.CommissionApplied,
                CommissionCharged = response.CommissionCharged,
                Message          = response.CommissionCharged
                    ? "Transferencia realizada. Comisión cobrada: " +
                      response.CommissionApplied.ToString("N2") + " RD$."
                    : "Transferencia realizada sin comisión."
            };
        }
    }
}
