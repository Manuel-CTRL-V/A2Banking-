using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace BankCLR
{
    /// <summary>
    /// Funciones CLR registradas en SQL Server.
    /// Este assembly se compila como .NET Framework Class Library
    /// y se registra en BankDB con:
    ///   CREATE ASSEMBLY BankCLR FROM '<ruta>'
    ///   WITH PERMISSION_SET = SAFE;
    ///
    /// PERMISSION_SET = SAFE significa que la función:
    ///   - No puede acceder a recursos externos (red, disco, BD)
    ///   - Solo hace cálculos en memoria
    ///   - Es la categoría más segura y la correcta para este caso
    /// </summary>
    public static class CommissionFunctions
    {
        /// <summary>
        /// Calcula la comisión de una transferencia.
        ///
        /// Regla de negocio:
        ///   - Mismo titular en ambas cuentas → comisión = 0.00
        ///   - Titulares distintos             → comisión = monto × 0.0015 (0.15%)
        ///
        /// La comparación es case-insensitive para evitar que
        /// "Homer Simpson" y "homer simpson" se traten como distintos.
        ///
        /// Llamada desde sp_Transfer:
        ///   SET @Commission = dbo.fn_CalculateCommission(
        ///       @FromHolder, @ToHolder, @Amount)
        /// </summary>
        [SqlFunction(
            DataAccess      = DataAccessKind.None,
            SystemDataAccess = SystemDataAccessKind.None,
            IsDeterministic  = true,
            IsPrecise        = true)]
        public static SqlDecimal CalculateCommission(
            SqlString  holderA,
            SqlString  holderB,
            SqlDecimal amount)
        {
            // Si alguno es NULL devolver 0
            if (holderA.IsNull || holderB.IsNull || amount.IsNull)
                return new SqlDecimal(0);

            // Comparación sin distinción de mayúsculas
            bool sameHolder = string.Compare(
                holderA.Value,
                holderB.Value,
                ignoreCase: true) == 0;

            if (sameHolder)
                return new SqlDecimal(0);

            // 0.15% sobre el monto
            decimal commission = amount.Value * 0.0015m;

            // Redondear a 2 decimales
            commission = decimal.Round(commission, 2);

            return new SqlDecimal(commission);
        }
    }
}
