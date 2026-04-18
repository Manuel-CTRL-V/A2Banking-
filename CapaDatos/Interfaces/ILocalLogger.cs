using ATM.Shared.Enums;

namespace ATM.Kiosk.Services.Interfaces
{
    /// <summary>
    /// Logger local del ATM. Solo escribe en log.txt.
    /// NO tiene acceso a la base de datos — esa responsabilidad
    /// es del servidor. El ATM registra localmente y el servidor
    /// registra en AuditLog de forma centralizada.
    /// </summary>
    public interface ILocalLogger
    {
        void LogInfo(string message, int? accountId = null, int? sessionId = null);
        void LogWarning(string message, int? accountId = null, int? sessionId = null);
        void LogError(string message, int? accountId = null, int? sessionId = null);
        void LogException(System.Exception ex, string context = "",
                          int? accountId = null, int? sessionId = null);
    }
}
