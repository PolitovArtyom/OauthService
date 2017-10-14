using Microsoft.VisualStudio.TestTools.UnitTesting;
using OauthService.Model;
using OauthService.TokenProviders.Jwt;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace UnitTests.OauthService.Token.Jwt
{
    [TestClass]
    public class JwtGeneratorTests
    {
        private JwtGenerator GetGenerator()
        {
            return new JwtGenerator("testIssuer", new TimeSpan(1000));
        }

        [TestMethod]
        public void Generate()
        {
            var client = new Client()
            {
                Identifier = "test",
                Secret = "test"
            };

            var identity = new ClaimsIdentity();

            GetGenerator().Generate(client, identity);
        }
    }
}
