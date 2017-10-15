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
    public class ObjectProvider<T> where T : class, IDbObject, new()
    {
        protected readonly string ConnectionString;

        public ObjectProvider(string tableName, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            ConnectionString = connectionString;
        }

        public async Task<T> GetAsync(long id)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                return await connection.GetAsync<T>(id);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                return await connection.GetAllAsync<T>();
            }
        }

        public async Task<long> AddAsync(T obj)
        {
            await ValidateModel(obj);

            using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.InsertAsync(obj);
                return obj.Id;
            }
        }

        public async Task UpdateAsync(T client)
        {
            var toUpdate = await GetAsync(client.Id);
            if (toUpdate == null)
            {
                throw new ObjectNotFoundException($"Client with id={client.Id} not found");
            }

            await ValidateModel(client);
            using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.UpdateAsync(client);
            }
        }

        public async Task DeleteAsync(long id)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.DeleteAsync<T>(new T() { Id = id });
            }
        }

        protected virtual Task ValidateModel(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Task.CompletedTask;
        }
    }
}
