using System.Collections.Generic;

namespace OauthService.Model
{
    public class Role : IDbObject
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public Client Client { get; set; }

        public IEnumerable<ResourceRole> ResourceRoles { get; set; }
    }
}
