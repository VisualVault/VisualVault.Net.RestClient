using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Library
{
    public class DocumentShareManager : VVRestApi.Common.BaseApi
    {
        internal DocumentShareManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        public List<Document> GetDocumentsSharedWithMe(RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.GetListResult<Document>(VVRestApi.GlobalConfiguration.Routes.DocumentsShares, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        public List<DocumentShare> GetListOfUsersDocumentSharedWith(Guid dlId, RequestOptions options = null)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("dlId is required but was an empty Guid", "dlId");
            }

            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.GetListResult<DocumentShare>(VVRestApi.GlobalConfiguration.Routes.DocumentsIdShares, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId);
        }

        public DocumentShare ShareDocument(Guid dlId, Guid usId)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("dlId is required but was an empty Guid", "dlId");
            }
            if (usId.Equals(Guid.Empty))
            {
                throw new ArgumentException("dlId is required but was an empty Guid", "usId");
            }
            dynamic postData = new ExpandoObject();
            postData.users = usId;

            return HttpHelper.Put<DocumentShare>(VVRestApi.GlobalConfiguration.Routes.DocumentsIdShares, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, dlId, usId);
        }

        public List<DocumentShare> ShareDocument(Guid dlId, List<Guid> usIdList)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("dlId is required but was an empty Guid", "dlId");
            }

            var jarray = new JArray();
            foreach (var usId in usIdList)
            {
                jarray.Add(new JObject(new JProperty("id", usId)));
            }
            var jobjectString = JsonConvert.SerializeObject(jarray);

            dynamic postData = new ExpandoObject();
            postData.users = jobjectString;

            return HttpHelper.PutListResult<DocumentShare>(VVRestApi.GlobalConfiguration.Routes.DocumentsIdShares, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, dlId);
        }

        public void RemoveUserFromSharedDocument(Guid dlId, Guid usId)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("dlId is required but was an empty Guid", "dlId");
            }
            if (usId.Equals(Guid.Empty))
            {
                throw new ArgumentException("dlId is required but was an empty Guid", "usId");
            }

            HttpHelper.DeleteReturnMeta(VVRestApi.GlobalConfiguration.Routes.DocumentsIdShares, UrlEncode("usid=" + usId.ToString()), GetUrlParts(), this.ApiTokens, dlId);
        }
    }
}