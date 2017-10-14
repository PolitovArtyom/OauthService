using OauthService.Model;
using System.Security.Claims;

namespace OauthService.TokenProviders
{
    public interface ITokenGenerator
    {
        string Generate(Client client, ClaimsIdentity identity);
    }
}
