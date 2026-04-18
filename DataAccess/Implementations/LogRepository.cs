using ATM.Shared.Enums;
using BankAPI.DataAccess.Helpers;
using BankAPI.DataAccess.Interfaces;
using System;

namespace BankAPI.DataAccess.Implementations
{
    public class LogRepository : BaseRepository, ILogRepository
    {
        public void Write(LogLevel level, string message,
                          int? accountId = null, int? adminId = null,
                          int? sessionId = null, string source = "API")
        {
            try
            {
                Execute(conn =>
                {
                    using (var cmd = SqlCommandBuilder
                        .For("sp_WriteLog", conn)
                        .With("@LevelId",  (byte)level)
                        .With("@Message",  message ?? "")
                        .WithNullable("@AccountId", accountId)
                        .WithNullable("@AdminId",   adminId)
                        .WithNullable("@SessionId", sessionId)
                        .With("@Source",   source ?? "API")
                        .Build())
                    {
                        cmd.ExecuteNonQuery();
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "[LogRepository] Fallo al persistir en BD: " + ex.Message);
            }
        }
    }
}
