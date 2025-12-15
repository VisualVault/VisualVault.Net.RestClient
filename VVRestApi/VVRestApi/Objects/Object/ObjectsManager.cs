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
    }
}
