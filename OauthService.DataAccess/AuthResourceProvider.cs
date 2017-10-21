using OauthService.Model;

namespace OauthService.DataAccess
{
    public class AuthResourceProvider : ObjectProvider<AuthResource>
    {
        private readonly ISqlConnectionProvider _connectionProvider;
        private readonly ISqlQueryBuilder<AuthResource> _queryBuilder;

        public AuthResourceProvider(ISqlQueryBuilder<AuthResource> queryBuilder, ISqlConnectionProvider connectionProvider) 
            : base(queryBuilder, connectionProvider)
        {
            _connectionProvider = connectionProvider;
            _queryBuilder = queryBuilder;
        }
    }
}
