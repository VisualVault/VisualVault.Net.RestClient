using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            dynamic postData = new ExpandoObject();
            postData.folderId = folderId;

            if (!String.IsNullOrWhiteSpace(name))
            {
                postData.Name = name;
            }
            if (!String.IsNullOrWhiteSpace(description))
            {
                postData.Description = description;
            }
            if (!String.IsNullOrWhiteSpace(revision))
            {
                postData.Revision = revision;
            }
            postData.documentState = documentState;

            
            return HttpHelper.Post<Document>(GlobalConfiguration.Routes.Documents, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
        }

        /// <summary>
        /// Creates a new document, updates the indexFields for the new document and adds the zeroByte file = true post data attribute
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="revision"></param>
        /// <param name="filename"></param>
        /// <param name="fileLength"></param>
        /// <param name="documentState"></param>
        /// <param name="indexFields"></param>
        /// <returns></returns>
        public Document CreateDocumentWithEmptyFile(Guid folderId, string name, string description, string revision, string filename, int fileLength, DocumentState documentState, List<KeyValuePair<string, string>> indexFields)
        {
            if (folderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("FolderId is required but was an empty Guid", "folderId");
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Filename is required but was an empty Guid", "filename");
            }

            if (!(fileLength > 0))
            {
                throw new ArgumentException("FileLength must be an acurate file length and should be greater than 0", "fileLength");
            }

            dynamic postData = new ExpandoObject();

            postData.folderId = folderId;

            if (!String.IsNullOrWhiteSpace(name))
            {
                postData.Name = name;
            }
            if (!String.IsNullOrWhiteSpace(description))
            {
                postData.Description = description;
            }
            if (!String.IsNullOrWhiteSpace(revision))
            {
                postData.Revision = revision;
            }
            if (!String.IsNullOrWhiteSpace(filename))
            {
                postData.Filename = filename;
            }
            postData.FileLength = fileLength;
            postData.DocumentState = documentState;
            postData.AllowNoFile = true;

            var jobject = new JObject();
            foreach (var indexField in indexFields)
            {
                jobject.Add(new JProperty(indexField.Key, indexField.Value));
            }
            var jobjectString = JsonConvert.SerializeObject(jobject);


            postData.indexFields = jobjectString;


            return HttpHelper.Post<Document>(GlobalConfiguration.Routes.Documents, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
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
