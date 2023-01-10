using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Forms.FormInstances;
using VVRestApi.Vault;

namespace VVRestApi.Forms
{
    /// <summary>
    /// Methods to make calls to the forms api. Beta Form viewer must be enabled and a forms api url must be provided to enable this class
    /// If disabled usage will throw an error
    /// </summary>
    public class FormsApi : BaseApi
    {
        public bool IsEnabled { get; set; }
        public string BaseUrl { get; set; }

        internal FormsApi()
        {

        }

        /// <summary>
        /// Creates a FormsApi instance
        /// </summary>
        /// <remarks>If token is not a JWT, the FormsApi object will be disabled</remarks>
        /// <param name="api"></param>
        /// <param name="jwt">Must be a JWT</param>
        internal FormsApi(VaultApi api, Tokens jwt)
        {
            var formsApiConfig = api.ConfigurationManager.GetFormsApiConfiguration();

            
            if (formsApiConfig == null || !jwt.IsJwt)
                return;// leave disabled

            IsEnabled = formsApiConfig.IsEnabled;
            BaseUrl = formsApiConfig.FormsApiUrl;

            base.Populate(api.ClientSecrets, jwt);

            FormInstances = new FormInstancesManager(this);
        }

        /// <summary>
        /// Retrieve and manage form instances
        /// </summary>
        public FormInstancesManager FormInstances { get; private set; }

        /// <summary>
        /// Populates the token
        /// </summary>
        /// <param name="clientSecrets"></param>
        /// <param name="apiTokens"> </param>
        internal void Populate(FormsApi api)
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
