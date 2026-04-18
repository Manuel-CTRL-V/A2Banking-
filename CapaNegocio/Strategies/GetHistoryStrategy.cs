using ATM.Kiosk.Business.Auth;
using ATM.Kiosk.Services.Implementations;
using ATM.Kiosk.Services.Interfaces;
using ATM.Shared.DTOs.Transactions;
using System.Collections.Generic;

namespace ATM.Kiosk.Business.Strategies
{
    public class GetHistoryStrategy : ITransactionStrategy
    {
        private readonly ITransactionApiClient _apiClient;
        private readonly LocalLogger _logger;

        public GetHistoryStrategy(ITransactionApiClient apiClient)
        {
            _apiClient = apiClient;
            _logger = LocalLogger.Instance;
        }

        public TransactionResult Execute(TransactionParameters parameters)
        {
            var session = SessionManager.Instance.RequireActiveSession();

            _logger.LogInfo("Consultando historial", accountId: session.AccountId,
                            sessionId: session.SessionId);

            var request = new TransactionHistoryRequest
            {
                AccountId = session.AccountId
            };

            var response = _apiClient.GetHistory(request);
            var items = response.Items ?? new List<TransactionHistoryItem>();

            _logger.LogInfo($"Historial obtenido: {items.Count} transacciones",
                accountId: session.AccountId,
                sessionId: session.SessionId);

            return new TransactionResult
            {
                Success = true,
                Message = $"Se encontraron {items.Count} transacciones."
            };
        }
    }
}
