using System;
using ATM.Shared.Enums;

namespace ATM.Shared.Models
{
    /// <summary>
    /// Registro de una operación bancaria.
    /// La capa de negocio trabaja con este modelo internamente.
    /// Se construye a partir de TransactionHistoryItem (DTO).
    ///
    /// BalanceBefore y BalanceAfter permiten mostrar el estado
    /// de la cuenta antes y después de cada operación en el
    /// historial, sin necesidad de recalcular.
    /// </summary>
    public class Transaction
    {
        public int               TransactionId  { get; set; }
        public int               AccountId      { get; set; }
        public string            HolderName     { get; set; }
        public TransactionType   Type           { get; set; }
        public decimal           Amount         { get; set; }
        public decimal           BalanceBefore  { get; set; }
        public decimal           BalanceAfter   { get; set; }
        public TransactionStatus Status         { get; set; }
        public string            ATM_Identifier { get; set; }
        public DateTime          Timestamp      { get; set; }

        // Presente solo si Type == Transfer
        // Usa el modelo Transfer independiente, no uno anidado
        public Transfer          Transfer       { get; set; }

        public bool IsTransfer => Type == TransactionType.Transfer
                               && Transfer != null;
    }
}
