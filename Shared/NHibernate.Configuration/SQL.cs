using HARS.Shared.DataBases;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using sql = Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Configuration
{
    public class SQL
    {
        public static async Task ExecuteAsync(string query, string connectionString, DatabaseTypes databasetype, bool silentFail = false)
        {
            try
            {
                using var connection = CreateConnection(connectionString, databasetype);
                using var command = CreateCommand(query, connection, databasetype);
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
            catch (SQLiteException sqlitEx)
            {
                if (!silentFail) throw sqlitEx;
                Console.WriteLine(sqlitEx.Message);
            }
            catch (SqlException sqlEx)
            {
                if (!silentFail) throw sqlEx;
                Console.WriteLine(sqlEx.Message);
            }
        }

        public static async Task ExecuteAsync(string query, DbConnection connection, DatabaseTypes dbType, bool silentFail = false)
        {
            using var command = CreateCommand(query, connection, dbType);
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                await command.ExecuteNonQueryAsync();
            }
            catch (SQLiteException sqlitEx)
            {
                if (!silentFail) throw sqlitEx;
                Console.WriteLine(sqlitEx.Message);
            }
            catch (SqlException sqlEx)
            {
                if (!silentFail) throw sqlEx;
                Console.WriteLine(sqlEx.Message);
            }
        }

        public static async Task<object> QueryScalarAsync(string query, DbConnection connection, DatabaseTypes dbType, bool silentFail = false)
        {
            using var command = CreateCommand(query, connection, dbType);
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return await command.ExecuteScalarAsync();
            }
            catch (SQLiteException sqlitEx)
            {
                if (!silentFail) throw sqlitEx;
                Console.WriteLine(sqlitEx.Message);
            }
            catch (SqlException sqlEx)
            {
                if (!silentFail) throw sqlEx;
                Console.WriteLine(sqlEx.Message);
            }
            return null;
        }

        public static DbConnection CreateConnection(string connectionString, DatabaseTypes dbType)
        {
            return dbType switch
            {
                DatabaseTypes.MsSql => new SqlConnection(connectionString),
                DatabaseTypes.Sqlite => new SQLiteConnection(connectionString),
                _ => null,
            };
        }

        private static DbCommand CreateCommand(string query, DbConnection connection, DatabaseTypes dbType)
        {
            return dbType switch
            {
                DatabaseTypes.MsSql => new sql.SqlCommand(query, connection as SqlConnection),
                DatabaseTypes.Sqlite => new SQLiteCommand(query, connection as SQLiteConnection),
                _ => null,
            };
        }
    }
}
