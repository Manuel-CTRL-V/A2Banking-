using System;
using ATM.Shared.Enums;

namespace ATM.Shared.Models
{
    /// <summary>
    /// Registro histórico de una sesión ATM tal como existe en la BD.
    /// El BackOffice y el BankAPI usan este modelo para auditar sesiones.
    ///
    /// Distinto de ActiveSession, que es el estado en memoria del ATM
    /// durante una sesión en curso.
    /// </summary>
    public class Session
    {
        public int                       SessionId         { get; set; }
        public int                       AccountId         { get; set; }
        public string                    ATM_Identifier    { get; set; }
        public DateTime                  StartedAt         { get; set; }
        public DateTime?                 EndedAt           { get; set; }
        public bool                      IsActive          { get; set; }
        public SessionTerminationReason? TerminationReason { get; set; }

        // Navegación
        public string                    HolderName        { get; set; }

        public TimeSpan? Duration => EndedAt.HasValue
            ? EndedAt.Value - StartedAt
            : (TimeSpan?)null;
    }

    /// <summary>
    /// Estado en memoria del ATM durante una sesión activa.
    /// Se crea tras autenticación completa (PIN + huella) y se
    /// destruye al hacer logout o por timeout de inactividad.
    ///
    /// Contiene todo lo necesario para operar sin volver a
    /// consultar el servidor en cada pantalla.
    ///
    /// FingerprintTemplate se guarda aquí temporalmente para
    /// re-verificar la huella en operaciones críticas dentro
    /// de la misma sesión, sin volver a pedirlo al servidor.
    /// Se limpia junto con la sesión — nunca se escribe a disco.
    /// </summary>
    public class ActiveSession
    {
        public int      SessionId           { get; set; }
        public int      AccountId           { get; set; }
        public string   HolderName          { get; set; }
        public decimal  CurrentBalance      { get; set; }
        public string   ATM_Identifier      { get; set; }
        public string   Token               { get; set; }
        public DateTime StartedAt           { get; set; }
        public byte[]   FingerprintTemplate { get; set; }
        public byte[] PIN_Salt { get; set; }  // para cambio de PIN
        public DateTime LastActivityAt      { get; set; }

        public void RefreshActivity() => LastActivityAt = DateTime.Now;
        public TimeSpan IdleTime => DateTime.Now - LastActivityAt;
    }
}
