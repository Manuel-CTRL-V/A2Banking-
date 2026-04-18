namespace ATM.Shared.Models
{
    /// <summary>
    /// Detalle de una transferencia bancaria.
    /// Especialización de Transaction — relación 1:0..1.
    ///
    /// Existe como modelo propio (no solo anidado en Transaction)
    /// porque el BackOffice y el BankAPI necesitan consultarlo
    /// de forma independiente, por ejemplo para reportes de
    /// comisiones o para auditar transferencias específicas.
    ///
    /// CommissionAmount es calculado por fn_CalculateCommission (SQL CLR):
    ///   DifferentHolder = true  → Amount × 0.0015
    ///   DifferentHolder = false → 0.00
    /// </summary>
    public class Transfer
    {
        public int     TransferId       { get; set; }
        public int     TransactionId    { get; set; }
        public int     FromAccountId    { get; set; }
        public int     ToAccountId      { get; set; }
        public decimal CommissionAmount { get; set; }
        public bool    DifferentHolder  { get; set; }

        // Navegación — poblados cuando se hace JOIN
        public string  FromHolderName   { get; set; }
        public string  ToHolderName     { get; set; }
    }
}
