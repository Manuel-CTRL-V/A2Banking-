using System;
using System.Collections.Generic;

namespace ATM.Shared.DTOs.Transactions
{
    // Depósito

    public class DepositRequest
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public int SessionId { get; set; }
        public string ATM_Identifier { get; set; }
    }

    public class DepositResponse
    {
        public int TransactionId { get; set; }
        public decimal NewBalance { get; set; }
        public string Message { get; set; }
    }

    // Retiro

    public class WithdrawRequest
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public int SessionId { get; set; }
        public string ATM_Identifier { get; set; }
    }

    public class WithdrawResponse
    {
        public int TransactionId { get; set; }
        public decimal NewBalance { get; set; }
        public string Message { get; set; }
    }

    // Transferencia 

    public class TransferRequest
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public int SessionId { get; set; }
        public string ATM_Identifier { get; set; }
    }

    public class TransferResponse
    {
        public int TransactionId { get; set; }
        public decimal NewBalance { get; set; }
        public decimal CommissionApplied { get; set; }
        public bool CommissionCharged { get; set; }
        public string Message { get; set; }
    }

    // Consulta de saldo

    public class BalanceRequest
    {
        public int AccountId { get; set; }
    }

    public class BalanceResponse
    {
        public int AccountId { get; set; }
        public string HolderName { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public string AccountType { get; set; }
    }

    // Historial

    public class TransactionHistoryRequest
    {
        public int? AccountId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? TypeId { get; set; }
    }

    public class TransactionHistoryItem
    {
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string HolderName { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public string Status { get; set; }
        public string ATM_Identifier { get; set; }
        public DateTime Timestamp { get; set; }

        // Campos presentes solo si es transferencia
        public int? FromAccountId { get; set; }
        public int? ToAccountId { get; set; }
        public decimal? CommissionAmount { get; set; }
        public bool? DifferentHolder { get; set; }
    }

    public class TransactionHistoryResponse
    {
        public List<TransactionHistoryItem> Items { get; set; }
            = new List<TransactionHistoryItem>();
    }

    public class ChangePinRequest
    {
        public int AccountId { get; set; }
        public byte[] OldPIN_Hash { get; set; }
        public byte[] NewPIN_Hash { get; set; }
        public byte[] NewPIN_Salt { get; set; }
        public int SessionId { get; set; }
    }

    public class ChangePinResponse
    {
        public int AccountId { get; set; }
        public string Message { get; set; }
    }
}
