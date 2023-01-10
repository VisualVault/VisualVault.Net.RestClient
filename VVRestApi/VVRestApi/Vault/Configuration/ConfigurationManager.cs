using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault.Users;

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
        /// Gets the configuration of the Forms Api for the Customer Database
        /// </summary>
        /// <returns>FormsApiConfig</returns>
        public FormsApiConfig GetFormsApiConfiguration()
        {
            return HttpHelper.Get<FormsApiConfig>(GlobalConfiguration.Routes.ConfigurationFormsApi, "", null, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }
       
    }
}