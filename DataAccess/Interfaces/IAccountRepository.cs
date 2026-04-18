using System.Collections.Generic;
using ATM.Shared.DTOs;
using ATM.Shared.DTOs.Auth;
using ATM.Shared.DTOs.Transactions;
using ATM.Shared.Enums;

namespace BankAPI.DataAccess.Interfaces
{
    public interface IAccountRepository
    {
        // ── Auth ──────────────────────────────────────────────────────
        AuthStartResponse GetAccountForAuth(int accountId);
        void VerifyPIN(VerifyPinRequest request);
        CreateSessionResponse CreateSession(CreateSessionRequest request);
        void CloseSession(int sessionId, string reason);

        // ── Operaciones ───────────────────────────────────────────────
        BalanceResponse GetBalance(int accountId);
        DepositResponse Deposit(DepositRequest request);
        WithdrawResponse Withdraw(WithdrawRequest request);
        TransferResponse Transfer(TransferRequest request);
        List<TransactionHistoryItem> GetHistory(TransactionHistoryRequest request);
        ChangePinResponse ChangePin(ChangePinRequest request);

        // ── Límites diarios ───────────────────────────────────────────
        decimal GetDailyTransactionTotal(int accountId, TransactionType type);
    }
}
