using System;

namespace ATM.Shared.Models
{
    /// <summary>
    /// Personaje de la Simpsons API — es el titular de las cuentas.
    /// En el ATM este modelo representa al cliente que está operando
    /// el kiosko. Se construye a partir de AuthStartResponse.
    /// </summary>
    public class Character
    {
        public int      CharacterId    { get; set; }
        public int      ApiCharacterId { get; set; }
        public string   FullName       { get; set; }
        public DateTime LastSyncedAt   { get; set; }
    }
}
