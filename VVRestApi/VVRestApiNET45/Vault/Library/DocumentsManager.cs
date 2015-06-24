using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Library
{
    /// <summary>
    /// manages documents
    /// </summary>
    public class DocumentsManager : VVRestApi.Common.BaseApi
    {

        internal DocumentsManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// create a new document
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="revision"></param>
        /// <param name="documentState"></param>
        /// <returns></returns>
        public Document CreateDocument(Guid folderId, string name, string description, string revision, DocumentState documentState)
        {
            if (folderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("FolderId is required but was an empty Guid", "folderId");
            }

            dynamic newDocument = new ExpandoObject();
            newDocument.folderId = folderId;

            if (!String.IsNullOrWhiteSpace(name))
            {
                newDocument.Name = name;
            }
            if (!String.IsNullOrWhiteSpace(description))
            {
                newDocument.Description = description;
            }
            if (!String.IsNullOrWhiteSpace(revision))
            {
                newDocument.Revision = revision;
            }
            newDocument.documentState = documentState;

            return HttpHelper.Post<Document>(GlobalConfiguration.Routes.Documents, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, newDocument);
        }

        public List<DocumentIndexField> GetDocumentIndexFields(Guid dlId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            
            return HttpHelper.GetListResult<DocumentIndexField>(VVRestApi.GlobalConfiguration.Routes.DocumentsIndexFields, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId);
        }

        public DocumentIndexField GetDocumentIndexField(Guid dlId, Guid dataId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.Get<DocumentIndexField>(VVRestApi.GlobalConfiguration.Routes.DocumentsIndexFieldsId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId, dataId);
        }

        public Document GetDocument(Guid dlId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.Get<Document>(VVRestApi.GlobalConfiguration.Routes.DocumentsId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId);
        }

        public List<Document> GetDocumentRevisions(Guid dlId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.GetListResult<Document>(VVRestApi.GlobalConfiguration.Routes.DocumentsRevisions, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId);
        }

        public Document GetDocumentRevision(Guid dlId,Guid dhId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.Get<Document>(VVRestApi.GlobalConfiguration.Routes.DocumentsRevisionsId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId, dhId);
        }

        public List<DocumentIndexField> GetDocumentRevisionIndexFields(Guid dlId, Guid dhId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.GetListResult<DocumentIndexField>(VVRestApi.GlobalConfiguration.Routes.DocumentsRevisionsIdIndexFields, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId, dhId);
        }

        public DocumentIndexField GetDocumentRevisionIndexField(Guid dlId, Guid dhId, Guid dataId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.Get<DocumentIndexField>(VVRestApi.GlobalConfiguration.Routes.DocumentsRevisionsIdIndexFieldsId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId, dhId, dataId);
        }
    }
}
