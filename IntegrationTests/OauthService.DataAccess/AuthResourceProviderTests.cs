using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OauthService.DataAccess;
using OauthService.Model;

namespace IntegrationTests.OauthService.DataAccess
{
    [TestClass]
    public class AuthResourceProviderTests
    {
        private static readonly string _testConnectionString = @"Data Source=C:\Temp\IdentityServer.sqlite";

        [TestMethod]
        public void Ctor_checks_arguments()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var provider = new AuthResourceProvider(" ");
            });
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var provider = new AuthResourceProvider("");
            });
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var provider = new AuthResourceProvider(null);
            });
        }

        [TestMethod]
        public void Can_get_all_authResource()
        {
            var provider = new AuthResourceProvider(_testConnectionString);
            var authResources = provider.GetAllAsync().Result.ToList();

            Assert.IsNotNull(authResources);
            Assert.IsTrue(authResources.Any());
            CollectionAssert.AllItemsAreNotNull(authResources);
        }

        [TestMethod]
        public void Can_get_authResource_byId()
        {
            var provider = new AuthResourceProvider(_testConnectionString);
            var authResources = provider.GetAllAsync().Result.ToList();

            var id = authResources.FirstOrDefault().Id;
            var authResourc = provider.GetAsync(id).Result;

            Assert.IsNotNull(authResourc);
        }


        [TestMethod]
        public void Can_add_new_authResource()
        {
            var provider = new AuthResourceProvider(_testConnectionString);

            var authResource = new AuthResource
            {
                Name = "create new client test",
                AuthResourceType = AuthResourceType.None
            };

            var id = provider.AddAsync(authResource).Result;

            var result = provider.GetAsync(id).Result;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Can_update_authResource()
        {
            var provider = new AuthResourceProvider(_testConnectionString);

            var authResource = new AuthResource
            {
                Name = "update client test",
                AuthResourceType = AuthResourceType.None
            };

            var id = provider.AddAsync(authResource).Result;
            authResource = provider.GetAsync(id).Result;

            var newName = Guid.NewGuid().ToString();
            authResource.Name = newName;
            provider.UpdateAsync(authResource).Wait();

            var result = provider.GetAsync(authResource.Id).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(authResource.Name, newName);
        }

        [TestMethod]
        public void Can_delete_authResource()
        {
            var provider = new AuthResourceProvider(_testConnectionString);

            var authResource = new AuthResource
            {
                Name = "update client test",
                AuthResourceType = AuthResourceType.None
            };

            var id = provider.AddAsync(authResource).Result;
            authResource = provider.GetAsync(id).Result;

            Assert.IsNotNull(authResource);

            provider.DeleteAsync(authResource.Id).Wait();
            var result = provider.GetAsync(id).Result;

            Assert.IsNull(result);
        }
    }
}