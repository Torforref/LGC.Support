using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace LGC.Support.Models
{
    public interface IDatabaseConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync();
    }
    public class SqlConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString) => _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            return sqlConnection;
        }
    }
}