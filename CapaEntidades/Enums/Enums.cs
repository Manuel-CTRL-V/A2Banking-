namespace ATM.Shared.Enums
{
    public enum AccountType
    {
        Corriente = 1,
        Ahorros = 2,
        Nomina = 3,
        Estudiante = 4
    }
    public enum TransactionStatus
    {
        Completed = 1,
        Failed    = 2,
        Reversed  = 3
    }

    public enum LogLevel
    {
        Info    = 1,
        Warning = 2,
        Error   = 3
    }

    public enum SessionTerminationReason
    {
        UserLogout,
        Timeout,
        MaxAttemptsExceeded,
        ServerForced
    }

    public enum UserRole
    {
        BranchOfficer = 1,   // Abre cuentas, hace enrollment
        Supervisor    = 2    // Suspende cuentas, gestión ampliada
    }
}
