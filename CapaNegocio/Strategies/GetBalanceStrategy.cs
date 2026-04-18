using ATM.Kiosk.Business.Auth;
using ATM.Kiosk.Services.Implementations;
using ATM.Kiosk.Services.Interfaces;

namespace ATM.Kiosk.Business.Strategies
{
    /// <summary>
    /// Consulta el saldo actual de la cuenta.
    /// Es la única operación que NO requiere verificación biométrica.
    /// Solo requiere sesión activa (JWT válido).
    /// </summary>
    public class GetBalanceStrategy : ITransactionStrategy
    {
        private readonly ITransactionApiClient _apiClient;
        private readonly LocalLogger           _logger;

        public GetBalanceStrategy(ITransactionApiClient apiClient)
        {
            _apiClient = apiClient;
            _logger    = LocalLogger.Instance;
        }

        public TransactionResult Execute(TransactionParameters parameters)
        {
            var session = SessionManager.Instance.RequireActiveSession();

            _logger.LogInfo("Consultando saldo", accountId: session.AccountId,
                            sessionId: session.SessionId);

            var response = _apiClient.GetBalance(session.AccountId);

            // Actualizar balance local para no volver a consultar
            SessionManager.Instance.UpdateBalance(response.Balance);

            _logger.LogInfo(
                "Saldo consultado: " + response.Balance.ToString("N2") + " RD$",
                accountId: session.AccountId,
                sessionId: session.SessionId);

            return new TransactionResult
            {
                Success    = true,
                NewBalance = response.Balance,
                Message    = "Saldo consultado exitosamente."
            };
        }
    }
}
