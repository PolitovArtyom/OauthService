using Microsoft.VisualStudio.TestTools.UnitTesting;
using OauthService.DataAccess;
using OauthService.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace UnitTests.OauthService.DataAccess
{
    [TestClass]
    public class ClientProviderTests
    {
        private static string _testConnectionString = @"Data Source=C:\Temp\IdentityServer.sqlite";

        [TestMethod]
        public void Ctor_checks_arguments()
        {
            Assert.ThrowsException<ArgumentNullException>(() => { var provider = new ClientProvider(" "); });
            Assert.ThrowsException<ArgumentNullException>(() => { var provider = new ClientProvider(""); });
            Assert.ThrowsException<ArgumentNullException>(() => { var provider = new ClientProvider(null); });
        }

        [TestMethod]
        public void Can_get_all_clients()
        {
            var provider = new ClientProvider(_testConnectionString);
            var clients = provider.GetClientsAsync().Result.ToList();

            Assert.IsNotNull(clients);
            Assert.IsTrue(clients.Any());
            CollectionAssert.AllItemsAreNotNull(clients);
        }

        [TestMethod]
        public void Can_get_client_byId()
        {
            var provider = new ClientProvider(_testConnectionString);
            var clients = provider.GetClientsAsync().Result.ToList();

            var clientId = clients.FirstOrDefault().Id;
            var client = provider.GetClientAsync(clientId);

            Assert.IsNotNull(client);
        }

        [TestMethod]
        public void Cant_add_with_dublicated_identifier()
        {
            var provider = new ClientProvider(_testConnectionString);
            var clients = provider.GetClientsAsync().Result.ToList();

            var newClient = new Client()
            {
                Name = "dublicate test",
                Identifier = clients.First().Identifier
            };

            Assert.ThrowsException<AggregateException>(() => { var t = provider.AddClientAsync(newClient).Result; });
        }

        [TestMethod]
        public void Can_add_new_client()
        {
            var provider = new ClientProvider(_testConnectionString);
            
            var newClient = new Client()
            {
                Name = "create new client test",
                Identifier = Guid.NewGuid().ToString("R")
            };

            var newClientId = provider.AddClientAsync(newClient).Result;

            var result = provider.GetClientAsync(newClientId);
            Assert.IsNotNull(result);
        }
    }
}
