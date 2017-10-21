using OauthService.Model;

namespace OauthService.DataAccess
{
    public interface ISqlQueryBuilder<T> where T : IDbObject
    {
        string Select(long id);

        string SelectAll();

        string Insert(T obj);

        string Update(T obj);

        string Delete(long id);
    }
}
