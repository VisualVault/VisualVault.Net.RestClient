using Newtonsoft.Json.Linq;
using System;
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

            var result = HttpHelper.Post(GlobalConfiguration.Routes.Scripts, queryString, GetUrlParts(), ApiTokens, ClientSecrets, postData);

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

            var result = HttpHelper.Post(GlobalConfiguration.Routes.Scripts, queryString, GetUrlParts(), ApiTokens, ClientSecrets, postData);
            return result;
        }

        /// <summary>
        /// Allows for sending a response to workflow execution
        /// </summary>
        /// <param name="executionId">The ID of the Workflow execution</param>
        /// <param name="workflowVariables">Response variables object to send to workflow</param>
        /// <returns></returns>
        public JObject CompleteWorkflowWebService(string executionId, dynamic workflowVariables)
        {
            dynamic postData = new
            {
                executionId,
                workflowVariables
            };

            var result = HttpHelper.Post(GlobalConfiguration.Routes.CompleteWfScript, null, GetUrlParts(), ApiTokens, ClientSecrets, postData);
            return result;
        }
    }
}