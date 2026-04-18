using System;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace BankAPI.DataAccess.Helpers
{
    public static class DataReaderExtensions
    {
        public static int GetInt(this SqlDataReader r, string col)
            => r.GetInt32(r.GetOrdinal(col));

        public static int? GetNullableInt(this SqlDataReader r, string col)
        {
            int ord = r.GetOrdinal(col);
            return r.IsDBNull(ord) ? (int?)null : r.GetInt32(ord);
        }

        public static string GetString(this SqlDataReader r, string col)
        {
            int ord = r.GetOrdinal(col);
            return r.IsDBNull(ord) ? null : r.GetString(ord);
        }

        public static decimal GetDecimal(this SqlDataReader r, string col)
            => r.GetDecimal(r.GetOrdinal(col));

        public static decimal? GetNullableDecimal(this SqlDataReader r, string col)
        {
            int ord = r.GetOrdinal(col);
            return r.IsDBNull(ord) ? (decimal?)null : r.GetDecimal(ord);
        }

        public static bool GetBool(this SqlDataReader r, string col)
            => r.GetBoolean(r.GetOrdinal(col));

        public static bool? GetNullableBool(this SqlDataReader r, string col)
        {
            int ord = r.GetOrdinal(col);
            return r.IsDBNull(ord) ? (bool?)null : r.GetBoolean(ord);
        }

        public static DateTime GetDateTime(this SqlDataReader r, string col)
            => r.GetDateTime(r.GetOrdinal(col));

        public static DateTime? GetNullableDateTime(this SqlDataReader r, string col)
        {
            int ord = r.GetOrdinal(col);
            return r.IsDBNull(ord) ? (DateTime?)null : r.GetDateTime(ord);
        }

        public static byte GetByte(this SqlDataReader r, string col)
            => r.GetByte(r.GetOrdinal(col));

        public static byte[] GetBytes(this SqlDataReader r, string col)
        {
            int ord = r.GetOrdinal(col);
            return r.IsDBNull(ord) ? null : (byte[])r.GetValue(ord);
        }
    }
}
