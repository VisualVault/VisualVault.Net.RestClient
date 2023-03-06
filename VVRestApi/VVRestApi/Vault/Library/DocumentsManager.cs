using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Library
{
    /// <summary>
    /// manages documents
    /// </summary>
    public class DocumentsManager : BaseApi
    {

        internal DocumentsManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        public List<Document> GetDocumentsBySearch(RequestOptions options)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.GetListResult<Document>(GlobalConfiguration.Routes.Documents, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        public Document CopyDocument(Guid dlId, Guid folderId)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "dlId");
            }
            if (folderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("FolderId is required but was an empty Guid", "folderId");
            }

            dynamic postData = new ExpandoObject();
            postData.folderId = folderId;

            return HttpHelper.Post<Document>(GlobalConfiguration.Routes.DocumentsIdCopy, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, dlId);
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
        public Document CreateDocument(Guid folderId, string name, string description, string revision, DocumentState documentState, List<KeyValuePair<string, string>> indexFields = null)
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

            if (indexFields != null)
            {
                var jobject = new JObject();

                foreach (var indexField in indexFields)
                {
                    jobject.Add(new JProperty(indexField.Key, indexField.Value));
                }

                var jobjectString = JsonConvert.SerializeObject(jobject);

                postData.indexFields = jobjectString;
            }

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
                throw new ArgumentException("Filename is required but was an empty string", "filename");
            }

            if (!(fileLength > 0))
            {
                throw new ArgumentException("FileLength must be an accurate file length and should be greater than 0", "fileLength");
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

        /// <summary>
        /// create a new document
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="revision"></param>
        /// <param name="documentState"></param>
        /// <returns></returns>
        public Document CreateDocumentWithFile(Guid folderId, string documentName, string description, string revision, DocumentState documentState, string fileName, long fileLength, Stream fileStream, List<KeyValuePair<string, string>> indexFields = null)
        {
            if (folderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("FolderId is required but was an empty Guid", "folderId");
            }

            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("folderId", folderId.ToString()),
                new KeyValuePair<string, string>("name", documentName),
                new KeyValuePair<string, string>("description", description),
                new KeyValuePair<string, string>("revision", revision),
                new KeyValuePair<string, string>("filename", fileName),
                new KeyValuePair<string, string>("fileLength", fileLength.ToString()),
                new KeyValuePair<string, string>("documentState", documentState.ToString())                
            };

            
            if (indexFields != null)
            {
                var jobject = new JObject();
                foreach (var indexField in indexFields)
                {
                    jobject.Add(new JProperty(indexField.Key, indexField.Value));
                }
                var jobjectString = JsonConvert.SerializeObject(jobject);

                postData.Add(new KeyValuePair<string, string>("indexFields", jobjectString));
            }
            
            var fileAttachments = new List<KeyValuePair<string, Stream>>
            {
                new KeyValuePair<string, Stream>(fileName, fileStream)
            };

            return HttpHelper.PostMultiPart<Document>(GlobalConfiguration.Routes.Documents, string.Empty, GetUrlParts(), this.ApiTokens, this.ClientSecrets, postData, fileAttachments);
        }

        public void DeleteDocument(Guid dhId)
        {
            HttpHelper.Delete(GlobalConfiguration.Routes.DocumentsId, "", GetUrlParts(), this.ApiTokens, this.ClientSecrets, dhId);
        }

        public bool DeleteDocument(Guid dhId, bool purgeDocument)
        {
            var success = false;

            var queryString = "purge=" + purgeDocument;

            var httpResult = HttpHelper.DeleteReturnMeta(GlobalConfiguration.Routes.DocumentsId, queryString, GetUrlParts(), this.ApiTokens, this.ClientSecrets, dhId);
            if (httpResult != null && httpResult.IsAffirmativeStatus())
            {
                success = true;
            }

            return success;
        }

        public List<DocumentIndexField> GetDocumentIndexFields(Guid dlId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.GetListResult<DocumentIndexField>(GlobalConfiguration.Routes.DocumentsIndexFields, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId);
        }

        public DocumentIndexField GetDocumentIndexField(Guid dlId, Guid dataId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.Get<DocumentIndexField>(GlobalConfiguration.Routes.DocumentsIndexFieldsId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId, dataId);
        }

        public Document GetDocument(Guid dlId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.Get<Document>(GlobalConfiguration.Routes.DocumentsId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId);
        }

        public List<Document> GetDocumentRevisions(Guid dlId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.GetListResult<Document>(GlobalConfiguration.Routes.DocumentsRevisions, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId);
        }

        public Document GetDocumentRevision(Guid dlId, Guid dhId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.Get<Document>(GlobalConfiguration.Routes.DocumentsRevisionsId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId, dhId);
        }

        public Document GetDocumentRevision(Guid dhId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.Get<Document>(GlobalConfiguration.Routes.DocumentsRevisionsChildIdOnly, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dhId);
        }

        public JObject GetDocumentOcrProperties(Guid dhId, RequestOptions options = null)
        {
            return HttpHelper.Get(GlobalConfiguration.Routes.DocumentsIdOcr, "", options, GetUrlParts(), this.ApiTokens, this.ClientSecrets, dhId);
        }


        public List<DocumentIndexField> GetDocumentRevisionIndexFields(Guid dlId, Guid dhId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.GetListResult<DocumentIndexField>(GlobalConfiguration.Routes.DocumentsRevisionsIdIndexFields, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId, dhId);
        }

        public DocumentIndexField GetDocumentRevisionIndexField(Guid dlId, Guid dhId, Guid fieldId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.Get<DocumentIndexField>(GlobalConfiguration.Routes.DocumentsRevisionsIdIndexFieldsId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, dlId, dhId, fieldId);
        }

        public DocumentIndexField UpdateIndexFieldValue(Guid dlId, Guid fieldId, string value)
        {
            dynamic postData = new ExpandoObject();

            postData.value = value;

            return HttpHelper.Put<DocumentIndexField>(GlobalConfiguration.Routes.DocumentsIndexFieldsId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, dlId, fieldId);
        }

        public List<DocumentIndexField> UpdateIndexFieldValues(Guid dlId, List<KeyValuePair<string, string>> indexFields)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "dlId");
            }

            dynamic postData = new ExpandoObject();

            var jobject = new JObject();
            foreach (var indexField in indexFields)
            {
                jobject.Add(new JProperty(indexField.Key, indexField.Value));
            }
            var jobjectString = JsonConvert.SerializeObject(jobject);


            postData.indexfields = jobjectString;

            return HttpHelper.PutListResult<DocumentIndexField>(GlobalConfiguration.Routes.DocumentsIndexFields, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, dlId);
        }

        public List<Document> GetDocumentFavorites(RequestOptions options = null)
        {
            return HttpHelper.GetListResult<Document>(GlobalConfiguration.Routes.DocumentsFavorites, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        public JObject UpdateDocumentOcrStatus(Guid dhId, OcrStatusType ocrStatusType, OcrErrorCodeType ocrErrorCode = OcrErrorCodeType.None)
        {
            if (dhId.Equals(Guid.Empty))
            {
                throw new ArgumentException("RevisionId is required but was an empty Guid", "dhId");
            }

            dynamic postData = new JObject();
            postData.ocrStatus = (int)ocrStatusType;
            postData.ocrErrorCode = (int)ocrErrorCode;

            return HttpHelper.Put(GlobalConfiguration.Routes.DocumentsIdOcrStatus, "", GetUrlParts(), this.ApiTokens, this.ClientSecrets, postData, dhId);
        }

        public Document SetDocumentAsFavorites(Guid dlId)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "dlId");
            }
            dynamic postData = new ExpandoObject();

            return HttpHelper.Put<Document>(GlobalConfiguration.Routes.DocumentsIdFavorites, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, dlId);
        }

        public void RemoveDocumentAsFavorites(Guid dlId)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "dlId");
            }

            var result = HttpHelper.Delete(GlobalConfiguration.Routes.DocumentsIdFavorites, "", GetUrlParts(), this.ApiTokens, this.ClientSecrets, dlId);
        }

        public Document MoveDocument(Guid dlId, Guid folderId)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "dlId");
            }
            if (folderId.Equals(Guid.Empty))
            {
                throw new ArgumentException("FolderId is required but was an empty Guid", "folderId");
            }

            dynamic postData = new ExpandoObject();
            postData.folderId = folderId;

            return HttpHelper.Put<Document>(GlobalConfiguration.Routes.DocumentsIdMove, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, dlId);
        }

        public Document RenameDocument(Guid dlId, string newName, bool renameAllRevisions = false)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "dlId");
            }
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("New Document name is required", "newName");
            }

            dynamic postData = new ExpandoObject();
            postData.newName = newName;
            postData.renameAllRevisions = renameAllRevisions;

            return HttpHelper.Put<Document>(GlobalConfiguration.Routes.DocumentsRename, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, dlId);
        }

        public JObject UpdateDocumentReleaseState(Guid dlId, Guid dhId, DocumentState documentState)
        {
            if (dhId.Equals(Guid.Empty))
            {
                throw new ArgumentException("RevisionId is required but was an empty Guid", "dhId");
            }

            dynamic postData = new JObject();
            postData.checkInDocumentState = (int)documentState;

            return HttpHelper.Put(GlobalConfiguration.Routes.DocumentsUpdateReleaseState, "", GetUrlParts(), this.ApiTokens, this.ClientSecrets, postData, dlId, dhId);
        }

        public JObject UpdateDocumentCheckInStatus(Guid dlId, CheckInStatus checkInStatus)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("Document Id is required but was an empty Guid", "dlId");
            }

            dynamic postData = new JObject();
            postData.checkIn = (int)checkInStatus;

            return HttpHelper.Put(GlobalConfiguration.Routes.DocumentsUpdateCheckInStatus, "", GetUrlParts(), this.ApiTokens, this.ClientSecrets, postData, dlId);
        }

        /// <summary>
        /// Updates a the metadata fields of a document's latest revision. Null parameters will be ignored in update.
        /// </summary>
        /// <param name="dlId">DocumentId</param>
        /// <param name="newName"></param>
        /// <param name="newRevision"></param>
        /// <param name="newDescription"></param>
        /// <param name="newKeywords"></param>
        /// <param name="newComments"></param>
        /// <param name="archiveType"></param>
        /// <param name="documentState"></param>
        /// <returns>Document with updated metadata</returns>
        public Document UpdateDocumentMetadata(Guid dlId, string newName = null, string newRevision = null, string newDescription = null, string newKeywords = null, string newComments = null, ArchiveType? archiveType = null, DocumentState? documentState = null)
        {
            if (dlId.Equals(Guid.Empty))
            {
                throw new ArgumentException("Document Id is required but was an empty Guid", "dlId");
            }

            dynamic putData = new JObject();
            if (newName != null)
                putData.name = newName;
            if (newRevision != null)
                putData.displayRev = newRevision;
            if (newDescription != null)
                putData.description = newDescription;
            if (newKeywords != null)
                putData.keywords = newKeywords;
            if (newComments != null)
                putData.comments = newComments;
            if (archiveType != null)
                putData.archive = (int)archiveType;
            if (documentState != null)
                putData.state = (int)documentState;

            return HttpHelper.Put<Document>(GlobalConfiguration.Routes.DocumentsId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, putData, dlId);
        }
    }
}
