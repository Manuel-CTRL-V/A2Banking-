using System;
using System.Text;
using ATM.Shared.DTOs.BackOffice;
using BankAPI.DataAccess.Exceptions;
using BankAPI.DataAccess.Helpers;
using BankAPI.DataAccess.Interfaces;

namespace BankAPI.DataAccess.Implementations
{
    public class AccountManagementRepository : BaseRepository, IAccountManagementRepository
    {
        // Admin auth

        public GetAdminSaltResponse GetAdminSalt(string username)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_GetAdminSalt", conn)
                    .With("@Username", username)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException(
                            "Usuario no encontrado.", 50001);

                    return new GetAdminSaltResponse
                    {
                        PasswordSalt = r.GetBytes("PasswordSalt")
                    };
                }
            });
        }

        public AdminLoginResponse AdminLogin(AdminLoginRequest request)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_AdminLogin", conn)
                    .With("@Username", request.Username)
                    .With("@PasswordHash", request.PasswordHash)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException(
                            "Credenciales inválidas.", 50001);

                    var payload = r.GetInt("AdminId") + ":" +
                                  r.GetString("RoleName") + ":" +
                                  DateTime.UtcNow.Ticks;

                    var response = new AdminLoginResponse
                    {
                        AdminId = r.GetInt("AdminId"),
                        Username = r.GetString("Username"),
                        Role = r.GetString("RoleName"),
                        Token = Convert.ToBase64String(
                                       Encoding.UTF8.GetBytes(payload))
                    };

                    // Result set 2: permisos del rol
                    r.NextResult();
                    while (r.Read())
                        response.Permissions.Add(r.GetString("PermissionKey"));

                    return response;
                }
            });
        }

        // Cuentas

        public CreateAccountResponse CreateAccount(
            CreateAccountRequest request, int adminId)
        {
            return Execute(conn =>
            {
                // Upsert del personaje primero
                using (var cmdChar = SqlCommandBuilder
                    .For("sp_UpsertCharacter", conn)
                    .With("@ApiCharacterId", request.ApiCharacterId)
                    .With("@FullName", request.FullName)
                    .Build())
                using (var rChar = cmdChar.ExecuteReader())
                {
                    if (!rChar.Read())
                        throw new BankDatabaseException(
                            "No se pudo registrar el personaje.", 50010);

                    int characterId = rChar.GetInt("CharacterId");
                    rChar.Close();

                    // Crear la cuenta con el CharacterId obtenido
                    using (var cmdAcc = SqlCommandBuilder
                        .For("sp_CreateAccount", conn)
                        .With("@CharacterId", characterId)
                        .With("@CreatedByAdminId", adminId)
                        .With("@AccountTypeId", (byte)request.AccountTypeId)
                        .Build())
                    using (var rAcc = cmdAcc.ExecuteReader())
                    {
                        if (!rAcc.Read())
                            throw new BankDatabaseException(
                                "No se pudo crear la cuenta.", 50020);

                        return new CreateAccountResponse
                        {
                            AccountId = Convert.ToInt32(
                                rAcc.GetValue(rAcc.GetOrdinal("AccountId"))),
                            Status = "Pending",
                            Message = "Cuenta creada. Proceda con el enrollment biométrico."
                        };
                    }
                }
            });
        }

        public EnrollBiometricResponse EnrollBiometric(
            EnrollBiometricRequest request, int adminId)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_EnrollBiometric", conn)
                    .With("@AccountId", request.AccountId)
                    .With("@PIN_Hash", request.PIN_Hash)
                    .With("@PIN_Salt", request.PIN_Salt)
                    .WithMax("@FingerprintTemplate", request.FingerprintTemplate)
                    .With("@EnrolledByAdminId", adminId)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException(
                            "No se pudo completar el enrollment.", 50021);

                    return new EnrollBiometricResponse
                    {
                        AccountId = Convert.ToInt32(
                            r.GetValue(r.GetOrdinal("AccountId"))),
                        Status = "Active",
                        Message = "Enrollment completado. Cuenta activada."
                    };
                }
            });
        }

        public AccountListResponse GetAccounts()
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_GetAccounts", conn)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    var list = new AccountListResponse();
                    while (r.Read())
                    {
                        list.Accounts.Add(new AccountSummary
                        {
                            AccountId = r.GetInt("AccountId"),
                            HolderName = r.GetString("HolderName"),
                            Balance = r.GetDecimal("Balance"),
                            Status = r.GetString("StatusName"),
                            AccountType = r.GetString("AccountType"),
                            CreatedAt = r.GetDateTime("CreatedAt")
                        });
                    }
                    return list;
                }
            });
        }

        public UpdateAccountStatusResponse UpdateAccountStatus(
            UpdateAccountStatusRequest request, int adminId)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_UpdateAccountStatus", conn)
                    .With("@AccountId", request.AccountId)
                    .With("@NewStatusId", (byte)request.NewStatusId)
                    .With("@AdminId", adminId)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                        throw new BankDatabaseException(
                            "No se pudo actualizar el estado.", 50090);

                    return new UpdateAccountStatusResponse
                    {
                        AccountId = r.GetInt("AccountId"),
                        NewStatus = r.GetString("NewStatus"),
                        Message = "Estado actualizado correctamente."
                    };
                }
            });
        }

        public AuditLogResponse GetLogs(AuditLogRequest request)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_GetAuditLog", conn)
                    .WithNullable("@FromDate", request.FromDate)
                    .WithNullable("@ToDate", request.ToDate)
                    .WithNullable("@LevelId", request.LevelId)
                    .WithNullable("@AccountId", request.AccountId)
                    .Build())
                using (var r = cmd.ExecuteReader())
                {
                    var response = new AuditLogResponse();
                    while (r.Read())
                    {
                        response.Items.Add(new AuditLogItemDto
                        {
                            LogId = r.GetInt("LogId"),
                            Timestamp = r.GetDateTime("Timestamp"),
                            Level = r.GetString("LevelName"),
                            Message = r.GetString("Message"),
                            AccountId = r.GetNullableInt("AccountId"),
                            HolderName = r.GetString("HolderName"),
                            AdminId = r.GetNullableInt("AdminId"),
                            AdminName = r.GetString("AdminName"),
                            Source = r.GetString("Source")
                        });
                    }
                    return response;
                }
            });
        }
        public StatisticsResponse GetStatistics(StatisticsRequest request)
        {
            return Execute(conn =>
            {
                using (var cmd = SqlCommandBuilder
                    .For("sp_GetStatistics", conn)
                    .WithNullable("@FromDate", request.FromDate)
                    .WithNullable("@ToDate", request.ToDate)
                    .Build())
                using (var reader = cmd.ExecuteReader())
                {
                    var response = new StatisticsResponse();

                    // Result set 1: resumen
                    if (reader.Read())
                    {
                        response.Summary = new StatisticsSummary
                        {
                            ActiveAccounts = reader.GetInt("ActiveAccounts"),
                            TotalAccounts = reader.GetInt("TotalAccounts"),
                            TotalTransactions = reader.GetInt("TotalTransactions"),
                            TotalDeposits = reader.GetDecimal("TotalDeposits"),
                            TotalWithdraws = reader.GetDecimal("TotalWithdraws"),
                            TotalTransfers = reader.GetDecimal("TotalTransfers"),
                            TotalCommissions = reader.GetDecimal("TotalCommissions")
                        };
                    }

                    // Result set 2: transacciones por tipo y día
                    reader.NextResult();
                    while (reader.Read())
                    {
                        response.Transactions.Add(new TransactionStatItem
                        {
                            TransactionDate = reader.GetDateTime("TransactionDate"),
                            TransactionType = reader.GetString("TransactionType"),
                            Quantity = reader.GetInt("Quantity"),
                            TotalAmount = reader.GetDecimal("TotalAmount")
                        });
                    }

                    // Result set 3: cuentas por estado y tipo
                    reader.NextResult();
                    while (reader.Read())
                    {
                        response.Accounts.Add(new AccountStatItem
                        {
                            StatusName = reader.GetString("StatusName"),
                            AccountType = reader.GetString("AccountType"),
                            Quantity = reader.GetInt("Quantity"),
                            TotalBalance = reader.GetDecimal("TotalBalance")
                        });
                    }

                    return response;
                }
            });
        }
    }
}