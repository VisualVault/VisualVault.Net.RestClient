using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationManager : BaseApi
    {
        internal ConfigurationManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CustomerDatabaseConfiguration GetDatabaseConfiguration()
        {
            return HttpHelper.Get<CustomerDatabaseConfiguration>(GlobalConfiguration.Routes.Configuration, "", null, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        /// <summary>
        /// Gets the configuration of the Doc Api for the Customer Database
        /// </summary>
        /// <returns>DocApiConfig</returns>
        public DocApiConfig GetDocApiConfiguration()
        {
            return HttpHelper.Get<DocApiConfig>(GlobalConfiguration.Routes.ConfigurationDocApi, "", null, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        /// <summary>
        /// Gets the configuration of the Forms Api for the Customer Database
        /// </summary>
        /// <returns>FormsApiConfig</returns>
        public FormsApiConfig GetFormsApiConfiguration()
        {
            return HttpHelper.Get<FormsApiConfig>(GlobalConfiguration.Routes.ConfigurationFormsApi, "", null, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        /// <summary>
        /// Gets the configuration of the Studio Api for the Customer Database
        /// </summary>
        /// <returns>StudioApiConfig</returns>
        public StudioApiConfig GetStudioApiConfiguration()
        {
            return HttpHelper.Get<StudioApiConfig>(GlobalConfiguration.Routes.ConfigurationStudioApi, "", null, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

    }
}