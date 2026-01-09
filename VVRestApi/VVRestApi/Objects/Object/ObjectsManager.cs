using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Common.Messaging;
using VVRestApi.Objects.DTO;
using VVRestApi.Objects.Models;

namespace VVRestApi.Objects.Object
{
    public class ObjectsManager : ObjectsApi
    {
        protected ObjectsManager() { }

        public ObjectsManager(ObjectsApi api)
        {
            base.Populate(api);
        }

        public ObjectModel CreateObject(ObjectCreateRequest objectCreateRequest)
        {
            var result = HttpHelper.PostBaseUrl<ObjectModel>(GlobalConfiguration.RoutesObjectsApi.CreateObject, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, objectCreateRequest);
            return result.Meta.StatusCode == System.Net.HttpStatusCode.OK ? result : null;
        }

        public ObjectModel GetObject(Guid id, bool includeRelated = false)
        {
            var queryString = $"includeRelated={includeRelated}";
            return HttpHelper.GetBaseUrl<ObjectModel>(GlobalConfiguration.RoutesObjectsApi.GetObject, queryString, null, GetUrlParts(), this.ClientSecrets, this.ApiTokens, id);
        }

        public ObjectModel UpdateObject(Guid id, ObjectUpdateRequest objectUpdateRequest)
        {
            var result = HttpHelper.PutBaseUrl<ObjectModel>(GlobalConfiguration.RoutesObjectsApi.UpdateObject, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, objectUpdateRequest, id);
            return result.Meta.StatusCode == System.Net.HttpStatusCode.OK ? result : null;
        }

        public bool DeleteObject(Guid id)
        {
            var result = HttpHelper.DeleteBaseUrl(GlobalConfiguration.RoutesObjectsApi.DeleteObject, string.Empty, null, GetUrlParts(), this.ApiTokens, this.ClientSecrets, id);
            return result != null 
                && result.TryGetValue("data", out JToken data).Equals(true) 
                && data.Type.Equals(JTokenType.Boolean) 
                && (bool)data;
        }

        public GetObjectsByModelIdResponse GetObjectsByModelId(Guid modelId, ObjectSearchRequest searchRequest, string q = "", bool mapPropertyNames = false)
        {
            var queryString = $"mapPropertyNames={mapPropertyNames}&q={q}";
            return HttpHelper.PostBaseUrl<GetObjectsByModelIdResponse>(GlobalConfiguration.RoutesObjectsApi.GetObjectsByModelId, queryString, GetUrlParts(), this.ClientSecrets, this.ApiTokens, searchRequest, modelId);
        }
    }
}
