using System.Threading.Tasks;

using Dapper;
using Microsoft.Data.Sqlite;
using OauthService.Model;

namespace OauthService.DataAccess
{
    public class ClientProvider : ObjectProvider<Client>
    {
        private const string TableName = "Clients";

        public ClientProvider(string connectionString) : base(TableName, connectionString)
        {
        }

        public async Task<Client> GetAsync(string identifier)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Client>(
                    $"SELECT * FROM {TableName} WHERE identifier = @identifier",
                    new {identifier});
            }
        }

    }
}