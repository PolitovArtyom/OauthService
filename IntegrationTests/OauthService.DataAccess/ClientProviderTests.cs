﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OauthService.DataAccess;
using OauthService.Model;

namespace UnitTests.OauthService.DataAccess
{
    [TestClass]
    public class ClientProviderTests
    {
        private static readonly string _testConnectionString = @"Data Source=C:\Temp\IdentityServer.sqlite";

        [TestMethod]
        public void Ctor_checks_arguments()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var provider = new ClientProvider(" ");
            });
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var provider = new ClientProvider("");
            });
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var provider = new ClientProvider(null);
            });
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
            var client = provider.GetClientAsync(clientId).Result;

            Assert.IsNotNull(client);
        }

        [TestMethod]
        public void Cant_add_with_dublicated_identifier()
        {
            var provider = new ClientProvider(_testConnectionString);
            var clients = provider.GetClientsAsync().Result.ToList();

            var newClient = new Client
            {
                Name = "dublicate test",
                Identifier = clients.First().Identifier
            };

            Assert.ThrowsException<AggregateException>(() =>
            {
                var t = provider.AddClientAsync(newClient).Result;
            });
        }

        [TestMethod]
        public void Can_add_new_client()
        {
            var provider = new ClientProvider(_testConnectionString);

            var newClient = new Client
            {
                Name = "create new client test",
                Identifier = Guid.NewGuid().ToString(),
                Secret = "Secret"
            };

            var newClientId = provider.AddClientAsync(newClient).Result;

            var result = provider.GetClientAsync(newClientId).Result;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_update_client()
        {
            var provider = new ClientProvider(_testConnectionString);

            var newClient = new Client
            {
                Name = "update client test",
                Identifier = Guid.NewGuid().ToString(),
                Secret = "Secret"
            };

            var newClientId = provider.AddClientAsync(newClient).Result;
            var client = provider.GetClientAsync(newClientId).Result;

            var clientNewIdentifier = Guid.NewGuid().ToString();
            client.Identifier = clientNewIdentifier;
            provider.UpdateClientAsync(client).Wait();

            var result = provider.GetClientAsync(client.Id).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(client.Identifier, clientNewIdentifier);
        }

        [TestMethod]
        public void Can_delete_client()
        {
            var provider = new ClientProvider(_testConnectionString);

            var newClient = new Client
            {
                Name = "delete client test",
                Identifier = Guid.NewGuid().ToString(),
                Secret = "Secret"
            };

            var newClientId = provider.AddClientAsync(newClient).Result;

            var client = provider.GetClientAsync(newClientId).Result;
            Assert.IsNotNull(client);

            provider.DeleteClientAsync(client.Id).Wait();
            var result = provider.GetClientAsync(newClientId).Result;

            Assert.IsNull(result);
        }
    }
}