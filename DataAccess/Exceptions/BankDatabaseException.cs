using System;
using Microsoft.Data.SqlClient;

namespace BankAPI.DataAccess.Exceptions
{
    /// <summary>
    /// Excepción tipada para errores originados en stored procedures.
    /// Preserva el número de error del THROW del SP para que la capa
    /// de negocio pueda tomar decisiones sin parsear strings.
    /// </summary>
    public class BankDatabaseException : Exception
    {
        public int ErrorNumber { get; }

        public BankDatabaseException(string message)
            : base(message) { ErrorNumber = 0; }

        public BankDatabaseException(string message, int errorNumber)
            : base(message) { ErrorNumber = errorNumber; }

        public BankDatabaseException(string message, Exception inner)
            : base(message, inner) { ErrorNumber = 0; }

        public BankDatabaseException(SqlException sqlEx)
            : base(sqlEx.Message, sqlEx)
        {
            ErrorNumber = sqlEx.Errors.Count > 0
                ? sqlEx.Errors[0].Number
                : sqlEx.Number;
        }

        // ── Propiedades semánticas que espeja los THROW de los SPs ────

        public bool IsInvalidCredentials => ErrorNumber == 50001;
        public bool IsUserInactive        => ErrorNumber == 50002;
        public bool IsCharacterNotFound   => ErrorNumber == 50010;
        public bool IsAccountNotFound     => ErrorNumber == 50020 || ErrorNumber == 50030
                                         || ErrorNumber == 50061 || ErrorNumber == 50071;
        public bool IsAccountNotPending   => ErrorNumber == 50021;
        public bool IsAccountNotActive    => ErrorNumber == 50032 || ErrorNumber == 50062
                                         || ErrorNumber == 50072 || ErrorNumber == 50084
                                         || ErrorNumber == 50085;
        public bool IsWrongPIN            => ErrorNumber == 50033;
        public bool IsInsufficientFunds   => ErrorNumber == 50073 || ErrorNumber == 50086;
        public bool IsSameAccount         => ErrorNumber == 50081;
        public bool IsSessionNotFound     => ErrorNumber == 50040;
        public bool IsInvalidAmount       => ErrorNumber == 50060 || ErrorNumber == 50070
                                         || ErrorNumber == 50080;
    }
}
