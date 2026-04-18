using System;
using System.IO;
using ATM.Shared.Enums;
using BankAPI.DataAccess.Configuration;
using BankAPI.DataAccess.Interfaces;

namespace BankAPI.DataAccess.Implementations
{
    /// <summary>
    /// Singleton Logger del servidor.
    /// Escribe simultáneamente en log.txt y en la tabla AuditLog de BD.
    /// El ATM solo escribe en su log.txt local — el log centralizado es este.
    /// </summary>
    public sealed class Logger
    {
        private static readonly Lazy<Logger> _instance =
            new Lazy<Logger>(() => new Logger());

        public static Logger Instance { get { return _instance.Value; } }

        private readonly ILogRepository _repo;
        private readonly string         _logFilePath;
        private readonly object         _lock = new object();

        private Logger()
        {
            _repo        = new LogRepository();
            _logFilePath = DatabaseConfig.LogFilePath;
            EnsureFileExists();
        }

        public void LogInfo(string msg, int? accountId = null, int? sessionId = null)
            => Write(LogLevel.Info, msg, accountId, sessionId: sessionId);

        public void LogWarning(string msg, int? accountId = null, int? sessionId = null)
            => Write(LogLevel.Warning, msg, accountId, sessionId: sessionId);

        public void LogError(string msg, int? accountId = null, int? sessionId = null)
            => Write(LogLevel.Error, msg, accountId, sessionId: sessionId);

        public void LogException(Exception ex, string context = "",
                                 int? accountId = null, int? sessionId = null)
        {
            var msg = string.IsNullOrEmpty(context) ? ex.Message : context + ": " + ex.Message;
            Write(LogLevel.Error, msg, accountId, sessionId: sessionId);
            WriteToFile(Format(LogLevel.Error,
                msg + Environment.NewLine + "  StackTrace: " + ex.StackTrace,
                accountId, null, sessionId));
        }

        private void Write(LogLevel level, string message,
                           int? accountId, int? adminId = null, int? sessionId = null)
        {
            var line = Format(level, message, accountId, adminId, sessionId);
            WriteToFile(line);
            _repo.Write(level, message, accountId, adminId, sessionId, "API");
        }

        private static string Format(LogLevel level, string message,
                                     int? accountId, int? adminId, int? sessionId)
        {
            string tag;
            switch (level)
            {
                case LogLevel.Warning: tag = "WARNING"; break;
                case LogLevel.Error:   tag = "ERROR  "; break;
                default:               tag = "INFO   "; break;
            }

            var ctx = "";
            if (accountId.HasValue) ctx += " [Account:" + accountId + "]";
            if (adminId.HasValue)   ctx += " [Admin:"   + adminId   + "]";
            if (sessionId.HasValue) ctx += " [Session:" + sessionId + "]";

            return "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]"
                 + " [" + tag + "]" + ctx + " " + message;
        }

        private void WriteToFile(string line)
        {
            try
            {
                lock (_lock)
                {
                    File.AppendAllText(_logFilePath,
                        line + Environment.NewLine,
                        System.Text.Encoding.UTF8);
                }
            }
            catch { }
        }

        private void EnsureFileExists()
        {
            try
            {
                var dir = Path.GetDirectoryName(_logFilePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                if (!File.Exists(_logFilePath))
                    File.WriteAllText(_logFilePath,
                        "=== BankAPI Log — iniciado "
                        + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        + " ===" + Environment.NewLine,
                        System.Text.Encoding.UTF8);
            }
            catch { }
        }
    }
}
