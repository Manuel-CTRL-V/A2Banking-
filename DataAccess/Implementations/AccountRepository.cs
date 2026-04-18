using System;
using System.Collections.Generic;
using ATM.Shared.DTOs.Auth;
using ATM.Shared.DTOs.Transactions;
using ATM.Shared.Enums;
using ATM.Shared.Models;
using BankAPI.DataAccess.Exceptions;
using BankAPI.DataAccess.Helpers;
using BankAPI.DataAccess.Interfaces;

namespace BankAPI.DataAccess.Implementations
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        private readonly Logger _logger = Logger.Instance;

        // ── Auth ──────────────────────────────────────────────────────

        public AuthStartResponse GetAccountForAuth(int accountId)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_GetAccountForAuth", conn)
                    .With("@AccountId", accountId)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException(
                            "Cuenta no encontrada.", 50030);

                    return new AuthStartResponse
                    {
                        AccountId           = r.GetInt("AccountId"),
                        HolderName          = r.GetString("HolderName"),
                        Status              = (AccountStatus)r.GetByte("StatusId"),
                        PIN_Salt            = r.GetBytes("PIN_Salt"),
                        FingerprintTemplate = r.GetBytes("FingerprintTemplate")
                    };
                }
            });
        }

        public void VerifyPIN(VerifyPinRequest request)
        {
            Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_VerifyPIN", conn)
                    .With("@AccountId", request.AccountId)
                    .With("@PIN_Hash",  request.PIN_Hash)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException(
                            "Respuesta inesperada de sp_VerifyPIN.");
                }
            });
        }

        public CreateSessionResponse CreateSession(CreateSessionRequest request)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_CreateSession", conn)
                    .With("@AccountId",      request.AccountId)
                    .With("@ATM_Identifier", request.ATM_Identifier)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException("No se pudo crear la sesión.");

                    return new CreateSessionResponse
                    {
                        SessionId = Convert.ToInt32(r.GetValue(r.GetOrdinal("SessionId"))),
                        Token = ""  // El JWT lo genera el servicio de negocio
                    };
                }
            });
        }

        public void CloseSession(int sessionId, string reason)
        {
            Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_CloseSession", conn)
                    .With("@SessionId",         sessionId)
                    .With("@TerminationReason", reason)
                    .Build())
                {
                    cmd.ExecuteNonQuery();
                }
            });
        }

        // ── Operaciones bancarias ─────────────────────────────────────

        public BalanceResponse GetBalance(int accountId)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_GetBalance", conn)
                    .With("@AccountId", accountId)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException("Cuenta no encontrada.", 50050);

                    return new BalanceResponse
                    {
                        AccountId  = r.GetInt("AccountId"),
                        HolderName = r.GetString("HolderName"),
                        Balance    = r.GetDecimal("Balance"),
                        Status     = r.GetString("StatusName")
                    };
                }
            });
        }

        public DepositResponse Deposit(DepositRequest request)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_Deposit", conn)
                    .With("@AccountId", request.AccountId)
                    .With("@Amount", request.Amount)
                    .WithNullable("@SessionId", request.SessionId)
                    .WithNullable("@ATM_Identifier", request.ATM_Identifier)
                    .Build())
                {
                    cmd.CommandTimeout = 60;
                    using (var r = cmd.ExecuteReader())
                    {
                        if (!r.Read())
                            throw new BankDatabaseException("Respuesta inesperada de sp_Deposit.");

                        var result = new DepositResponse
                        {
                            TransactionId = r.GetInt("TransactionId"),
                            NewBalance = r.GetDecimal("NewBalance"),
                            Message = "Depósito realizado exitosamente."
                        };

                        _logger.LogInfo(
                            "Depósito completado. TxId: " + result.TransactionId +
                            ", Monto: " + request.Amount.ToString("N2") + " RD$" +
                            ", Nuevo saldo: " + result.NewBalance.ToString("N2") + " RD$",
                            accountId: request.AccountId,
                            sessionId: request.SessionId);

                        return result;
                    }
                }
            });
        }

        public WithdrawResponse Withdraw(WithdrawRequest request)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_Withdraw", conn)
                    .With("@AccountId",      request.AccountId)
                    .With("@Amount",         request.Amount)
                    .WithNullable("@SessionId",      request.SessionId)
                    .WithNullable("@ATM_Identifier", request.ATM_Identifier)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException("Respuesta inesperada de sp_Withdraw.");

                    var result = new WithdrawResponse
                    {
                        TransactionId = r.GetInt("TransactionId"),
                        NewBalance    = r.GetDecimal("NewBalance"),
                        Message       = "Retiro realizado exitosamente."
                    };

                    _logger.LogInfo(
                        "Retiro completado. TxId: " + result.TransactionId +
                        ", Monto: " + request.Amount.ToString("N2") + " RD$" +
                        ", Nuevo saldo: " + result.NewBalance.ToString("N2") + " RD$",
                        accountId: request.AccountId,
                        sessionId: request.SessionId);

                    return result;
                }
            });
        }

        public TransferResponse Transfer(TransferRequest request)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_Transfer", conn)
                    .With("@FromAccountId",  request.FromAccountId)
                    .With("@ToAccountId",    request.ToAccountId)
                    .With("@Amount",         request.Amount)
                    .WithNullable("@SessionId",      request.SessionId)
                    .WithNullable("@ATM_Identifier", request.ATM_Identifier)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException("Respuesta inesperada de sp_Transfer.");

                    var result = new TransferResponse
                    {
                        TransactionId     = r.GetInt("TransactionId"),
                        NewBalance        = r.GetDecimal("NewBalance"),
                        CommissionApplied = r.GetDecimal("CommissionApplied"),
                        CommissionCharged = r.GetBool("CommissionCharged"),
                        Message           = "Transferencia realizada exitosamente."
                    };

                    _logger.LogInfo(
                        "Transferencia completada. TxId: " + result.TransactionId +
                        ", Comisión: " + result.CommissionApplied.ToString("N2") + " RD$",
                        accountId: request.FromAccountId,
                        sessionId: request.SessionId);

                    return result;
                }
            });
        }

        public List<TransactionHistoryItem> GetHistory(TransactionHistoryRequest request)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_GetTransactionHistory", conn)
                    .WithNullable("@AccountId", request.AccountId)
                    .WithNullable("@FromDate",  request.FromDate)
                    .WithNullable("@ToDate",    request.ToDate)
                    .WithNullable("@TypeId",    request.TypeId)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    var list = new List<TransactionHistoryItem>();
                    while (r.Read())
                    {
                        list.Add(new TransactionHistoryItem
                        {
                            TransactionId    = r.GetInt("TransactionId"),
                            AccountId        = r.GetInt("AccountId"),
                            HolderName       = r.GetString("HolderName"),
                            TransactionType  = r.GetString("TransactionType"),
                            Amount           = r.GetDecimal("Amount"),
                            BalanceBefore    = r.GetDecimal("BalanceBefore"),
                            BalanceAfter     = r.GetDecimal("BalanceAfter"),
                            Status           = r.GetString("TransactionStatus"),
                            ATM_Identifier   = r.GetString("ATM_Identifier"),
                            Timestamp        = r.GetDateTime("Timestamp"),
                            FromAccountId    = r.GetNullableInt("FromAccountId"),
                            ToAccountId      = r.GetNullableInt("ToAccountId"),
                            CommissionAmount = r.GetNullableDecimal("CommissionAmount"),
                            DifferentHolder  = r.GetNullableBool("DifferentHolder")
                        });
                    }
                    return list;
                }
            });
        }

        public ChangePinResponse ChangePin(ChangePinRequest request)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_ChangePin", conn)
                    .With("@AccountId", request.AccountId)
                    .With("@OldPIN_Hash", request.OldPIN_Hash)
                    .With("@NewPIN_Hash", request.NewPIN_Hash)
                    .With("@NewPIN_Salt", request.NewPIN_Salt)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException(
                            "Respuesta inesperada de sp_ChangePin.");

                    _logger.LogInfo(
                        "PIN cambiado correctamente.",
                        accountId: request.AccountId,
                        sessionId: request.SessionId);

                    return new ChangePinResponse
                    {
                        AccountId = r.GetInt("AccountId"),
                        Message = r.GetString("Message")
                    };
                }
            });
        }

        public decimal GetDailyTransactionTotal(int accountId, TransactionType type)
        {
            var today = DateTime.Today;
            var request = new TransactionHistoryRequest
            {
                AccountId = accountId,
                FromDate = today,
                ToDate = today.AddDays(1).AddSeconds(-1),
                TypeId = (byte)type
            };

            var history = GetHistory(request);
            decimal total = 0;
            foreach (var item in history)
            {
                total += item.Amount;
            }
            return total;
        }

    }
}
