using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapper;
using OauthService.Model;

namespace OauthService.DataAccess
{
    public class ObjectProvider<T> where T : class, IDbObject, new()
    {
        private readonly ISqlQueryBuilder<T> _queryBuilder;
        private readonly ISqlConnectionProvider _connectionProvider;

        public ObjectProvider(ISqlQueryBuilder<T> queryBuilder, ISqlConnectionProvider connectionProvider)
        {
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
        }

        public async Task<T> GetAsync(long id)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<T>(_queryBuilder.Select(id));
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                return await connection.QueryAsync<T>(_queryBuilder.SelectAll());
            }
        }

        public async Task<long> AddAsync(T obj)
        {
            await ValidateModel(obj);

            using (var connection = _connectionProvider.GetConnection())
            {
                return await connection.QuerySingleAsync<long>(_queryBuilder.Insert(obj));
            }
        }

        public async Task UpdateAsync(T obj)
        {
            await ValidateModel(obj);
            using (var connection = _connectionProvider.GetConnection())
            {
                 await connection.ExecuteAsync(_queryBuilder.Update(obj));
            }
        }

        public async Task DeleteAsync(long id)
        {
            using (var connection = _connectionProvider.GetConnection())
            {
                await connection.ExecuteAsync(_queryBuilder.Delete(id));
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
