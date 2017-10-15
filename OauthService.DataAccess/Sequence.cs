using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;

namespace OauthService.DataAccess
{
    internal class Sequence
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public Sequence(string connectionString, string tableName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            _connectionString = connectionString;
            _tableName = tableName;
        }

        public async Task<long> GetNextValue(SqliteTransaction transaction)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                var lastInsertedId = await connection.QueryFirstAsync<long>($"SELECT last_insert_rowid() FROM {_tableName}", transaction);
                return lastInsertedId + 1;
            }
        }
    }
}
