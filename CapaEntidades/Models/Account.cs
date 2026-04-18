using System;
using ATM.Shared.Enums;

namespace ATM.Shared.Models
{
    /// <summary>
    /// Cuenta bancaria — versión del modelo que conoce el cliente.
    ///
    /// Deliberadamente NO incluye PIN_Hash, PIN_Salt ni
    /// FingerprintTemplate. Esos datos sensibles nunca salen
    /// del servidor salvo en los endpoints específicos de auth,
    /// y el ATM los usa solo en memoria durante el login,
    /// nunca los persiste ni los pone en este modelo.
    ///
    /// El ATM construye este objeto a partir de AuthStartResponse
    /// y lo mantiene en memoria durante la sesión activa para
    /// no tener que consultarlo en cada operación.
    /// </summary>
    public class Account
    {
        public int AccountId { get; set; }
        public int CharacterId { get; set; }
        public decimal Balance { get; set; }
        public AccountStatus Status { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime CreatedAt { get; set; }
        public string HolderName { get; set; }

        public bool IsActive => Status == AccountStatus.Active;
    }
}
