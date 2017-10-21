using System.Linq;
using System.Threading.Tasks;

using Dapper;
using Microsoft.Data.Sqlite;
using OauthService.Model;

namespace OauthService.DataAccess
{
    public class ClientProvider : ObjectProvider<Client>
    {
        private readonly ISqlConnectionProvider _connectionProvider;
        private readonly ISqlQueryBuilder<Client> _queryBuilder;

        public ClientProvider(ISqlQueryBuilder<Client> queryBuilder, ISqlConnectionProvider connectionProvider) 
        : base(queryBuilder, connectionProvider)
        {
            _connectionProvider = connectionProvider;
            _queryBuilder = queryBuilder;
        }

        //public async Task<Client> GetAsync(string identifier)
        //{
        //    using (var connection = new SqliteConnection(ConnectionString))
        //    {
        //        var result = await connection.QueryAsync<Client, AuthResource, Client>(
        //            $"SELECT * FROM {TableName} c INNER JOIN AuthResources ar ON c.AuthResource = ar.Id" +
        //            $" WHERE c.identifier = @identifier",
        //            (client, authResource) => { client.AuthResource = authResource;
        //                return client;
        //            },
        //            new {identifier});
        //        return result.SingleOrDefault();
        //    }
        //}

    }
}