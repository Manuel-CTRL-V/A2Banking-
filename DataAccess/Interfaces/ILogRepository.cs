using ATM.Shared.DTOs.Auth;
using ATM.Shared.DTOs.Transactions;
using ATM.Shared.Models;

namespace BankAPI.DataAccess.Interfaces
{
    public interface ILogRepository
    {
        /// <summary>
        /// Persiste en AuditLog via sp_WriteLog.
        /// Nunca lanza excepción — el log no interrumpe el flujo.
        /// </summary>
        void Write(ATM.Shared.Enums.LogLevel level,
                   string  message,
                   int?    accountId = null,
                   int?    adminId   = null,
                   int?    sessionId = null,
                   string  source    = "API");
    }
}
