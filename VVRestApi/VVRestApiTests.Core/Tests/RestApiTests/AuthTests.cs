using NUnit.Framework;
using VVRestApi.Vault;

namespace VVRestApiTests.Core.Tests.RestApiTests
{
    [TestFixture]
    public class AuthTests : TestBase
    {

        [Test]
        public void ClientCredentialsGrantType_LoginTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);
        }

        [Test]
        public void ResourceOwnerGrantType_LoginTest()
        {
            VaultApi vaultApi = new VaultApi(this, _ResourceOwnerUserName, _ResourceOwnerPassword);

            Assert.IsNotNull(vaultApi);
        }

        [Test]
        public void RefreshTokenTest()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            Assert.IsTrue(vaultApi.RefreshAccessToken());

            Assert.IsTrue(vaultApi.RefreshAccessToken());
        }

        [Test]
        public void GetVaultUserWebLoginToken()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var user = vaultApi.Users.GetUser(_ResourceOwnerUserName);

            if (user != null)
            {
                string value = user.GetWebLoginToken();

                //if necessary to validate an application user's credentials they can be provided as parameters
                //login token only returned if credentials are valid.
                //example:  string value = user.GetWebLoginToken("someuser","password");

                Assert.IsNotEmpty(value);
            }
        }

        [Test]
        public void GetDefaultCustomerAndDatabaseAliases()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var defaultCustomerInfo = vaultApi.Users.GetUserDefaultCustomerAndDatabaseInfo();
        }

        [Test]
        public void VVRestApiNet2LoginTest()
        {
            //VVRestAPINet2.Vault.VaultApi vaultApi = new VVRestAPINet2.Vault.VaultApi(this);

            //Assert.NotNull(vaultApi);
        }
    }
}
