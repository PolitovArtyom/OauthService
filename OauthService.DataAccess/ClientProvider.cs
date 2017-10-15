using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.Sqlite;
using OauthService.DataAccess.Exceptions;
using OauthService.Model;

namespace OauthService.DataAccess
{
    public class ClientProvider
    {
        private const string TableName = "Clients";

        private readonly string _connectionString;
        private readonly Sequence _sequence;

        public ClientProvider(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            _connectionString = connectionString;
            _sequence = new Sequence(_connectionString, TableName);
        }

        public async Task<Client> GetClientAsync(long id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return await connection.GetAsync<Client>(id);
            }
        }

        public async Task<Client> GetClientAsync(string identifier)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                return await connection.QuerySingleOrDefaultAsync<Client>(
                    "SELECT * FROM Clients WHERE identifier = @identifier",
                    new {identifier});
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
            await CheckModel(client);

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead))
                {
                    try
                    {
                        client.Id = await _sequence.GetNextValue(transaction);
                        await connection.InsertAsync(client, transaction);
                        transaction.Commit();
                        return client.Id;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task UpdateClientAsync(Client client)
        {
            var toUpdate = await GetClientAsync(client.Id);
            if (toUpdate == null)
            {
                throw new ObjectNotFoundException($"Client with id={client.Id} not found");
            }

            await CheckModel(client);
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.UpdateAsync(client);
            }
        }

        public async Task DeleteClientAsync(long id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.DeleteAsync<Client>(new Client(){Id = id});
            }
        }

        private async Task CheckModel(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.Identifier))
            {
                throw new ArgumentNullException(nameof(client.Identifier));
            }

            if (await GetClientAsync(client.Identifier) != null)
            {
                throw new DuplicateNameException($"Client with identifier={client.Identifier} already exists");
            }
        }
    }
}