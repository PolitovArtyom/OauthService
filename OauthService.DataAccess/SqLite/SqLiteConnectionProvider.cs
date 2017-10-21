using System.Data;
using Microsoft.Data.Sqlite;

namespace OauthService.DataAccess.SqLite
{
    public class SqLiteConnectionProvider : ISqlConnectionProvider
    {
        private readonly string _connectionString;

        public SqLiteConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
