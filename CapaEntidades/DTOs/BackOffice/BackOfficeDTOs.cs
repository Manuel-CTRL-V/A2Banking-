using System;
using System.Collections.Generic;
using ATM.Shared.Enums;

namespace ATM.Shared.DTOs.BackOffice
{
    public class AdminLoginRequest
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }

    public class AdminLoginResponse
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public List<string> Permissions { get; set; }
            = new List<string>();
    }

    public class GetAdminSaltResponse
    {
        public byte[] PasswordSalt { get; set; }
    }

    // Simpsons API 

    public class SimpsonsCharacterDto
    {
        public int CharacterId { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public string PortraitPath { get; set; }
        public string PortraitUrl => "https://cdn.thesimpsonsapi.com/500" + PortraitPath;
    }

    // Creación de cuenta 

    public class CreateAccountRequest
    {
        public int ApiCharacterId { get; set; }
        public string FullName { get; set; }
        public decimal Balance { get; set; }
        public int AccountTypeId { get; set; }  // 1=Corriente, 2=Ahorros, 3=Nómina, 4=Estudiante
    }

    public class CreateAccountResponse
    {
        public int AccountId { get; set; }
        public string Status { get; set; }   // "Pending"
        public string Message { get; set; }
    }

    // Enrollment biométrico

    public class EnrollBiometricRequest
    {
        public int AccountId { get; set; }
        public byte[] PIN_Hash { get; set; }
        public byte[] PIN_Salt { get; set; }
        public byte[] FingerprintTemplate { get; set; }
    }

    public class EnrollBiometricResponse
    {
        public int AccountId { get; set; }
        public string Status { get; set; }   // "Active"
        public string Message { get; set; }
    }

    // Gestión de cuentas 

    public class AccountSummary
    {
        public int AccountId { get; set; }
        public string HolderName { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
        public string AccountType { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class AccountListResponse
    {
        public List<AccountSummary> Accounts { get; set; }
            = new List<AccountSummary>();
    }

    public class UpdateAccountStatusRequest
    {
        public int AccountId { get; set; }
        public int NewStatusId { get; set; }
    }

    public class UpdateAccountStatusResponse
    {
        public int AccountId { get; set; }
        public string NewStatus { get; set; }
        public string Message { get; set; }
    }

    // Logs de auditoría

    public class AuditLogRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? LevelId { get; set; }
        public int? AccountId { get; set; }
    }

    public class AuditLogItemDto
    {
        public int LogId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public int? AccountId { get; set; }
        public string HolderName { get; set; }
        public int? AdminId { get; set; }
        public string AdminName { get; set; }
        public string Source { get; set; }
    }

    public class AuditLogResponse
    {
        public List<AuditLogItemDto> Items { get; set; }
            = new List<AuditLogItemDto>();
    }

    public class StatisticsRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class StatisticsSummary
    {
        public int ActiveAccounts { get; set; }
        public int TotalAccounts { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalDeposits { get; set; }
        public decimal TotalWithdraws { get; set; }
        public decimal TotalTransfers { get; set; }
        public decimal TotalCommissions { get; set; }
    }

    public class TransactionStatItem
    {
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class AccountStatItem
    {
        public string StatusName { get; set; }
        public string AccountType { get; set; }
        public int Quantity { get; set; }
        public decimal TotalBalance { get; set; }
    }

    public class StatisticsResponse
    {
        public StatisticsSummary Summary { get; set; }
        public List<TransactionStatItem> Transactions { get; set; }
            = new List<TransactionStatItem>();
        public List<AccountStatItem> Accounts { get; set; }
            = new List<AccountStatItem>();
    }
}
