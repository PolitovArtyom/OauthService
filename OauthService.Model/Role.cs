using System;
using System.Collections.Generic;
using System.Text;

namespace OauthService.Model
{
    public class Role : IDbObject
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }

        public AuthResource AuthResource { get; set; }

        public IEnumerable<ResourceRole> ResourceRoles { get; set; }
    }
}
