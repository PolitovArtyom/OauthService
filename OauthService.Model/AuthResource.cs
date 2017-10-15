namespace OauthService.Model
{
    public class AuthResource : IDbObject
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public AuthResourceType AuthResourceType { get; set; } = AuthResourceType.None;

        public string Settings { get; set; }
    }
}
