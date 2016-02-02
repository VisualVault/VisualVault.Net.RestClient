using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.DocumentViewer
{
    public class DocumentViewerManager : VVRestApi.Common.BaseApi
    {

        internal DocumentViewerManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }


        public List<AnnotationLayer> GetAnnotationLayers()
        {
            return HttpHelper.GetListResult<AnnotationLayer>(VVRestApi.GlobalConfiguration.Routes.DocumentViewerAnnotationsLayers, "", null, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        public List<AnnotationLayerAndPermission> GetAnnotationLayersWithPrivileges(Guid usId)
        {
            return HttpHelper.GetListResult<AnnotationLayerAndPermission>(VVRestApi.GlobalConfiguration.Routes.DocumentViewerAnnotationsLayersPermissionsId, "", null, GetUrlParts(), this.ClientSecrets, this.ApiTokens, usId);
        }


        public int GetUserAnnotationPermissions(Guid usId, string layerName, RequestOptions options = null)
        {
            var queryString = string.Format("layerName={0}", layerName);

            var result = HttpHelper.Get(GlobalConfiguration.Routes.DocumentViewerAnnotationsPermissionsId, queryString, options, GetUrlParts(), this.ApiTokens, usId);

            return result.Value<int>("data");
        }

        public byte[] GetAllDocumentAnnotations(Guid documentId)
        {
            var queryString = "";
            var result = HttpHelper.Get(GlobalConfiguration.Routes.DocumentViewerIdAnnotations, queryString, null, GetUrlParts(), this.ApiTokens, documentId);
            return result.Value<byte[]>("data");
        }

        //public byte[] GetlDocumentAnnotationsByLayer(Guid documentId, string layerName)
        //{
        //    var queryString = string.Format("layerName={0}", layerName);
        //    var result = HttpHelper.Get(GlobalConfiguration.Routes.DocumentViewerIdAnnotationsLayers, queryString, null, GetUrlParts(), this.ApiTokens, documentId);
        //    return result.Value<byte[]>("data");
        //}
                
        public byte[] GetDocumentAnnotationsByLayer(Guid documentId, string annotationLayerName)
        {
            var queryString = string.Format("layerName={0}", annotationLayerName);
            var result = HttpHelper.Get<DocumentViewerAnnotation>(GlobalConfiguration.Routes.DocumentViewerIdAnnotationsLayers, queryString, null, GetUrlParts(), this.ClientSecrets, this.ApiTokens, documentId);
            return result.Annotations;
            //var annotation = result as DocumentViewerAnnotation;

            //JToken tokenValue;
            //if (result.TryGetValue("data", StringComparison.OrdinalIgnoreCase, out tokenValue))
            //{
            //    var jobject = tokenValue as JObject;
            //    if (jobject != null)
            //    {
            //        return new byte[0];
            //    }
            //}

            //return result.Value<byte[]>("data");
        }

        public void SaveAnnotations(Guid documentId, Guid usId, string annotationLayerName, byte[] annotations)
        {
                        
            if (documentId.Equals(Guid.Empty))
            {
                throw new ArgumentException("documentId is required but was an empty Guid", "documentId");
            }

            dynamic postData = new ExpandoObject();
            postData.usId = usId;
            postData.layerName = annotationLayerName;
            postData.annotations = annotations;

            var result = HttpHelper.Post(GlobalConfiguration.Routes.DocumentViewerIdAnnotations, "", GetUrlParts(), this.ApiTokens, postData, documentId);
        }


        public byte[] GetBytes(string str)
        {
            var bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static DocumentViewerUrlResult GetDocumentViewerUrlFromLink(string link, ClientSecrets clientSecrets)
        {
            var queryString = string.Format("link={0}", WebUtility.UrlEncode(link));
            return HttpHelper.GetPublicNoCustomerAliases<DocumentViewerUrlResult>(GlobalConfiguration.Routes.Viewer, queryString, GetUrlParts(clientSecrets));
            //return result.Value<string>("data");
            //return result;
        }



        private static UrlParts GetUrlParts(ClientSecrets clientSecrets)
        {
            UrlParts urlParts = new UrlParts
            {
                ApiVersion = clientSecrets.ApiVersion,
                BaseUrl = clientSecrets.BaseUrl,
                CustomerAlias = clientSecrets.CustomerAlias,
                DatabaseAlias = clientSecrets.DatabaseAlias,
                OAuthTokenEndPoint = clientSecrets.OAuthTokenEndPoint
            };

            return urlParts;
        }

    }
}