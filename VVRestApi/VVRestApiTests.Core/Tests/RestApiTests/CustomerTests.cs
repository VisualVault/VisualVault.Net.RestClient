using NUnit.Framework;
using System;
using System.Net;
using VVRestApi.Vault;
using VVRestApi.Vault.Configuration;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class CustomerTests : TestBase
    {

        [Test]
        public void GetCustomerDatabaseInfo()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dbInfo = vaultApi.Customer.GetCustomerDatabaseInfo();

            Assert.IsNotNull(dbInfo);

        }

        [Test]
        public void GetCustomerDatabaseConfigInfo()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var databaseConfiguration = vaultApi.ConfigurationManager.GetDatabaseConfiguration();

            try
            {
                if (databaseConfiguration != null)
                {
                    foreach (ContentProvider provider in databaseConfiguration.ContentProviders)
                    {
                        switch (provider.ContentProviderType)
                        {
                            case ContentProviderType.AwsS3Provider:
                                AwsS3Provider s3Provider = new AwsS3Provider(provider);
                                break;
                            case ContentProviderType.FileSystemProvider:
                                FileSystemProvider fileSystemProvider = new FileSystemProvider(provider);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(databaseConfiguration);
            }

            Assert.IsNotNull(databaseConfiguration);

        }

        [Test]
        public void CustomerAssignUser()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var customerId = new Guid("");

            var authUserId = "";

            var result = vaultApi.Customer.AssignUser(customerId, authUserId);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void CustomerDatabaseAssignUser()
        {
            var vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var customerDatabaseId = new Guid("");

            var authUserId = "";

            var result = vaultApi.CustomerDatabase.AssignUser(customerDatabaseId, authUserId);
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }
    }
}
