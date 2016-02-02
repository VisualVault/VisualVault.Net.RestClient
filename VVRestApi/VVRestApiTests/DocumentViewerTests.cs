using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using VVRestApi.Common.Extensions;
using Newtonsoft.Json.Linq;
using VVRestApi.Common.Logging;
using VVRestApi.Vault.DocumentViewer;
using VVRestApi.Vault.Forms;
using VVRestApi.Vault.Library;
using VVRestApi.Vault.Meta;
using VVRestApi.Vault.PersistedData;
using VVRestApi.Vault.Users;




namespace VVDocumentViewerTests
{
    using NUnit.Framework;
    using VVRestApi.Common;
    using VVRestApi.Vault;

    /// <summary>
    ///     The login tests.
    /// </summary>
    [TestFixture]
    public class DocumentViewerTests
    {


        #region Constants
        
        //Base URL to VisualVault.  Copy URL string preceding the version number ("/v1")
        const string VaultApiBaseUrl = "http://development7/VisualVault4_1_12";

        //API version number (number following /v in the URL).  Used to provide backward compatitiblity.
        const string ApiVersion = "1";

        //OAuth2 token endpoint, exchange credentials for api access token
        //typically the VaultApiBaseUrl + /oauth/token unless using an external OAuth server
        private const string OAuthServerTokenEndPoint = "http://development7/VisualVault4_1_12/oauth/token";

        //your customer alias value.  Visisble in the URL when you log into VisualVault
        const string CustomerAlias = "AceOfHearts";

        //your customer database alias value.  Visisble in the URL when you log into VisualVault
        const string DatabaseAlias = "Main";

        //Copy "API Key" value from User Account Property Screen
        const string ClientId = "ce9e042b-8755-42d5-97af-435afe70152b";


        //Copy "API Secret" value from User Account Property Screen
        const string ClientSecret = "/PbgaChHbPoboS/1s07E6pfGCNFSdqPsDnB/yiKHfHw=";


        // Scope is used to determine what resource types will be available after authentication.  If unsure of the scope to provide use
        // either 'vault' or no value.  'vault' scope is used to request access to a specific customer vault (aka customer database). 
        const string Scope = "vault";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string ResourceOwnerUserName = "ace.admin";
        const string ResourceOwnerUsID = "ABD2F88A-8861-E111-8E23-14FEB5F06078";

        /// <summary>
        /// Resource owner is a VisualVault user with access to resources.  An OAuth 2 enabled client application exchanges the resource owner credentials for an access token.
        /// </summary>
        const string ResourceOwnerPassword = "p";


        //const string VaultApiBaseUrl = "http://development7/VisualVault4_1_10";
        //private const string OAuthServerTokenEndPoint = "http://development7/VisualVault4_1_10/oauth/token";
        //const string ApiVersion = "1";
        //const string CustomerAlias = "Amazon";
        //const string DatabaseAlias = "Main";
        //const string ClientId = "0ec5fb65-95e3-4833-847e-4d299752bd64";
        //const string ClientSecret = "QzeojEH6vDylTXmYgSrcPqn6AQKWgx/VauvPxNeUXjw=";
        //const string Scope = "vault";
        //const string ResourceOwnerUserName = "amazon.admin";
        //const string ResourceOwnerUsID = "4EEE59B9-0599-E511-AB25-5CF3706C36ED";
        //const string ResourceOwnerPassword = "p";

        #endregion



        #region Shared Document Tests

        [Test]
        public void GetDocumentsSharedWithMe()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = DocumentViewerTests.ClientId,
                ApiSecret = DocumentViewerTests.ClientSecret,
                OAuthTokenEndPoint = DocumentViewerTests.OAuthServerTokenEndPoint,
                BaseUrl = DocumentViewerTests.VaultApiBaseUrl,
                ApiVersion = DocumentViewerTests.ApiVersion,
                CustomerAlias = DocumentViewerTests.CustomerAlias,
                DatabaseAlias = DocumentViewerTests.DatabaseAlias,
                Scope = DocumentViewerTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var documentList = vaultApi.DocumentShares.GetDocumentsSharedWithMe();

            Assert.IsNotNull(documentList);
        }

        [Test]
        public void ShareDocumentWithUser()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = DocumentViewerTests.ClientId,
                ApiSecret = DocumentViewerTests.ClientSecret,
                OAuthTokenEndPoint = DocumentViewerTests.OAuthServerTokenEndPoint,
                BaseUrl = DocumentViewerTests.VaultApiBaseUrl,
                ApiVersion = DocumentViewerTests.ApiVersion,
                CustomerAlias = DocumentViewerTests.CustomerAlias,
                DatabaseAlias = DocumentViewerTests.DatabaseAlias,
                Scope = DocumentViewerTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            //var userWopUsId = new Guid("369493C9-36AD-E211-9D53-14FEB5F06078");
            var userWpUsId = new Guid("A6DFFCC3-35AD-E211-9D53-14FEB5F06078");

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentShare = vaultApi.DocumentShares.ShareDocument(dlId, userWpUsId);

