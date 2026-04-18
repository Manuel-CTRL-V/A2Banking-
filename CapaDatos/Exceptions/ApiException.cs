using System;

namespace ATM.Kiosk.Services.Exceptions
{
    /// <summary>
    /// Excepción tipada para errores de comunicación con el BankAPI.
    ///
    /// Hay dos categorías de error que esta clase distingue:
    ///
    /// 1. Error de negocio (el servidor respondió, pero con Success=false):
    ///    ErrorCode > 0  →  el servidor procesó la petición y rechazó
    ///    la operación por una regla de negocio (PIN incorrecto, fondos
    ///    insuficientes, cuenta suspendida, etc.)
    ///
    /// 2. Error de infraestructura (no se pudo comunicar con el servidor):
    ///    ErrorCode = 0  →  timeout, red caída, servidor no disponible.
    ///    IsConnectionError = true.
    ///
    /// La capa de negocio hace switch(ex.ErrorCode) para mostrar
    /// el mensaje correcto en la UI sin depender de strings.
    /// </summary>
    public class ApiException : Exception
    {
        public int  ErrorCode        { get; }
        public bool IsConnectionError => ErrorCode == 0;

        public ApiException(string message)
            : base(message)
        {
            ErrorCode = 0;
        }

        public ApiException(int errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public ApiException(string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = 0;
        }

        // ── Propiedades semánticas que espeja BankDatabaseException ──
        // Así la capa de negocio puede usar el mismo vocabulario
        // sin importar si el error vino de red o de BD.

        public bool IsAccountNotFound => ErrorCode == 50020 || ErrorCode == 50030
                                       || ErrorCode == 50061 || ErrorCode == 50071;
        public bool IsAccountNotActive => ErrorCode == 50032 || ErrorCode == 50062
                                       || ErrorCode == 50072 || ErrorCode == 50084;
        public bool IsWrongPIN => ErrorCode == 50033;
        public bool IsInsufficientFunds => ErrorCode == 50073 || ErrorCode == 50086;
        public bool IsSameAccount => ErrorCode == 50081;
        public bool IsInvalidAmount => ErrorCode == 50060 || ErrorCode == 50070
                                       || ErrorCode == 50080;
        public bool IsSessionNotFound => ErrorCode == 50040;
        public bool IsDailyLimitExceeded => ErrorCode == 50090;
    }
}
