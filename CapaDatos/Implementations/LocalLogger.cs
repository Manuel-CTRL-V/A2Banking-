using System;
using System.IO;
using ATM.Kiosk.Services.Configuration;
using ATM.Kiosk.Services.Interfaces;
using ATM.Shared.Enums;

namespace ATM.Kiosk.Services.Implementations
{
    /// <summary>
    /// Singleton Logger del ATM. Escribe únicamente en log.txt local.
    ///
    /// A diferencia del Logger del servidor, este NO persiste en BD.
    /// El registro centralizado en AuditLog lo hace el BankAPI,
    /// que sí tiene acceso a SQL Server.
    ///
    /// El lock garantiza escrituras seguras si múltiples hilos
    /// (UI principal + timeout de sesión en segundo plano) loguean
    /// simultáneamente.
    /// </summary>
    public sealed class LocalLogger : ILocalLogger
    {
        private static readonly Lazy<LocalLogger> _instance =
            new Lazy<LocalLogger>(() => new LocalLogger(), isThreadSafe: true);

        public static LocalLogger Instance => _instance.Value;

        private readonly string _logFilePath;
        private readonly object _lock = new object();

        private LocalLogger()
        {
            _logFilePath = KioskConfig.LogFilePath;
            EnsureFileExists();
        }

        public void LogInfo(string message, int? accountId = null, int? sessionId = null)
            => Write(LogLevel.Info, message, accountId, sessionId);

        public void LogWarning(string message, int? accountId = null, int? sessionId = null)
            => Write(LogLevel.Warning, message, accountId, sessionId);

        public void LogError(string message, int? accountId = null, int? sessionId = null)
            => Write(LogLevel.Error, message, accountId, sessionId);

        public void LogException(Exception ex, string context = "",
                                 int? accountId = null, int? sessionId = null)
        {
            var msg = string.IsNullOrEmpty(context) ? ex.Message : $"{context}: {ex.Message}";
            Write(LogLevel.Error, msg, accountId, sessionId);

            // Stack trace completo solo en archivo — no se manda al servidor
            var full = Format(LogLevel.Error,
                $"{msg}{Environment.NewLine}  StackTrace: {ex.StackTrace}",
                accountId, sessionId);
            WriteToFile(full);
        }

        // ── Privados ──────────────────────────────────────────────

        private void Write(LogLevel level, string message,
                           int? accountId, int? sessionId)
        {
            var line = Format(level, message, accountId, sessionId);
            WriteToFile(line);
        }

        private static string Format(LogLevel level, string message,
                                     int? accountId, int? sessionId)
        {
            string tag;
            switch (level)
            {
                case LogLevel.Warning: tag = "WARNING"; break;
                case LogLevel.Error: tag = "ERROR  "; break;
                default: tag = "INFO   "; break;
            }

            var context = "";
            if (accountId.HasValue) context += " [Account:" + accountId + "]";
            if (sessionId.HasValue) context += " [Session:" + sessionId + "]";

            return "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]"
                 + " [" + tag + "]" + context + " " + message;
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
            catch
            {
                // Si el archivo falla no podemos hacer nada más.
                // La UI mostrará el resultado de la operación igual.
            }
        }

        private void EnsureFileExists()
        {
            try
            {
                var dir = Path.GetDirectoryName(_logFilePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (!File.Exists(_logFilePath))
                    File.WriteAllText(
                        _logFilePath,
                        "=== ATM Kiosk Log — " + KioskConfig.ATM_Identifier
                        + " — iniciado " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        + " ===" + Environment.NewLine,
                        System.Text.Encoding.UTF8);
            }
            catch { }
        }
    }
}
