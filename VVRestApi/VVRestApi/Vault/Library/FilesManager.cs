﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Extensions;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Library
{
    public class FilesManager : BaseApi
    {

        internal FilesManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        public Stream GetFileBySearch(RequestOptions options)
        {
            return HttpHelper.GetStream(GlobalConfiguration.Routes.Files, "", options, GetUrlParts(), this.ApiTokens, this.ClientSecrets);
        }

        public JObject UploadFile(Guid documentId, string filename, string revision, string changeReason, DocumentCheckInState checkInState, List<KeyValuePair<string, string>> indexFields, byte[] file)
        {
            if (documentId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "documentId");
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("filename is required but was empty", "filename");
            }
            var jobject = new JObject();
            foreach (var indexField in indexFields)
            {
                jobject.Add(new JProperty(indexField.Key, indexField.Value));
            }
            var jobjectString = JsonConvert.SerializeObject(jobject);

            var postData = new List<KeyValuePair<string, string>>
            {       
                new KeyValuePair<string, string>("documentId", documentId.ToString()),
                new KeyValuePair<string, string>("filename", filename),
                new KeyValuePair<string, string>("revision", revision),
                new KeyValuePair<string, string>("changeReason", changeReason),
                new KeyValuePair<string, string>("checkInDocumentState", checkInState.ToString()),
                new KeyValuePair<string, string>("indexFields", jobjectString)
            };

            return HttpHelper.PostMultiPart(GlobalConfiguration.Routes.Files, "", GetUrlParts(), this.ApiTokens, this.ClientSecrets, postData, filename, file);
        }

        public JObject UploadFile(Guid documentId, string fileName, string revision, string changeReason, DocumentCheckInState checkInState, List<KeyValuePair<string, string>> indexFields, Stream fileStream, JObject optionalParameters = null)
        {
            if (documentId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "documentId");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("FileName is required but was empty", "fileName");
            }

            var jobject = new JObject();
            foreach (var indexField in indexFields)
            {
                jobject.Add(new JProperty(indexField.Key, indexField.Value));
            }
            var jobjectString = JsonConvert.SerializeObject(jobject);

            var postData = new List<KeyValuePair<string, string>>
            {       
                new KeyValuePair<string, string>("documentId", documentId.ToString()),
                new KeyValuePair<string, string>("fileName", fileName),
                new KeyValuePair<string, string>("revision", revision),
                new KeyValuePair<string, string>("changeReason", changeReason),
                new KeyValuePair<string, string>("checkInDocumentState", checkInState.ToString()),
                new KeyValuePair<string, string>("indexFields", jobjectString)
            };

            if (optionalParameters != null)
            {
                postData.Add(new KeyValuePair<string, string>("parameters", optionalParameters.ToString()));
            }

            return HttpHelper.PostMultiPart(GlobalConfiguration.Routes.Files, "", GetUrlParts(), this.ApiTokens, this.ClientSecrets, postData, fileName, fileStream);
        }


        public JObject UploadFileByChunks(Guid documentId, string fileName, string revision, string changeReason, DocumentCheckInState checkInState, List<KeyValuePair<string, string>> indexFields, Stream fileStream, long chunkSize = 10485760)
        {
            JObject result = null;

            if (documentId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "documentId");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("FileName is required but was empty", "fileName");
            }

            var jobject = new JObject();
            foreach (var indexField in indexFields)
            {
                jobject.Add(new JProperty(indexField.Key, indexField.Value));
            }
            var jobjectString = JsonConvert.SerializeObject(jobject);

            var basePostData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("fileId", Guid.NewGuid().ToString()),
                new KeyValuePair<string, string>("documentId", documentId.ToString()),
                new KeyValuePair<string, string>("fileName", fileName),
                new KeyValuePair<string, string>("revision", revision),
                new KeyValuePair<string, string>("changeReason", changeReason),
                new KeyValuePair<string, string>("checkInDocumentState", checkInState.ToString()),
                new KeyValuePair<string, string>("indexFields", jobjectString)
            };

            //Calculate the chunk number based on chunk size
            var fileLength = fileStream.Length;
            var numberChunks = Math.Ceiling((double)fileLength / (double)chunkSize);
            for(int i = 0; i < numberChunks; i++)
            {
                var postData = new List<KeyValuePair<string, string>>(basePostData)
                {
                    new KeyValuePair<string, string>("chunkNumber", (i + 1).ToString()),
                    new KeyValuePair<string, string>("totalChunks", numberChunks.ToString())
                };

                var totalFileBytesRemaining = fileLength - fileStream.Position;
                var bufferSize = Math.Min(chunkSize, totalFileBytesRemaining);
                var buffer = new byte[bufferSize];
                var bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                using(var ms = new MemoryStream(buffer))
                {
                    result = HttpHelper.PostMultiPart(GlobalConfiguration.Routes.FilesChunk, "", GetUrlParts(), this.ApiTokens, this.ClientSecrets, postData, fileName, ms);

                    if(result == null || result.GetMetaData().StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        //If any of the chunk requests fail, stop sending request and return the result immediately
                        break;
                    }
                }
            }

            //Should return the final call's result
            return result;
        }

        /// <summary>
        /// Gets a file stream
        /// </summary>
        /// <param name="documentRevisionId"></param>
        /// <param name="options"> </param>
        /// <returns></returns>
        public Stream GetStream(Guid documentRevisionId, RequestOptions options = null)
        {
            return HttpHelper.GetStream(GlobalConfiguration.Routes.FilesId, "", options, GetUrlParts(), this.ApiTokens, this.ClientSecrets, documentRevisionId);
        }

        public JObject UploadZeroByteFile(Guid documentId, string fileName, long fileSize, byte[] file)
        {
            if (documentId.Equals(Guid.Empty))
            {
                throw new ArgumentException("DocumentId is required but was an empty Guid", "documentId");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("DocumentId is required but was empty", "fileName");
            }

            dynamic newFile = new ExpandoObject();
            newFile.documentId = documentId.ToString();
            newFile.fileName = fileName;
            newFile.allowNoFile = true;
            newFile.checkInDocumentState = DocumentCheckInState.Replace.ToString();
            newFile.fileLength = fileSize.ToString();
            newFile.revision = "1";
            newFile.changeReason = "Test zero byte file upload - Replace Revision";

            return HttpHelper.Post(GlobalConfiguration.Routes.FilesNoFileAllowed, "", GetUrlParts(), this.ApiTokens, newFile, fileName, file);
        }




    }
}
