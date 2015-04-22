// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginTests.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// <summary>
//   The login tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Common.Logging;
using VVRestApi.Vault.Forms;
using VVRestApi.Vault.Meta;
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
        const string VaultApiBaseUrl = "http://dev68/VisualVault4_1_9";

        //API version number (number following /v in the URL).  Used to provide backward compatitiblity.
        const string ApiVersion = "1";

        //OAuth2 token endpoint, exchange credentials for api access token
        //typically the VaultApiBaseUrl + /oauth/token unless using an external OAuth server
        private const string OAuthServerTokenEndPoint = "http://dev68/VisualVault4_1_9/oauth/token";

        //your customer alias value.  Visisble in the URL when you log into VisualVault
        const string CustomerAlias = "Customer412";

        //your customer database alias value.  Visisble in the URL when you log into VisualVault
        const string DatabaseAlias = "Main";

        //Copy "API Key" value from User Account Property Screen
        const string ClientId = "6abdcfa4-c45a-48d9-b21f-b8893c189cf9";

        //Copy "API Secret" value from User Account Property Screen
        const string ClientSecret = "SqjSsY2I4bDU0E5tj9HgHYxLV0ClJJCFf75ckg3fRKU=";

        // Scope is used to determine what resource types will be available after authentication.  If unsure of the scope to provide use
        // either 'vault' or no value.  'vault' scope is used to request access to a specific customer vault (aka customer database). 
        const string Scope = "vault";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string ResourceOwnerUserName = "vault.config";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string ResourceOwnerPassword = "p";

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
                string value = user.GetWebLoginToken("vault.config","p");

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

            const string dataToStore = "{\"invoice no\":\"272255\",\"formname\":\"orderform\",\"Address\":\"ds\"}";

            string persistedDataUniqueName = ShortGuid.NewGuid();

            PersistedClientData data = vaultApi.PersistedData.CreateData(persistedDataUniqueName, ScopeType.Global, dataToStore, "text/JSON", "", LinkedObjectType.None, null);

            Assert.IsNotNull(data);

            //get the persisted data by Id
            data = vaultApi.PersistedData.GetData(data.Id);

            Assert.IsNotNull(data);

            //Note:  The list of field names below was generated by running the GetApiObjectFieldNames test 
            //which calls the MetaData APi endpoint

            //PersistedData fields: 

            //CreateByUsId
            //CreateDateUtc
            //DataLength
            //DataMimeType
            //ExpirationDateUtc
            //Id
            //LinkedObjectId
            //LinkedObjectType
            //ModifiedByUsId
            //ModifiedDateUtc
            //Name
            //PersistedData
            //Scope

            //example of getting the persisted data by name (vs. using the Id) using a query
            //this api call returns a page of data matching the query parameters
            //in this example we only expect to get one item

            //basic query syntax (note field names must be enclosed in square brackets)
            //
            //[field name] {logical operator} '{predicate}'
            //
            //

            Page<PersistedClientData> dataPage = vaultApi.PersistedData.GetAllData(new RequestOptions() { Query = string.Format("[Name] eq '{0}'", persistedDataUniqueName) });

            Assert.IsNotNull(dataPage);

            var user = vaultApi.Users.GetUser(ResourceOwnerUserName);

            if (user != null)
            {
                string loginToken = user.GetWebLoginToken();

                Assert.IsNotNullOrEmpty(loginToken);

                string url = string.Format("{0}/v1/en/Customer412/Main/vvlogin?token={1}&returnurl=userportal%3fportalname={2}%26persistedId={3}", VaultApiBaseUrl, loginToken, "InvoiceData", data.Id);

                LogEventManager.Info(string.Format("Token login URL with Persisted Data Id query string parameter:{0} {1}", Environment.NewLine, url));
            }

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

        [Test]
        public void GetApiObjectFieldNames()
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

            List<MetaDataType> ApiObjectTypes = vaultApi.Meta.GetDataTypes();

            StringBuilder sbFieldList = new StringBuilder();

            sbFieldList.AppendLine("API Object Field List");

            foreach (MetaDataType apiObjectType in ApiObjectTypes)
            {
                sbFieldList.AppendLine("-----------------------------------------------------------------");

                sbFieldList.AppendLine(string.Format("{0} fields: {1}", apiObjectType.Name, Environment.NewLine));

                foreach (string fieldName in apiObjectType.AvailableFields)
                {
                    sbFieldList.AppendLine(string.Format("{0}", fieldName));
                }
            }

            LogEventManager.Info(sbFieldList.ToString());
        }

        #endregion
    }
}