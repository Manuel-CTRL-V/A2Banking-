using ATM.Kiosk.Services.Implementations;

namespace ATM.Kiosk.Business.Biometric
{
    /// <summary>
    /// Implementación STUB del servicio biométrico.
    /// Usada mientras el dispositivo DigitalPersona 4500
    /// no está disponible físicamente.
    ///
    /// Reemplazar por BiometricService cuando
    /// llegue el dispositivo. El único cambio necesario es en
    /// la instanciación dentro de AuthService — las strategies
    /// no cambian porque trabajan con IBiometricService.
    ///
    /// VerifyFingerprint() siempre devuelve true para no bloquear
    /// el desarrollo de la UI y la lógica de negocio.
    /// </summary>
    public class BiometricServiceStub : IBiometricService
    {
        private readonly LocalLogger _logger = LocalLogger.Instance;

        public bool IsDeviceAvailable
        {
            get
            {
                _logger.LogWarning(
                    "[STUB] Dispositivo biométrico no disponible — " +
                    "usando stub de desarrollo.");
                return false;
            }
        }

        public bool VerifyFingerprint(byte[] storedTemplate)
        {
            _logger.LogWarning(
                "[STUB] Verificación biométrica omitida — " +
                "stub siempre aprueba. Reemplazar por BiometricService.");
            return true;
        }
    }
}
