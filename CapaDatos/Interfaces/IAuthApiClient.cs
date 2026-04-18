using ATM.Shared.DTOs.Auth;

namespace ATM.Kiosk.Services.Interfaces
{
    /// <summary>
    /// Contrato para las operaciones de autenticación del ATM.
    /// La implementación concreta usa HttpClient.
    /// Separar la interfaz permite testear la lógica de negocio
    /// sin levantar un servidor real.
    /// </summary>
    public interface IAuthApiClient
    {
        /// <summary>
        /// Paso 1: obtiene datos de la cuenta y el material
        /// necesario para autenticar (PIN_Salt y FingerprintTemplate).
        /// Llama: GET /api/auth/start/{accountId}
        /// </summary>
        AuthStartResponse GetAccountForAuth(int accountId);

        /// <summary>
        /// Paso 2: verifica el PIN hasheado contra el servidor.
        /// C# hashea SHA256(PIN + salt) antes de llamar esto.
        /// Llama: POST /api/auth/verify-pin
        /// Lanza ApiException con IsWrongPIN=true si no coincide.
        /// </summary>
        VerifyPinResponse VerifyPin(VerifyPinRequest request);

        /// <summary>
        /// Paso 3: notifica al servidor que la autenticación biométrica
        /// fue exitosa y solicita la creación de la sesión.
        /// Llama: POST /api/auth/create-session
        /// Devuelve el SessionId y el JWT para requests posteriores.
        /// </summary>
        CreateSessionResponse CreateSession(CreateSessionRequest request);

        /// <summary>
        /// Cierra la sesión activa del cliente.
        /// Llama: POST /api/auth/close-session
        /// </summary>
        void CloseSession(CloseSessionRequest request);
    }
}
