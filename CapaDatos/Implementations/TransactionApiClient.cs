using ATM.Kiosk.Services.Interfaces;
using ATM.Shared.DTOs.Notifications;
using ATM.Shared.DTOs.Transactions;

namespace ATM.Kiosk.Services.Implementations
{
    /// <summary>
    /// Cliente HTTP para operaciones bancarias.
    /// Cada método traduce una acción del usuario en un request HTTP
    /// al BankAPI y devuelve el resultado tipado.
    ///
    /// ATM_Identifier y SessionId se incluyen en cada request para que
    /// el servidor pueda registrarlos en la tabla Transactions.
    /// </summary>
    public class TransactionApiClient : BaseApiClient, ITransactionApiClient
    {
        private readonly LocalLogger _logger = LocalLogger.Instance;

        /// <inheritdoc/>
        public BalanceResponse GetBalance(int accountId)
        {
            _logger.LogInfo("Consultando saldo", accountId: accountId);

            return Get<BalanceResponse>($"transactions/balance/{accountId}");
        }

        /// <inheritdoc/>
        public DepositResponse Deposit(DepositRequest request)
        {
            _logger.LogInfo(
                $"Enviando depósito. Monto: {request.Amount:N2} RD$",
                accountId: request.AccountId,
                sessionId: request.SessionId);

            var response = Post<DepositResponse>("transactions/deposit", request);

            _logger.LogInfo(
                $"Depósito confirmado por servidor. " +
                $"TxId: {response.TransactionId}, Nuevo saldo: {response.NewBalance:N2} RD$",
                accountId: request.AccountId,
                sessionId: request.SessionId);

            return response;
        }

        /// <inheritdoc/>
        public WithdrawResponse Withdraw(WithdrawRequest request)
        {
            _logger.LogInfo(
                $"Enviando retiro. Monto: {request.Amount:N2} RD$",
                accountId: request.AccountId,
                sessionId: request.SessionId);

            var response = Post<WithdrawResponse>("transactions/withdraw", request);

            _logger.LogInfo(
                $"Retiro confirmado por servidor. " +
                $"TxId: {response.TransactionId}, Nuevo saldo: {response.NewBalance:N2} RD$",
                accountId: request.AccountId,
                sessionId: request.SessionId);

            return response;
        }

        /// <inheritdoc/>
        public TransferResponse Transfer(TransferRequest request)
        {
            _logger.LogInfo(
                $"Enviando transferencia. " +
                $"De: {request.FromAccountId} → A: {request.ToAccountId}, " +
                $"Monto: {request.Amount:N2} RD$",
                accountId: request.FromAccountId,
                sessionId: request.SessionId);

            var response = Post<TransferResponse>("transactions/transfer", request);

            var commissionMsg = response.CommissionCharged
                ? $", Comisión: {response.CommissionApplied:N2} RD$"
                : ", Sin comisión";

            _logger.LogInfo(
                $"Transferencia confirmada. TxId: {response.TransactionId}" +
                $"{commissionMsg}, Nuevo saldo: {response.NewBalance:N2} RD$",
                accountId: request.FromAccountId,
                sessionId: request.SessionId);

            return response;
        }

        /// <inheritdoc/>
        public TransactionHistoryResponse GetHistory(TransactionHistoryRequest request)
        {
            return Post<TransactionHistoryResponse>("transactions/history", request);
        }
        public ChangePinResponse ChangePin(ChangePinRequest request)
        {
            _logger.LogInfo(
                "Enviando cambio de PIN.",
                accountId: request.AccountId,
                sessionId: request.SessionId);

            var response = Post<ChangePinResponse>("transactions/change-pin", request);

            _logger.LogInfo(
                "PIN cambiado correctamente.",
                accountId: request.AccountId,
                sessionId: request.SessionId);

            return response;
        }
        public NotificationSettingsResponse GetNotificationSettings(int accountId)
        {
            return Get<NotificationSettingsResponse>(
                "notifications/settings/" + accountId);
        }
        public void SaveNotificationSettings(NotificationSettingsRequest request)
        {
            Post<object>("notifications/settings", request);
        }
    }
}
