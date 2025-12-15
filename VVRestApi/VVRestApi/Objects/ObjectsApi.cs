using System;
using System.Collections.Generic;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Objects.DTO;
using VVRestApi.Objects.Models;
using VVRestApi.Vault;

namespace VVRestApi.Objects
{
    public class ObjectsApi : BaseApi
    {
        public bool IsEnabled { get; set; }
        public string BaseUrl { get; set; }

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
        }

        #region Objects

        public ObjectModel CreateObject(ObjectCreateRequest objectCreateRequest)
        {
            var result = HttpHelper.Post<ObjectModel>(GlobalConfiguration.RoutesObjectsApi.CreateObject, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, objectCreateRequest);
            return result.Meta.StatusCode == System.Net.HttpStatusCode.OK ? result : null;        
        }

        public ObjectModel GetObject(Guid id)
        {
            return HttpHelper.Get<ObjectModel>(GlobalConfiguration.RoutesObjectsApi.GetObject, string.Empty, null, GetUrlParts(), this.ClientSecrets, this.ApiTokens, false, id);
        }

        public ObjectModel UpdateObject(Guid id, ObjectUpdateRequest objectUpdateRequest)
        {
            var result = HttpHelper.Put<ObjectModel>(GlobalConfiguration.RoutesObjectsApi.UpdateObject, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, objectUpdateRequest, id);
            return result.Meta.StatusCode == System.Net.HttpStatusCode.OK ? result : null;
        }

        public ApiMetaData DeleteObject(Guid id)
        {
            return HttpHelper.DeleteReturnMeta(GlobalConfiguration.RoutesObjectsApi.DeleteObject, string.Empty, GetUrlParts(), this.ApiTokens, this.ClientSecrets, id);
        }

        public GetObjectsByModelIdResponse GetObjectsByModelId(Guid modelId, ObjectSearchRequest searchRequest, string q = "", bool mapPropertyNames = false)
        {
            var queryString = $"mapPropertyNames={mapPropertyNames}&q={q}";
            return HttpHelper.PostNoCustomerAlias<GetObjectsByModelIdResponse>(GlobalConfiguration.RoutesObjectsApi.GetObjectsByModelId, queryString, GetUrlParts(), this.ClientSecrets, this.ApiTokens, searchRequest, modelId);
        }

        #endregion

        #region Models

        public IEnumerable<Model> GetModelsAssignedToCustomerDatabase()
        {
            return HttpHelper.GetListResult<Model>(GlobalConfiguration.RoutesObjectsApi.GetModelsAssignedToCustomerDatabase, string.Empty, null, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        public Model GetModelAssignedToCustomerDatabase(Guid id)
        {
            return HttpHelper.Get<Model>(GlobalConfiguration.RoutesObjectsApi.GetModelAssignedToCustomerDatabase, string.Empty, null, GetUrlParts(), this.ClientSecrets, this.ApiTokens, false, id);
        }

        #endregion

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
