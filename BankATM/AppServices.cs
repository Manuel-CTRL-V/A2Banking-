using ATM.Kiosk.Business.Biometric;
using ATM.Kiosk.Business.Context;
using ATM.Kiosk.Services.Implementations;
using ATM.Kiosk.Services.Interfaces;

namespace ATM.Kiosk
{
    /// <summary>
    /// Punto único de construcción de dependencias.
    /// Se inicializa una sola vez en Program.cs al arrancar la app.
    ///
    /// La presentación solo usa las propiedades de esta clase —
    /// nunca hace "new TransactionApiClient()" directamente.
    /// Así si mañana cambia la implementación concreta (ej: de
    /// BiometricServiceStub a BiometricService), solo cambia aquí.
    /// </summary>
    public static class AppServices
    {
        public static AuthService Auth { get; private set; }
        public static TransactionContext Transactions { get; private set; }
        public static ITransactionApiClient ApiClient { get; private set; }

        public static void Initialize()
        {
            // Capas de servicios (acceso a datos vía HTTP)

            var authApiClient = new AuthApiClient();
            var transactionApiClient = new TransactionApiClient();
            IBiometricService biometric = new BiometricService();

            Auth = new AuthService(authApiClient, biometric);
            Transactions = new TransactionContext(transactionApiClient, biometric);
            ApiClient = transactionApiClient;
        }

        /// <summary>
        /// Reinicia las dependencias entre sesiones.
        /// Llamar después de cada logout para que el AuthService
        /// empiece limpio sin estado intermedio de la sesión anterior.
        /// </summary>
        public static void Reset()
        {
            Initialize();
        }
    }
}

