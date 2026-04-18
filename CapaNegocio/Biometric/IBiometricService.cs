namespace ATM.Kiosk.Business.Biometric
{
    /// <summary>
    /// Contrato para la verificación biométrica con el DigitalPersona 4500.
    /// Separar en interfaz permite:
    ///   - Usar el stub mientras no hay dispositivo físico
    ///   - Cambiar a la implementación real sin tocar las strategies
    ///   - Testear la lógica de negocio sin hardware
    /// </summary>
    public interface IBiometricService
    {
        /// <summary>
        /// Indica si el dispositivo está conectado y disponible.
        /// La UI lo consulta al iniciar para informar al usuario.
        /// </summary>
        bool IsDeviceAvailable { get; }

        /// <summary>
        /// Captura la huella del dedo apoyado en el lector y la
        /// verifica contra el template almacenado de la cuenta.
        ///
        /// El template viene de ActiveSession.FingerprintTemplate
        /// — fue descargado del servidor durante el login y vive
        /// solo en memoria, nunca en disco.
        ///
        /// Bloquea el hilo hasta que el usuario apoya el dedo
        /// o se alcanza el timeout.
        /// </summary>
        /// <param name="storedTemplate">
        /// DPFP.Template serializado, obtenido de AuthStartResponse.
        /// </param>
        /// <returns>True si la huella coincide con el template.</returns>
        bool VerifyFingerprint(byte[] storedTemplate);
    }
}
