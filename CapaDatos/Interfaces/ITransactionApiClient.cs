using ATM.Shared.DTOs.Notifications;
using ATM.Shared.DTOs.Transactions;

namespace ATM.Kiosk.Services.Interfaces
{
    /// <summary>
    /// Contrato para todas las operaciones bancarias del ATM.
    /// Todos los métodos requieren un JWT válido en el header,
    /// que el ApiClient incluye automáticamente desde la sesión activa.
    /// </summary>
    public interface ITransactionApiClient
    {
        /// <summary>
        /// Consulta el saldo actual de la cuenta.
        /// Llama: GET /api/transactions/balance/{accountId}
        /// No requiere re-autenticación — el JWT de sesión es suficiente.
        /// </summary>
        BalanceResponse GetBalance(int accountId);

        /// <summary>
        /// Realiza un depósito.
        /// Llama: POST /api/transactions/deposit
        /// Lanza ApiException con IsInvalidAmount=true si amount <= 0.
        /// </summary>
        DepositResponse Deposit(DepositRequest request);

        /// <summary>
        /// Realiza un retiro.
        /// Llama: POST /api/transactions/withdraw
        /// Lanza ApiException con IsInsufficientFunds=true si no alcanza.
        /// </summary>
        WithdrawResponse Withdraw(WithdrawRequest request);

        /// <summary>
        /// Realiza una transferencia entre cuentas.
        /// La comisión la calcula el servidor via SQL CLR.
        /// Llama: POST /api/transactions/transfer
        /// </summary>
        TransferResponse Transfer(TransferRequest request);

        /// <summary>
        /// Obtiene el historial de transacciones.
        /// Llama: POST /api/transactions/history
        /// </summary>
        TransactionHistoryResponse GetHistory(TransactionHistoryRequest request);

        /// <summary>
        /// Cambia el PIN de la cuenta activa.
        /// El ATM hashea el PIN viejo y el nuevo antes de enviarlo.
        /// POST /api/transactions/change-pin
        /// </summary>
        ChangePinResponse ChangePin(ChangePinRequest request);
        NotificationSettingsResponse GetNotificationSettings(int accountId);
        void SaveNotificationSettings(NotificationSettingsRequest request);

    }
}
