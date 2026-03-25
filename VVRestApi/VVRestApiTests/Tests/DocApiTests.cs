// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginTests.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// <summary>
//   The login tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using NUnit.Framework;

namespace VVRestApiTests.Tests
{
    using System;
    using VVRestApi.Common;
    using VVRestApi.Documents;
    using VVRestApi.Vault;

    /// <summary>
    ///     The login tests.
    /// </summary>
    [TestFixture]
    public class DocApiTests : IClientSecrets
    {

        #region Constants

        //Base URL to VisualVault.  Copy URL string preceding the version number ("/v1")
        const string _VaultApiBaseUrl = "http://localhost/visualvault4_1_13";

        //API version number (number following /v in the URL).  Used to provide backward compatitiblity.
        const string _ApiVersion = "1";

        //OAuth2 token endpoint, exchange credentials for api access token
        //typically the VaultApiBaseUrl + /oauth/token unless using an external OAuth server
        private const string _OAuthServerTokenEndPoint = "http://localhost/visualvault4_1_13/oauth/token";

        //your customer alias value.  Visisble in the URL when you log into VisualVault
        const string _CustomerAlias = "Cust";

        //your customer database alias value.  Visisble in the URL when you log into VisualVault
        const string _DatabaseAlias = "Default";

        //Copy "API Key" value from User Account Property Screen
        const string _ClientId = "860028f6-0fbf-4a13-99fd-598dcaad6a36";

        //Copy "API Secret" value from User Account Property Screen
        const string _ClientSecret = "c5MCZasWnIeEerz6SnXQw5WGE1r3JIxN7LhR66E0APU=";

        // Scope is used to determine what resource types will be available after authentication.  If unsure of the scope to provide use
        // either 'vault' or no value.  'vault' scope is used to request access to a specific customer vault (aka customer database). 
        const string _Scope = "vault";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string _ResourceOwnerUserName = "";
        const string _ResourceOwnerUsID = "";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string _ResourceOwnerPassword = "";

        /// <summary>
        /// Audience is used to identify known applications. If unsure of the audience, leave blank
        /// </summary>
        const string _Audience = "";

        #endregion

        #region Test Data

        /// <summary>
        /// FolderId used for DocApi create/update tests (local environment)
        /// </summary>
        const string _TestFolderId = "07dcaf21-3197-4c85-bae0-1d2e48b64f3c";

        #endregion

        #region Client Authentication Properties

        public string BaseUrl
        {
            get { return _VaultApiBaseUrl; }
        }
        public string ApiKey
        {
            get { return _ClientId; }
        }
        public string ApiSecret
        {
            get { return _ClientSecret; }
        }
        public string CustomerAlias
        {
            get { return _CustomerAlias; }
        }
        public string DatabaseAlias
        {
            get { return _DatabaseAlias; }
        }
        public string ApiVersion
        {
            get { return _ApiVersion; }
        }
        public string OAuthTokenEndPoint
        {
            get { return _OAuthServerTokenEndPoint; }
        }
        public string Scope
        {
            get { return _Scope; }
        }

        public string Audience
        {
            get { return _Audience; }
        }

        #endregion

        #region Tests
        [Test]
        public void GetDocumentRevision()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dhId = Guid.Parse("ff120053-caea-ed11-84da-00d49e2382b8");

            var template = vaultApi.DocApi.GetRevision(dhId);
            Assert.IsNotNull(template);
            Assert.AreEqual(dhId, template.Id);
        }

        [Test]
        public void GetDocumentOcrStatus()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var dhId = Guid.Parse("ff120053-caea-ed11-84da-00d49e2382b8");

            var template = vaultApi.DocApi.GetDocumentOcrStatus(dhId);
            Assert.IsNotNull(template);
            Assert.AreEqual(dhId, template.Id);
        }

        [Test]
        public void CreateAndUpdateDocument()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var name = $"DOC-TEST-{Guid.NewGuid():N}".Substring(0, 14);

            var createRequest = new DocApiCreateDocumentRequest
            {
                FolderId = _TestFolderId,
                Name = name,
                Description = "DocApi create test",
                Revision = "1",
                DocumentState = "Released",
                FileName = "test.txt",
                FileLength = 0
            };

            var created = vaultApi.DocApi.CreateDocument(createRequest);
            Assert.IsNotNull(created);
            Assert.IsNotEmpty(created.DocumentId.ToString());

            var updateRequest = new DocApiUpdateDocumentRequest
            {
                Description = "DocApi update test",
                Keywords = "test,docapi"
            };

            var updated = vaultApi.DocApi.UpdateDocument(created.DocumentId, updateRequest);
            Assert.IsNotNull(updated);
            Assert.AreEqual(created.DocumentId, updated.DocumentId);
        }

        


        #endregion
    }
}
