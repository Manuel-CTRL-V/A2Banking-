using ATM.Shared.DTOs.Transactions;
using ATM.Shared.Enums;
using BankAPI.DataAccess.Configuration;
using BankAPI.DataAccess.Exceptions;
using BankAPI.DataAccess.Implementations;
using BankAPI.DataAccess.Interfaces;

namespace BankAPI.Business.Services
{
    /// <summary>
    /// Lógica de negocio de operaciones bancarias en el servidor.
    /// Valida los requests antes de pasarlos al repositorio.
    /// La atomicidad de cada operación la garantiza el stored procedure.
    /// </summary>
    public class TransactionService
    {
        private readonly IAccountRepository _repo;
        private readonly Logger             _logger;

        public TransactionService(IAccountRepository repo)
        {
            _repo   = repo;
            _logger = Logger.Instance;
        }

        public BalanceResponse GetBalance(int accountId)
        {
            return _repo.GetBalance(accountId);
        }

        public DepositResponse Deposit(DepositRequest request)
        {
            if (request.Amount <= 0)
                throw new BankDatabaseException(
                    "El monto del depósito debe ser mayor a cero.", 50060);

            ValidateDailyLimit(request.AccountId, TransactionType.Deposit, request.Amount, DatabaseConfig.DailyDepositLimit);

            return _repo.Deposit(request);
        }

        public WithdrawResponse Withdraw(WithdrawRequest request)
        {
            if (request.Amount <= 0)
                throw new BankDatabaseException(
                    "El monto del retiro debe ser mayor a cero.", 50070);

            ValidateDailyLimit(request.AccountId, TransactionType.Withdraw, request.Amount, DatabaseConfig.DailyWithdrawLimit);

            return _repo.Withdraw(request);
        }

        public TransferResponse Transfer(TransferRequest request)
        {
            if (request.Amount <= 0)
                throw new BankDatabaseException(
                    "El monto de la transferencia debe ser mayor a cero.", 50080);

            if (request.FromAccountId == request.ToAccountId)
                throw new BankDatabaseException(
                    "Las cuentas de origen y destino no pueden ser la misma.", 50081);

            ValidateDailyLimit(request.FromAccountId, TransactionType.Transfer, request.Amount, DatabaseConfig.DailyTransferLimit);

            return _repo.Transfer(request);
        }

        public TransactionHistoryResponse GetHistory(TransactionHistoryRequest request)
        {
            var items = _repo.GetHistory(request);
            return new TransactionHistoryResponse { Items = items };
        }

        public ChangePinResponse ChangePin(ChangePinRequest request)
        {
            if (request.NewPIN_Hash == null || request.NewPIN_Hash.Length == 0)
                throw new BankDatabaseException("El nuevo PIN no puede estar vacío.", 50060);

            if (request.NewPIN_Salt == null || request.NewPIN_Salt.Length == 0)
                throw new BankDatabaseException("El salt no puede estar vacío.", 50060);

            return _repo.ChangePin(request);
        }

        private void ValidateDailyLimit(int accountId, TransactionType type, decimal amount, decimal limit)
        {
            var todayTotal = _repo.GetDailyTransactionTotal(accountId, type);
            
            _logger.LogInfo(
                $"[LIMIT CHECK] accountId={accountId}, type={type}, amount={amount:N2} RD$, used={todayTotal:N2} RD$, limit={limit:N2} RD$",
                accountId: accountId);
            
            if (todayTotal + amount > limit)
            {
                var typeName = type switch
                {
                    TransactionType.Deposit => "depósito",
                    TransactionType.Withdraw => "retiro",
                    TransactionType.Transfer => "transferencia",
                    _ => "transacción"
                };
                throw new BankDatabaseException(
                    $"El monto excede el límite diario de {typeName} de {limit:N2} RD$. " +
                    $"Ha utilizado {todayTotal:N2} RD$ hoy.", 50090);
            }
            
            _logger.LogInfo($"[LIMIT OK] {todayTotal:N2} RD$ + {amount:N2} RD$ <= {limit:N2} RD$");
        }
    }
}
