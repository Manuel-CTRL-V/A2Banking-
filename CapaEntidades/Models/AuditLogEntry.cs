using System;
using ATM.Shared.Enums;

namespace ATM.Shared.Models
{
    /// <summary>
    /// Entrada en el log de auditoría centralizado (tabla AuditLog).
    ///
    /// El BankAPI persiste aquí cada operación via sp_WriteLog.
    /// El BackOffice consulta este modelo para mostrar el historial
    /// de actividad al Supervisor.
    ///
    /// El ATM también escribe en su log.txt local, pero ese es
    /// un archivo plano — no un modelo del dominio.
    ///
    /// AccountId y AdminId son nullable porque algunos eventos
    /// del sistema (arranque del servidor, errores de infraestructura)
    /// no están asociados a ninguna entidad específica.
    /// </summary>
    public class AuditLogEntry
    {
        public int      LogId     { get; set; }
        public DateTime Timestamp { get; set; }
        public LogLevel Level     { get; set; }
        public string   Message   { get; set; }
        public int?     AccountId { get; set; }
        public int?     AdminId   { get; set; }
        public int?     SessionId { get; set; }

        /// <summary>"ATM" | "API" | "BackOffice" | "System"</summary>
        public string   Source    { get; set; }

        // Navegación — poblados opcionalmente para mostrar en UI
        public string   HolderName  { get; set; }
        public string   AdminName   { get; set; }
    }
}
