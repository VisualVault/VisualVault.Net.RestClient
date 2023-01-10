using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Scripts
{
    /// <summary>
    /// 
    /// </summary>
    public class ScriptsManager : BaseApi
    {
        internal ScriptsManager(VaultApi api)
        {
            Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// Executes a web service in VisualVault and returns the data passed back from the service, if applicable
        /// </summary>
        /// <param name="serviceName">The Name of the web service in VisualVault</param>
        /// <param name="serviceData">The data to be passed to the script</param>
        /// <returns></returns>
        public JObject RunWebService(string serviceName, JArray serviceData)
        {
            var queryString = $"name={serviceName}";
            dynamic postData = serviceData;

            var result = HttpHelper.Post(GlobalConfiguration.Routes.Scripts, queryString, GetUrlParts(), this.ApiTokens, ClientSecrets, postData);

            return result;
        }

        /// <summary>
        /// Executes a web service in VisualVault and returns the data passed back from the service, if applicable
        /// </summary>
        /// <param name="serviceName">The Name of the web service in VisualVault</param>
        /// <param name="usId">The user for whom to retrieve user preference settings when executing the web service</param>
        /// <param name="serviceData">The data to be passed to the script</param>
        /// <returns></returns>
        public JObject RunWebService(string serviceName, Guid usId, JArray serviceData)
        {
            var queryString = $"name={serviceName}&usId={usId}";
            dynamic postData = serviceData;

            var result = HttpHelper.Post(GlobalConfiguration.Routes.Scripts, queryString, GetUrlParts(), this.ApiTokens, ClientSecrets, postData);

            return result;
        }
    }
}