using ATM.Shared.DTOs.Transactions;
using BankAPI.Business.Services;
using BankAPI.DataAccess.Helpers;
using BankAPI.DataAccess.Implementations;
using BankAPI.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Route("api/transactions")]
    public class TransactionController : BaseApiController
    {
        private readonly TransactionService _txService;
        private readonly NotificationService _notifService;

        public TransactionController(IConfiguration config)
        {
            var repo = new AccountRepository();
            _txService = new TransactionService(repo);
            _notifService = new NotificationService(
                new NotificationRepository(), config);
        }

        // GET api/transactions/balance/{accountId}
        [HttpGet("balance/{accountId:int}")]
        public IActionResult GetBalance(int accountId)
        {
            return ExecuteSafe(() => _txService.GetBalance(accountId));
        }

        // POST api/transactions/deposit
        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] DepositRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() =>
            {
                var result = _txService.Deposit(request);
                var balance = _txService.GetBalance(request.AccountId);

                _notifService.NotifyTransaction(
                    request.AccountId,
                    balance.HolderName,
                    "Depósito",
                    request.Amount,
                    result.NewBalance);

                return result;
            });
        }

        // POST api/transactions/withdraw
        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() =>
            {
                var result = _txService.Withdraw(request);
                var balance = _txService.GetBalance(request.AccountId);

                _notifService.NotifyTransaction(
                    request.AccountId,
                    balance.HolderName,
                    "Retiro",
                    request.Amount,
                    result.NewBalance);

                return result;
            });
        }

        // POST api/transactions/transfer
        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] TransferRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() =>
            {
                var result = _txService.Transfer(request);
                var balance = _txService.GetBalance(request.FromAccountId);

                _notifService.NotifyTransaction(
                    request.FromAccountId,
                    balance.HolderName,
                    "Transferencia",
                    request.Amount,
                    result.NewBalance,
                    result.CommissionApplied);

                return result;
            });
        }

        // POST api/transactions/history
        [HttpPost("history")]
        public IActionResult GetHistory([FromBody] TransactionHistoryRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _txService.GetHistory(request));
        }

        // POST api/transactions/change-pin
        [HttpPost("change-pin")]
        public IActionResult ChangePin([FromBody] ChangePinRequest request)
        {
            if (request == null) return BadRequest("Request inválido.");
            return ExecuteSafe(() => _txService.ChangePin(request));
        }
    }
}
