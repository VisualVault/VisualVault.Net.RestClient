// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginTests.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// <summary>
//   The login tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using VVRestApi.Vault.Forms;
using VVRestApi.Vault.PersistedData;
using VVRestApi.Vault.Users;

namespace VVRestApiTests
{
    using NUnit.Framework;
    using VVRestApi.Common;
    using VVRestApi.Vault;

    /// <summary>
    ///     The login tests.
    /// </summary>
    [TestFixture]
    public class RestApiTests
    {
        #region Constants

        //Base URL to VisualVault.  Copy URL string preceding the version number ("/v1")
        const string VaultApiBaseUrl = "https://demo.visualvault.com";

        //API version number (number following /v in the URL).  Used to provide backward compatitiblity.
        const string ApiVersion = "1";

        //OAuth2 token endpoint, exchange credentials for api access token
        //typically the VaultApiBaseUrl + /oauth/token unless using an external OAuth server
        private const string OAuthServerTokenEndPoint = "https://demo.visualvault.com/oauth/token";

        //your customer alias value.  Visisble in the URL when you log into VisualVault
        const string CustomerAlias = "CustomerName";

        //your customer database alias value.  Visisble in the URL when you log into VisualVault
        const string DatabaseAlias = "DatabaseName";

        //Copy "API Key" value from User Account Property Screen
        const string ClientId = "f567e2e5-1d07-4a1d-b828-d61e21382f5a";

        //Copy "API Secret" value from User Account Property Screen
        const string ClientSecret = "2/BROCVpbwi+ACNlg55CV9gzFiFqyF4/bWF1+Oi4rKU=";

        // Scope is used to determine what resource types will be available after authentication.  If unsure of the scope to provide use
        // either 'vault' or no value.  'vault' scope is used to request access to a specific customer vault (aka customer database). 
        const string Scope = "vault";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string ResourceOwnerUserName = "DeveloperUserId";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string ResourceOwnerPassword = "DeveloperPassword";

        #endregion

        #region Tests

        [Test]
        public void ClientCredentialsGrantType_LoginTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);
        }

        [Test]
        public void ResourceOwnerGrantType_LoginTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets, RestApiTests.ResourceOwnerUserName, RestApiTests.ResourceOwnerPassword);

            Assert.IsNotNull(vaultApi);
        }

        [Test]
        public void RefreshTokenTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            Assert.IsTrue(vaultApi.RefreshAccessToken());
        }

        [Test]
        public void GetUsersTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            Page<User> users = vaultApi.Users.GetUsers();

            Assert.IsNotNull(users);
        }

        [Test]
        public void GetVaultUserWebLoginToken()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var user = vaultApi.Users.GetUser(ResourceOwnerUserName);

            if (user != null)
            {
                string value = user.GetWebLoginToken();

                Assert.IsNotNullOrEmpty(value);
            }
        }

        [Test]
        public void FormTemplatesTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            Page<FormTemplate> formTemplates = vaultApi.FormTemplates.GetFormTemplates();

            foreach (FormTemplate formTemplate in formTemplates.Items)
            {
                Assert.IsNotNullOrEmpty(formTemplate.Name);
            }

            Assert.IsNotNull(formTemplates);
        }

        [Test]
        public void PersistedDataTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            const string dataToStore = "{\"patientId\":\"12345\",\"claimNumber\":\"12345\"}";

            PersistedClientData data = vaultApi.PersistedData.CreateData(ShortGuid.NewGuid().ToString(), ScopeType.Global, dataToStore, "text/JSON", "", LinkedObjectType.None, null);

            Assert.IsNotNull(data);

            //use data id to retrieve the data
            string dataId = data.Id.ToString();

            string loginToken = vaultApi.CurrentUser.GetCurrentUser().GetWebLoginToken();

            string url = string.Format("{0}/v1/en/ACMEINC/Main/vvlogin?authtoken={1}&returnurl=userportal?portalname={2}%26persistedId={3}", VaultApiBaseUrl, loginToken, "claims", dataId);

            System.Diagnostics.Debug.WriteLine(url);
        }

        [Test]
        public void RequestOptionTest()
        {
            VVRestApi.Common.RequestOptions options = new RequestOptions();
            options.Query = "name eq 'world' AND id eq 'whatever'";

            var request = options.GetQueryString("q=userid eq 'vault.config'&stuff=things");
            Assert.IsNotNullOrEmpty(request);
        }

        [Test]
        public void CreateCustomerTest()
        {
            ClientSecrets clientSecrets = new ClientSecrets
            {
                ApiKey = RestApiTests.ClientId,
                ApiSecret = RestApiTests.ClientSecret,
                OAuthTokenEndPoint = RestApiTests.OAuthServerTokenEndPoint,
                BaseUrl = RestApiTests.VaultApiBaseUrl,
                ApiVersion = RestApiTests.ApiVersion,
                CustomerAlias = RestApiTests.CustomerAlias,
                DatabaseAlias = RestApiTests.DatabaseAlias,
                Scope = RestApiTests.Scope
            };

            VaultApi vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var newCustomer = vaultApi.Customer.CreateCustomer("Customer2", "Customer2", "Main", "Customer2.Admin", "p",
                                             "tod.olsen@visualvault.com", 1, 5, true);

            Assert.IsNotNull(newCustomer);
        }

        #endregion
    }
}