            Assert.IsNotNull(documentShare);
        }

        [Test]
        public void ShareDocumentWithUsers()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = DocumentViewerTests.ClientId,
                ApiSecret = DocumentViewerTests.ClientSecret,
                OAuthTokenEndPoint = DocumentViewerTests.OAuthServerTokenEndPoint,
                BaseUrl = DocumentViewerTests.VaultApiBaseUrl,
                ApiVersion = DocumentViewerTests.ApiVersion,
                CustomerAlias = DocumentViewerTests.CustomerAlias,
                DatabaseAlias = DocumentViewerTests.DatabaseAlias,
                Scope = DocumentViewerTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            var userWopUsId = new Guid("369493C9-36AD-E211-9D53-14FEB5F06078");
            var userWpUsId = new Guid("A6DFFCC3-35AD-E211-9D53-14FEB5F06078");
            var aceAdmin = new Guid("ABD2F88A-8861-E111-8E23-14FEB5F06078");

            var userList = new List<Guid>
            {
                userWopUsId,
                userWpUsId,
                aceAdmin
            };

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentShare = vaultApi.DocumentShares.ShareDocument(dlId, userList);

            Assert.IsNotEmpty(documentShare);
        }

        [Test]
        public void RemoveUserFromDocumentShare()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = DocumentViewerTests.ClientId,
                ApiSecret = DocumentViewerTests.ClientSecret,
                OAuthTokenEndPoint = DocumentViewerTests.OAuthServerTokenEndPoint,
                BaseUrl = DocumentViewerTests.VaultApiBaseUrl,
                ApiVersion = DocumentViewerTests.ApiVersion,
                CustomerAlias = DocumentViewerTests.CustomerAlias,
                DatabaseAlias = DocumentViewerTests.DatabaseAlias,
                Scope = DocumentViewerTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            var userWopUsId = new Guid("369493C9-36AD-E211-9D53-14FEB5F06078");

            vaultApi.DocumentShares.RemoveUserFromSharedDocument(dlId, userWopUsId);

            //Assert.IsNotNull(document);
        }

        [Test]
        public void GetListOfDocumentSharesOfDocument()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = DocumentViewerTests.ClientId,
                ApiSecret = DocumentViewerTests.ClientSecret,
                OAuthTokenEndPoint = DocumentViewerTests.OAuthServerTokenEndPoint,
                BaseUrl = DocumentViewerTests.VaultApiBaseUrl,
                ApiVersion = DocumentViewerTests.ApiVersion,
                CustomerAlias = DocumentViewerTests.CustomerAlias,
                DatabaseAlias = DocumentViewerTests.DatabaseAlias,
                Scope = DocumentViewerTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");

            //var document = vaultApi.Documents.GetDocument(dlId, new RequestOptions { Fields = "Id,DhId,FieldId,FieldType,Label,Required,Value,OrdinalPosition,CreateDate,CreateById,CreateBy,ModifyDate,ModifyBy,ModifyById" });
            var documentList = vaultApi.DocumentShares.GetListOfUsersDocumentSharedWith(dlId);

            Assert.IsNotNull(documentList);
        }

        [Test]
        public void GetDocumentViewerUrlFromShareLink()
        {
            var clientSecrets = new ClientSecrets
            {
                ApiKey = DocumentViewerTests.ClientId,
                ApiSecret = DocumentViewerTests.ClientSecret,
                OAuthTokenEndPoint = DocumentViewerTests.OAuthServerTokenEndPoint,
                BaseUrl = DocumentViewerTests.VaultApiBaseUrl,
                ApiVersion = DocumentViewerTests.ApiVersion,
                CustomerAlias = DocumentViewerTests.CustomerAlias,
                DatabaseAlias = DocumentViewerTests.DatabaseAlias,
                Scope = DocumentViewerTests.Scope
            };


            var vaultApi = new VaultApi(clientSecrets);

            Assert.IsNotNull(vaultApi);

            var dlId = new Guid("6ADC119D-C6A8-E411-8278-14FEB5F06078");
            //var userWopUsId = new Guid("369493C9-36AD-E211-9D53-14FEB5F06078");

            var documentShare = vaultApi.DocumentShares.GetDocumentShareLink(dlId, RoleType.Editor);

            //Assert.IsNotNull(documentShare);

            //Assert.AreEqual(documentShare.ShareLinkType, DocumentShareLinkType.InternalShareLink);
            //Assert.AreEqual(documentShare.UserId, userWopUsId);
            //Assert.AreEqual(documentShare.LinkRole, RoleType.Editor);

            //var fullLink = "http://aws.visualvault.com/staples/view/?xcdid=d1YejmGIEeGOIxT-tfBgeA&d=nRHcaqjGEeSCeBT-tfBgeA52A8_614EeSCeRT-tfBgeA&authKey=aJnBqYCoBUWqJm2GYeX2yw&src=share";
            //var link = fullLink.Split('?')[1];

            var fullLink = documentShare.ShareLink;
            var link = fullLink.Split('?')[1];

            var viewerUrl = DocumentViewerManager.GetDocumentViewerUrlFromLink(link, clientSecrets);

            Assert.IsNotNull(viewerUrl);
        }


        #endregion



    }
}