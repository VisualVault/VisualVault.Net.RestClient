// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginTests.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// <summary>
//   The login tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using NUnit.Framework;
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
using VVRestApi.Vault.Configuration;
using VVRestApi.Vault.DocumentViewer;
using VVRestApi.Vault.Forms;
using VVRestApi.Vault.Library;
using VVRestApi.Vault.Meta;
using VVRestApi.Vault.PersistedData;
using VVRestApi.Vault.Users;
using VVRestApiTests.TestHelpers;

namespace VVRestApiTests.Core.Tests
{
    using System.Net;
    using VVRestApi.Common;
    using VVRestApi.Vault;

    /// <summary>
    ///     The login tests.
    /// </summary>
    [TestFixture]
    public class FormsApiTests : IClientSecrets
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

        //VisualVault FolderStore Constants
        public const string GeneralFolderDefaultName = "General";
        public string AttachmentsFolderDefaultName = "Attachments";

        // Scope is used to determine what resource types will be available after authentication.  If unsure of the scope to provide use
        // either 'vault' or no value.  'vault' scope is used to request access to a specific customer vault (aka customer database). 
        const string _Scope = "vault";

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

        #endregion

        #region Form Instances
        [Test]
        public void SaveFormInstance()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var template = vaultApi.FormTemplates.GetFormTemplate("Child2");
            Assert.IsNotNull(template);

            var fields = new List<KeyValuePair<string, object>>();
            fields.Add(new KeyValuePair<string, object>("txtTextRename", "jj"));
            fields.Add(new KeyValuePair<string, object>("calDate", "Bad Date Example"));



            var formInstance = vaultApi.FormsApi.FormInstances.CreateNewFormInstance(template.RevisionId, fields);
            Assert.IsNotNull(formInstance);
        }


        [Test]
        public void UpdateFormInstance()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var template = vaultApi.FormTemplates.GetFormTemplate("Child2");
            Assert.IsNotNull(template);

            var fields = new List<KeyValuePair<string, object>>();
            fields.Add(new KeyValuePair<string, object>("txtTextRename", "jj"));
            fields.Add(new KeyValuePair<string, object>("calDate", "Bad Date Example"));

            var formInstance = vaultApi.FormsApi.FormInstances.CreateNewFormInstance(template.RevisionId, fields);
            Assert.IsNotNull(formInstance);

            var fields2 = new List<KeyValuePair<string, object>>();
            fields2.Add(new KeyValuePair<string, object>("txtTextRename", "jj2"));
            fields2.Add(new KeyValuePair<string, object>("calDate", "Bad Date Example2"));

            var updatedFormInstance = vaultApi.FormsApi.FormInstances.CreateNewFormInstanceRevision(template.RevisionId, formInstance.FormId, fields2);
            Assert.IsNotNull(updatedFormInstance);

        }

        #endregion
    }
}
