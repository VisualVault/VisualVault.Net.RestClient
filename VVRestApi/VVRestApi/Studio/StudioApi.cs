using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Forms.FormInstances;
using VVRestApi.Studio.Workflow;
using VVRestApi.Vault;
using VVRestApi.Vault.Configuration;

namespace VVRestApi.Forms
{
    /// <summary>
    /// Methods to make calls to the forms api. Beta Form viewer must be enabled and a forms api url must be provided to enable this class
    /// If disabled usage will throw an error
    /// </summary>
    public class StudioApi : BaseApi
    {
        public bool IsEnabled { get; set; }
        public string BaseUrl { get; set; }

        internal StudioApi()
        {

        }

        /// <summary>
        /// Creates a StudioApi instance
        /// </summary>
        /// <remarks>If token is not a JWT, the StudioApi object will be disabled</remarks>
        /// <param name="api"></param>
        /// <param name="jwt">Must be a JWT</param>
        internal StudioApi(VaultApi api, Tokens jwt)
        {
            var studioApiConfig = api.ConfigurationManager.GetStudioApiConfiguration();


            if (studioApiConfig == null || !jwt.IsJwt)
                return;// leave disabled

            IsEnabled = studioApiConfig.IsEnabled;
            BaseUrl = studioApiConfig.StudioApiUrl;

            base.Populate(api.ClientSecrets, jwt);

            Workflow = new WorkflowManager(this);
        }

        /// <summary>
        /// Retrieve and manage form instances
        /// </summary>
        public WorkflowManager Workflow { get; private set; }

        /// <summary>
        /// Populates the token
        /// </summary>
        /// <param name="clientSecrets"></param>
        /// <param name="apiTokens"> </param>
        internal void Populate(StudioApi api)
        {
            this.ClientSecrets = api.ClientSecrets;
            this.ApiTokens = api.ApiTokens;
            this.BaseUrl = api.BaseUrl;
            this.IsEnabled = api.IsEnabled;
        }

        internal new UrlParts GetUrlParts()
        {
            UrlParts urlParts = new UrlParts
            {
                ApiVersion = ClientSecrets.ApiVersion,
                BaseUrl = BaseUrl,
                OAuthTokenEndPoint = ClientSecrets.OAuthTokenEndPoint
            };

            return urlParts;
        }
    }
}
