// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginTests.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// <summary>
//   The login tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using VVRestApi.Vault.Forms;
using VVRestApi.Vault.PersistedData;

namespace VVRestApiTests
{
    using NUnit.Framework;
    using VVRestApi;
    using VVRestApi.Common;
    using VVRestApi.Vault;

    /// <summary>
    ///     The login tests.
    /// </summary>
    [TestFixture]
    public class LoginTests
    {
        const string VaultServerUrl = "http://localhost/VisualVault4_0_0";             //Base URL to VisualVault.  Copy URL string preceding the version number ("/v1")
        const string CustomerAlias = "AcmeInc";
        const string DatabaseAlias = "Main";
        const string ApiKey = "00000000-0000-0000-0000-000000000000";                   //Copy "API Key" value from User Account Property Screen
        const string DeveloperId = "00000000-0000-0000-0000-000000000000";              //Copy "API Key" value from User Account Property Screen (APIKey and Developer Id should be the same value)"
        const string DeveloperSecret = "00000000-0000-0000-0000-000000000000";          //Copy "API Secret" value from User Account Property Screen


        #region Public Methods and Operators

        /// <summary>
        /// The login test.
        /// </summary>
        [Test]
        public void LoginTest()
        {
            VaultApi vault = Authentication.GetVaultApi(ApiKey, DeveloperId, DeveloperSecret, VaultServerUrl, CustomerAlias, DatabaseAlias);
            Assert.IsNotNull(vault);
        }

        [Test]
        public void FormTemplatesTest()
        {
            VaultApi vault = Authentication.GetVaultApi(ApiKey, DeveloperId, DeveloperSecret, VaultServerUrl, CustomerAlias, DatabaseAlias);
            Assert.IsNotNull(vault);

            Page<FormTemplate> formTemplates = vault.FormTemplates.GetFormTemplates();

            foreach (FormTemplate formTemplate in formTemplates.Items)
            {
                Assert.IsNotNullOrEmpty(formTemplate.Name);
            }

            Assert.IsNotNull(formTemplates);

            var user = vault.Users.GetUser("test");

            string token = user.GetWebLoginToken();
        }

        [Test]
        public void PersistedDataTest()
        {
            VaultApi vault = Authentication.GetVaultApi(ApiKey, DeveloperId, DeveloperSecret, VaultServerUrl, CustomerAlias, DatabaseAlias);
            Assert.IsNotNull(vault);

            const string dataToStore = "{\"patientId\":\"12345\",\"claimNumber\":\"12345\"}";

            PersistedClientData data = vault.PersistedData.CreateData(ShortGuid.NewGuid().ToString(), ScopeType.Global, dataToStore, "text/JSON", "", LinkedObjectType.None, null);

            Assert.IsNotNull(data);

            //use data id to retrieve the data
            string dataId = data.Id.ToString();

            string loginToken = vault.CurrentUser.GetCurrentUser().GetWebLoginToken();

            string url = string.Format("{0}/v1/en/ACMEINC/Main/vvlogin?authtoken={1}&returnurl=userportal?portalname={2}%26persistedId={3}", VaultServerUrl, loginToken,"claims",dataId);

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

        #endregion
    }
}