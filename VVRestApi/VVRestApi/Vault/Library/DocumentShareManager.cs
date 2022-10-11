using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Library
{
    public class DocumentShareManager : BaseApi
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
            return HttpHelper.GetListResult<Document>(GlobalConfiguration.Routes.DocumentsShares, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
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
            return HttpHelper.GetListResult<DocumentShare>(GlobalConfiguration.Routes.DocumentsIdShares, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId);
        }

        public DocumentShare ShareDocument(Guid dlId, Guid usId, string message = "", RoleType linkRole = RoleType.Viewer)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("dlId is required but was an empty Guid", "dlId");
            }
            if (usId.Equals(Guid.Empty))
            {
                throw new ArgumentException("dlId is required but was an empty Guid", "usId");
            }

            switch (linkRole)
            {
                case RoleType.Owner:
                case RoleType.Editor:
                    linkRole = RoleType.Editor;
                    break;
                default:
                    linkRole = RoleType.Viewer;
                    break;
            }

            dynamic postData = new ExpandoObject();
            postData.users = new JObject(new JProperty("id", usId));
            postData.message = message;
            postData.baseUrl = EmailShareUrl;
            postData.isPublic = "true";
            postData.linkRole = linkRole.ToString().ToLower();

            return HttpHelper.Put<DocumentShare>(GlobalConfiguration.Routes.DocumentsIdShares, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, dlId, usId);
        }

        public List<DocumentShare> ShareDocument(Guid dlId, List<Guid> usIdList, string message = "", RoleType linkRole = RoleType.Viewer)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("dlId is required but was an empty Guid", "dlId");
            }

            switch (linkRole)
            {
                case RoleType.Owner:
                case RoleType.Editor:
                    linkRole = RoleType.Editor;
                    break;
                default:
                    linkRole = RoleType.Viewer;
                    break;
            }

            var jarray = new JArray();
            foreach (var usId in usIdList)
            {
                jarray.Add(new JObject(new JProperty("id", usId)));
            }

            dynamic postData = new ExpandoObject();
            postData.users = jarray;
            postData.message = message;
            postData.baseUrl = EmailShareUrl;
            postData.isPublic = "true";
            postData.linkRole = linkRole.ToString().ToLower();

            return HttpHelper.PutListResult<DocumentShare>(GlobalConfiguration.Routes.DocumentsIdShares, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, dlId);
        }

        /// <summary>
        /// Creates a link the can be embedded in a web page
        /// </summary>
        public DocumentShare GetDocumentShareLink(Guid dlId, RoleType linkRole)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("dlId is required but was an empty Guid", "dlId");
            }

            switch (linkRole)
            {
                case RoleType.Owner:
                case RoleType.Editor:
                    linkRole = RoleType.Editor;
                    break;
                default:
                    linkRole = RoleType.Viewer;
                    break;
            }

            dynamic postData = new ExpandoObject();
            postData.baseUrl = EmailShareUrl;
            postData.linkRole = linkRole.ToString().ToLower();

            return HttpHelper.Post<DocumentShare>(GlobalConfiguration.Routes.DocumentsIdShares, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, dlId);
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

            HttpHelper.DeleteReturnMeta(GlobalConfiguration.Routes.DocumentsIdShares, UrlEncode("usid=" + usId.ToString()), GetUrlParts(), this.ApiTokens, this.ClientSecrets, dlId);
        }
    }
}