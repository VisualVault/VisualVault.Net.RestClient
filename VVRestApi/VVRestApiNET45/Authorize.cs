// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Authorize.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace VVRestApi
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    using VVRestApi.Administration;
    using VVRestApi.Common;
    using VVRestApi.Common.Extensions;
    using VVRestApi.Common.Logging;
    using VVRestApi.Common.Messaging;
    using VVRestApi.Vault;

    /// <summary>
    ///     The login.
    /// </summary>
    public static class Authorize
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets an API for creating customers, databases, users and configuration settings.
        /// </summary>
        /// <param name="loginToken">The login token or api key for the account you want to access</param>
        /// <param name="developerId">The ID of the application. See http://developers.visualvault.com for more information</param>
        /// <param name="developerSecret">The secret key of the application. Used to sign the JWT request, but not sent over the wire.</param>
        /// <param name="baseVaultUrl">The base URL to VisualVault without /api/{version} on it</param>
        /// <returns></returns>
        public static AdministrationApi GetAdmininistrationApi(string loginToken, string developerId, string developerSecret,  string baseVaultUrl)
        {
            AdministrationApi api = null;

            SessionToken authToken = GetAuthenticatedToken(loginToken, developerId, developerSecret, GlobalConfiguration.AdminApiJwtIssuer, "config", "admin", baseVaultUrl);
            if (authToken != null && authToken.IsValid() && authToken.TokenType == TokenType.Config)
            {
                api = new AdministrationApi(authToken);
            }

            return api;
        }
        
        /// <summary>
        ///     Obtain a working Vault by logging in with a valid loginToken and secret key along with the aliases of the customer and database you want to work with.
        ///     Vaults are the specific customer database instances you work with.
        /// </summary>
        /// <param name="loginToken">The login token or api key for the account you want to access</param>
        /// <param name="developerId">The ID of the application. See http://developers.visualvault.com for more information</param>
        /// <param name="developerSecret">The secret key of the application. Used to sign the JWT request, but not sent over the wire.</param>
        /// <param name="customerAlias">The customer alias or Config</param>
        /// <param name="databaseAlias">The database alias,  Admin if the customer alias is Config </param>
        /// <param name="baseVaultUrl">The Base Vault URL without the api/{version} in it</param>
   /// <returns></returns>
        public static VaultApi GetVaultApi(string loginToken, string developerId, string developerSecret, string baseVaultUrl, string customerAlias, string databaseAlias)
        {
            VaultApi vaultApi = null;

            if (customerAlias.Equals("config", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentOutOfRangeException("customerAlias", "Customer Alias cannot be 'config'. Use GetAdministrationApi to get the desired API.");
            }

            SessionToken authToken = GetAuthenticatedToken(loginToken, developerId, developerSecret, GlobalConfiguration.VaultApiJwtIssuer, customerAlias, databaseAlias, baseVaultUrl);
            if (authToken != null && authToken.IsValid() && authToken.TokenType == TokenType.Vault)
            {
                vaultApi = new VaultApi(authToken);
            }

            return vaultApi;
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginToken">Username to add as the "iss" in the JWT token</param>
        /// <param name="appSecretKey">The secret key to encrypt the token with</param>
        /// <param name="customerAlias">The customer alias or Config</param>
        /// <param name="databaseAlias">The database alias, Admin if the customer alias is Config </param>
        /// <param name="baseVaultUrl">The Base Vault URL without the api/{version} in it</param>
        /// <param name="extraValuesToSend"></param>
        /// <returns></returns>
        private static SessionToken GetAuthenticatedToken(string loginToken, string developerId, string developerSecret, string secretKeyIssuer, string customerAlias, string databaseAlias, string baseVaultUrl, Dictionary<string, object> extraValuesToSend = null)
        {
            SessionToken authToken = null;
            string token = GetJwtToken(loginToken, developerId, developerSecret, secretKeyIssuer, extraValuesToSend);

            if (!baseVaultUrl.EndsWith("/"))
            {
                baseVaultUrl += "/";
            }

            string baseUrl = baseVaultUrl + "api/v1/";
            string customerDatabaseUrl = string.Format("{0}{1}/{2}/", baseUrl, customerAlias, databaseAlias);
            string url = customerDatabaseUrl + "authorize";

            JObject resultData = null;

            var client = new HttpClient();
            var parsedUri = new Uri(url);
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string signature = HttpHelper.CreateAuthorization(client.DefaultRequestHeaders, parsedUri, "GET", string.Empty, developerId, developerSecret);
            client.DefaultRequestHeaders.Add("X-Authorization", signature);
            

            HttpHelper.OutputCurlCommand(client, HttpMethod.Get, url, null, true);

            Task task = client.GetAsync(url).ContinueWith(async taskwithresponse =>
            {
                var result = await taskwithresponse.Result.Content.ReadAsAsync<JObject>();
                resultData = HttpHelper.ProcessResultData(result, url, HttpMethod.Get);
            });
            task.Wait();

            if (resultData != null)
            {
                if (resultData.IsHttpStatus(HttpStatusCode.OK))
                {
                    dynamic expData = resultData.ToObject<ExpandoObject>();

                    if (expData.data != null && expData.data.token != null)
                    {
                        authToken = new SessionToken(baseUrl, customerAlias, databaseAlias, expData.data.token, expData.data.expires, developerId, developerSecret);
                    }
                }
            }

            return authToken;
        }

     
        private static string GetJwtToken(string loginToken, string developerId, string developerSecret, string secretKeyIssuer, Dictionary<string, object> extraValues = null)
        {

            byte[] byteKey = Convert.FromBase64String(developerSecret);

       

            if (!String.IsNullOrWhiteSpace(developerId))
            {
                if (extraValues == null)
                {
                    extraValues = new Dictionary<string, object>();
                }

                extraValues.Add("devid", developerId);
            }

            Dictionary<string, object> payload = JsonWebToken.GenerateDefaultPayload(loginToken, secretKeyIssuer, extraValues);
            var jsonPayload = JsonConvert.SerializeObject(payload);
            LogEventManager.Debug("JWT Payload: " + Environment.NewLine + jsonPayload);
            string token = JsonWebToken.Encode(payload, byteKey, JwtHashAlgorithm.HS256);

            return token;
        }





        #endregion
    }
}