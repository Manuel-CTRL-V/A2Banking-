using System;
using System.Data.SqlClient;
using BankAPI.DataAccess.Configuration;
using BankAPI.DataAccess.Exceptions;
using Microsoft.Data.SqlClient;

namespace BankAPI.DataAccess.Implementations
{
    public abstract class BaseRepository
    {
        protected readonly string _connectionString;

        protected BaseRepository()
        {
            _connectionString = DatabaseConfig.ConnectionString;
        }

        protected T Execute<T>(Func<SqlConnection, T> action)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(_connectionString);
                conn.Open();
                return action(conn);
            }
            catch (BankDatabaseException)
            {
                throw;
            }
            catch (SqlException sqlEx)
            {
                throw new BankDatabaseException(sqlEx);
            }
            catch (Exception ex)
            {
                throw new BankDatabaseException(
                    "Error inesperado en la capa de datos: " + ex.Message, ex);
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State != System.Data.ConnectionState.Closed)
                        conn.Close();
                    conn.Dispose();
                }
            }
        }

        protected void Execute(Action<SqlConnection> action)
        {
            Execute<object>(conn => { action(conn); return null; });
        }
    }
}
