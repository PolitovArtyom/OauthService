using Dapper;
using Microsoft.Data.Sqlite;
using OauthService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace OauthService.DataAccess
{
    public class ClientProvider
    {
        private string _connectionString;

        public ClientProvider(string connectionString)
        {
            if(string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            _connectionString = connectionString;
        }

        public async Task<Client> GetClientAsync(long id)
        {
            using(var connection = new SqliteConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Client>("SELECT * FROM Clients WHERE Id = @id", new { id = id });
            }
        }

        public async Task<Client> GetClientAsync(string identifier)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Client>("SELECT * FROM Clients WHERE identifier = @identifier", new { identifier = identifier });
            }
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return await connection.QueryAsync<Client>("SELECT * FROM Clients");
            }
        }

        public async Task<long> AddClientAsync(Client client)
        {
            if (await GetClientAsync(client.Identifier) != null)
            {
                throw new DuplicateNameException($"Client with identifier={client.Identifier} already exists");
            }
   
            using (var connection = new SqliteConnection(_connectionString))
            {
                return await connection.QueryFirstAsync<long>(
                "INSERT INTO Clients VALUES (null, @Id, @Name, @Identifier, @Secret, @Callback, @Desciption, @LogoutPage)",
                 client);
            }
        }


    }
}
