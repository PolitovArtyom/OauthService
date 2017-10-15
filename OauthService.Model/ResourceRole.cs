namespace OauthService.Model
{
    public class ResourceRole : IDbObject
    {
        public long Id { get; set; }

        public AuthResource AuthResource { get; set; }

        public string SourceId { get; set; }

        public string SourceName { get; set; }
    }
}
