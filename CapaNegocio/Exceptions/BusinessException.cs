using System;

namespace ATM.Kiosk.Business.Exceptions
{
    /// <summary>
    /// Excepción para violaciones de reglas de negocio detectadas
    /// en esta capa, antes de llegar al servidor.
    ///
    /// Ejemplos:
    ///   - Monto ingresado es cero o negativo
    ///   - Cuenta de origen igual a destino en transferencia
    ///   - Sesión expirada por inactividad
    ///   - Autenticación biométrica fallida
    ///
    /// Distinta de ApiException (error del servidor) y de
    /// excepciones de infraestructura (red caída, timeout).
    /// La UI captura esta excepción para mostrar mensajes
    /// amigables sin necesidad de parsear textos.
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessErrorCode Code { get; }

        public BusinessException(BusinessErrorCode code, string message)
            : base(message)
        {
            Code = code;
        }
    }

    public enum BusinessErrorCode
    {
        InvalidAmount,           // Monto <= 0
        SameAccountTransfer,     // Origen == destino
        SessionExpired,          // Inactividad excedida
        BiometricFailed,         // Huella no verificada
        BiometricDeviceError,    // Dispositivo no disponible
        AccountNotActive,        // Cuenta no está activa
        WrongPin,                // PIN incorrecto (detectado localmente)
        GeneralError             // Cualquier otro caso
    }
}
