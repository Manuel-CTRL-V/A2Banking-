namespace ATM.Kiosk.Business.Strategies
{
    /// <summary>
    /// Contrato del patrón Strategy para operaciones bancarias.
    ///
    /// Cada estrategia encapsula una operación completa:
    ///   - Validaciones de negocio previas (monto > 0, etc.)
    ///   - Verificación biométrica si aplica
    ///   - Llamada al ApiClient correspondiente
    ///   - Actualización del estado de sesión local
    ///   - Logging del resultado
    ///
    /// TransactionContext es el único que instancia y ejecuta
    /// strategies — la UI nunca las llama directamente.
    /// </summary>
    public interface ITransactionStrategy
    {
        /// <summary>
        /// Ejecuta la operación bancaria completa.
        /// Lanza BusinessException o ApiException si algo falla.
        /// Devuelve un mensaje de resultado para mostrar en la UI.
        /// </summary>
        TransactionResult Execute(TransactionParameters parameters);
    }

    /// <summary>
    /// Parámetros que la UI pasa al TransactionContext.
    /// Contiene todos los datos posibles — cada estrategia
    /// usa solo los que necesita.
    /// </summary>
    public class TransactionParameters
    {
        public decimal Amount        { get; set; }
        public int     ToAccountId   { get; set; }  // Solo para transferencias
    }

    /// <summary>
    /// Resultado devuelto por cualquier strategy a la UI.
    /// </summary>
    public class TransactionResult
    {
        public bool    Success           { get; set; }
        public string  Message           { get; set; }
        public decimal NewBalance        { get; set; }
        public decimal CommissionApplied { get; set; }
        public bool    CommissionCharged { get; set; }
        public int     TransactionId     { get; set; }
    }
}
