using System.Data;
using System.Data.SqlClient;

namespace OauthService.DataAccess
{
    public interface ISqlConnectionProvider
    {
        IDbConnection GetConnection();
    }
}
