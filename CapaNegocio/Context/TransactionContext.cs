using ATM.Kiosk.Business.Auth;
using ATM.Kiosk.Business.Biometric;
using ATM.Kiosk.Business.Strategies;
using ATM.Kiosk.Services.Interfaces;
using ATM.Shared.DTOs.Transactions;
using System.Collections.Generic;

namespace ATM.Kiosk.Business.Context
{
    /// <summary>
    /// Contexto del patrón Strategy para operaciones bancarias.
    /// Es el único punto que la UI toca para ejecutar operaciones.
    ///
    /// La UI no conoce las strategies concretas — solo llama
    /// ExecuteDeposit(), ExecuteWithdraw(), etc. El contexto
    /// instancia la strategy correcta, la ejecuta y devuelve
    /// el resultado.
    ///
    /// También actúa como Factory implícita: centraliza la
    /// creación de las strategies con sus dependencias.
    /// Si en el futuro una strategy necesita una dependencia
    /// nueva, el cambio es aquí — no en la UI.
    /// </summary>
    public class TransactionContext
    {
        private readonly ITransactionApiClient _apiClient;
        private readonly IBiometricService     _biometric;

        public TransactionContext(ITransactionApiClient apiClient,
                                  IBiometricService     biometric)
        {
            _apiClient = apiClient;
            _biometric = biometric;
        }

        // ── Ejecución de strategies ───────────────────────────────────

        public TransactionResult ExecuteDeposit(decimal amount)
        {
            var strategy = new DepositStrategy(_apiClient, _biometric);
            var parameters = new TransactionParameters { Amount = amount };
            return strategy.Execute(parameters);
        }

        public TransactionResult ExecuteWithdraw(decimal amount)
        {
            var strategy = new WithdrawStrategy(_apiClient, _biometric);
            var parameters = new TransactionParameters { Amount = amount };
            return strategy.Execute(parameters);
        }

        public TransactionResult ExecuteTransfer(decimal amount, int toAccountId)
        {
            var strategy = new TransferStrategy(_apiClient, _biometric);
            var parameters = new TransactionParameters
            {
                Amount      = amount,
                ToAccountId = toAccountId
            };
            return strategy.Execute(parameters);
        }

        public TransactionResult ExecuteGetBalance()
        {
            var strategy = new GetBalanceStrategy(_apiClient);
            return strategy.Execute(new TransactionParameters());
        }

        public List<TransactionHistoryItem> ExecuteGetHistory()
        {
            var strategy = new GetHistoryStrategy(_apiClient);
            var result = strategy.Execute(new TransactionParameters());

            var session = SessionManager.Instance.RequireActiveSession();
            var request = new TransactionHistoryRequest
            {
                AccountId = session.AccountId
            };
            var response = _apiClient.GetHistory(request);
            return response.Items ?? new List<TransactionHistoryItem>();
        }
    }
}
