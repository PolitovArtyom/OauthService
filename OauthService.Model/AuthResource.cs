namespace OauthService.Model
{
    public class AuthResource : IDbObject
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public AuthProviderType ProviderType { get; set; } = AuthProviderType.None;

        public string Settings { get; set; }
    }
}
