using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVRestApi.Common;
using VVRestApi.Studio.DTO;
using VVRestApi.Studio.Models;
using VVRestApi.Vault;

namespace VVRestApiTests.Tests
{
    [TestFixture]
    public class StudioApiTests : IClientSecrets
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
        const string _CustomerAlias = "Main";

        //your customer database alias value.  Visisble in the URL when you log into VisualVault
        const string _DatabaseAlias = "VisualVaultCustomerMaintest";

        //Copy "API Key" value from User Account Property Screen
        const string _ClientId = "fab5d5b7-0878-4018-a27c-c95d3cd04f45";


        //Copy "API Secret" value from User Account Property Screen
        const string _ClientSecret = "4NPWkHSvbeR0yPQ+vbq3avoGONUSIgUBfcReLidq1NA=";


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
        public void GetWorkflow()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var workflow = vaultApi.StudioApi.Workflow.GetWorkflow(new Guid("94f5b74e-6aa6-4349-b5da-be8f3e318edd"));
            Assert.IsNotNull(workflow);
            Assert.IsNotNull(workflow.Name);
        }

        [Test]
        public void GetWorkflowByName()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var workflow = vaultApi.StudioApi.Workflow.GetWorkflowByName("Test");
            Assert.IsNotNull(workflow);
            Assert.IsNotNull(workflow.Name);
        }

        [Test]
        public void GetWorkflowVariables()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var workflow = vaultApi.StudioApi.Workflow.GetWorkflowByName("Test");
            Assert.IsNotNull(workflow);

            var variables = vaultApi.StudioApi.Workflow.GetWorkflowVariables(workflow.Id);
            Assert.IsNotNull(variables);
        }

        [Test]
        public void RunWorkflow()
        {
            VaultApi vaultApi = new VaultApi(this);

            Assert.IsNotNull(vaultApi);

            var workflow = vaultApi.StudioApi.Workflow.GetWorkflowByName("Test");
            Assert.IsNotNull(workflow);

            var data = new List<WorkflowVariable>()
            {
                new WorkflowVariable("TESTVariable", "A Value"),
                new WorkflowVariable("NotPresentVariable", "A Second Value"),
            };

            var result = vaultApi.StudioApi.Workflow.TriggerWorkflow(workflow.Id, workflow.Revision, Guid.NewGuid(), data);
            Assert.IsTrue(result != Guid.Empty);
        }

        #endregion
    }
}
