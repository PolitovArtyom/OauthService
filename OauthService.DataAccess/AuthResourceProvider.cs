using OauthService.Model;

namespace OauthService.DataAccess
{
    public class AuthResourceProvider : ObjectProvider<AuthResource>
    {
        private const string TableName = "AuthResources";

        public AuthResourceProvider(string connectionString) : base(TableName, connectionString)
        {
        }
    }
}
