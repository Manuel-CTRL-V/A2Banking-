using ATM.Shared.Enums;

namespace ATM.Shared.DTOs.Auth
{
    // ── Paso 1: el ATM pide datos de la cuenta para iniciar sesión ──

    public class AuthStartRequest
    {
        public int AccountId { get; set; }
    }

    public class AuthStartResponse
    {
        public int AccountId { get; set; }
        public string HolderName { get; set; }
        public AccountStatus Status { get; set; }
        public string AccountType { get; set; } 

        // El servidor devuelve el salt para que el ATM pueda
        // hashear el PIN localmente antes de enviarlo.
        // NUNCA devuelve el hash almacenado — solo el salt.
        public byte[] PIN_Salt { get; set; }

        // Template del DigitalPersona serializado.
        // El ATM lo deserializa en memoria y el SDK lo usa
        // para verificar la huella localmente.
        // Nunca se persiste en el disco del ATM.
        public byte[] FingerprintTemplate { get; set; }
    }

    // ── Paso 2: el ATM envía el PIN hasheado (nunca el PIN plano) ──

    public class VerifyPinRequest
    {
        public int    AccountId { get; set; }
        public byte[] PIN_Hash  { get; set; }
    }

    public class VerifyPinResponse
    {
        public bool   Verified { get; set; }
        public string Message  { get; set; }
    }

    // ── Paso 3: autenticación completa, servidor crea la sesión ──

    public class CreateSessionRequest
    {
        public int    AccountId      { get; set; }
        public string ATM_Identifier { get; set; }
    }

    public class CreateSessionResponse
    {
        public int    SessionId { get; set; }
        public string Token     { get; set; }   // JWT para requests posteriores
    }

    // ── Cierre de sesión ──

    public class CloseSessionRequest
    {
        public int    SessionId         { get; set; }
        public string TerminationReason { get; set; }
    }
}
