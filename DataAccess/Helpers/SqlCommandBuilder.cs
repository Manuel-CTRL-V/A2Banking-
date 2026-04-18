using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace BankAPI.DataAccess.Helpers
{
    public sealed class SqlCommandBuilder
    {
        private readonly SqlCommand _cmd;

        private SqlCommandBuilder(string sp, SqlConnection conn)
        {
            _cmd             = new SqlCommand(sp, conn);
            _cmd.CommandType = CommandType.StoredProcedure;
        }

        public static SqlCommandBuilder For(string sp, SqlConnection conn)
            => new SqlCommandBuilder(sp, conn);

        public SqlCommandBuilder With(string name, int value)
        {
            _cmd.Parameters.Add(new SqlParameter(name, SqlDbType.Int) { Value = value });
            return this;
        }
        //public SqlCommandBuilder With(string name, bool value)
        //{
        //    _cmd.Parameters.Add(new SqlParameter(name, SqlDbType.Bit) { Value = value });
        //    return this;
        //}

        public SqlCommandBuilder With(string name, decimal value)
        {
            _cmd.Parameters.Add(new SqlParameter(name, SqlDbType.Decimal)
                { Value = value, Precision = 18, Scale = 2 });
            return this;
        }

        public SqlCommandBuilder With(string name, string value)
        {
            _cmd.Parameters.Add(new SqlParameter(name, SqlDbType.NVarChar)
                { Value = value ?? (object)DBNull.Value });
            return this;
        }
        public SqlCommandBuilder With(string name, bool value)
        {
            _cmd.Parameters.Add(new SqlParameter(name, SqlDbType.Bit)
            { Value = value });
            return this;
        }

        public SqlCommandBuilder With(string name, byte value)
        {
            _cmd.Parameters.Add(new SqlParameter(name, SqlDbType.TinyInt) { Value = value });
            return this;
        }

        public SqlCommandBuilder With(string name, byte[] value)
        {
            _cmd.Parameters.Add(new SqlParameter(name, SqlDbType.VarBinary)
                { Value = value ?? (object)DBNull.Value });
            return this;
        }

        public SqlCommandBuilder WithMax(string name, byte[] value)
        {
            _cmd.Parameters.Add(new SqlParameter(name, SqlDbType.VarBinary, -1)
                { Value = value != null ? (object)value : DBNull.Value });
            return this;
        }

        public SqlCommandBuilder WithNullable(string name, int? value)
        {
            _cmd.Parameters.Add(new SqlParameter(name, SqlDbType.Int)
                { Value = value.HasValue ? (object)value.Value : DBNull.Value });
            return this;
        }

        public SqlCommandBuilder WithNullable(string name, string value)
        {
            _cmd.Parameters.Add(new SqlParameter(name, SqlDbType.NVarChar)
                { Value = value != null ? (object)value : DBNull.Value });
            return this;
        }

        public SqlCommandBuilder WithNullable(string name, DateTime? value)
        {
            _cmd.Parameters.Add(new SqlParameter(name, SqlDbType.DateTime)
                { Value = value.HasValue ? (object)value.Value : DBNull.Value });
            return this;
        }

        public SqlCommand Build() => _cmd;
    }
}
