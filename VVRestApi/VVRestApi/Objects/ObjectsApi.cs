using System;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Objects.Model;
using VVRestApi.Objects.Object;
using VVRestApi.Vault;

namespace VVRestApi.Objects
{
    public class ObjectsApi : BaseApi
    {
        public bool IsEnabled { get; set; }
        public string BaseUrl { get; set; }

        internal ObjectsApi() { }

        /// <summary>
        /// Creates a ObjectsAPI instance
        /// </summary>
        /// <remarks>If token is not a JWT, the ObjectsAPI object will be disabled</remarks>
        /// <param name="api"></param>
        /// <param name="jwt">Must be a JWT</param>
        internal ObjectsApi(VaultApi api, Tokens jwt)
        {
            var objectsApiConfig = api.ConfigurationManager.GetObjectsApiConfiguration();


            if (objectsApiConfig == null || !jwt.IsJwt)
                return;// leave disabled

            IsEnabled = objectsApiConfig.IsEnabled;
            BaseUrl = objectsApiConfig.ObjectsApiUrl;

            base.Populate(api.ClientSecrets, jwt);

            Models = new ModelsManager(this);
            Objects = new ObjectsManager(this);    
        }

        public ModelsManager Models { get; private set; }

        public ObjectsManager Objects { get; private set; }

        /// <summary>
        /// Populates the token
        /// </summary>
        /// <param name="clientSecrets"></param>
        /// <param name="apiTokens"> </param>
        internal void Populate(ObjectsApi api)
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
