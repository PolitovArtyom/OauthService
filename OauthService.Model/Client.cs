using System;
using System.Collections.Generic;
using System.Text;

namespace OauthService.Model
{
    public class Client
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Identifier { get; set; }

        public string Secret { get; set; }

        public string Callback { get; set; }

        public string Description { get; set; }

        public string LogoutPage { get; set; }
    }
}
