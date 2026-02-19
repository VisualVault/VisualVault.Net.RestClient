using System;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault;

namespace VVRestApi.Documents
{
    public class DocApi : BaseApi
    {
        public bool IsEnabled { get; set; }
        public string BaseUrl { get; set; }

        /// <summary>
        /// Creates a FormsApi instance
        /// </summary>
        /// <remarks>If token is not a JWT, the FormsApi object will be disabled</remarks>
        /// <param name="api"></param>
        /// <param name="jwt">Must be a JWT</param>
        internal DocApi(VaultApi api, Tokens jwt)
        {
            var docApiConfig = api.ConfigurationManager.GetDocApiConfiguration();


            if (docApiConfig == null || !jwt.IsJwt)
                return;// leave disabled

            IsEnabled = docApiConfig.IsEnabled;
            BaseUrl = docApiConfig.DocApiUrl;

            base.Populate(api.ClientSecrets, jwt);

        }

        public DocApiDocument GetRevision(Guid documentRevisionId)
        {
            return HttpHelper.Get<DocApiDocument>(GlobalConfiguration.RoutesDocApi.GetRevision, "", null, GetUrlParts(), this.ClientSecrets, this.ApiTokens, false, documentRevisionId);
        }

        public DocumentOcrStatus GetDocumentOcrStatus(Guid documentRevisionId)
        {
            return HttpHelper.Get<DocumentOcrStatus>(GlobalConfiguration.RoutesDocApi.OcrStatus, "", null, GetUrlParts(), this.ClientSecrets, this.ApiTokens, false, documentRevisionId);
        }

        public DocumentOcrStatusUpdateResponse updateDocumentOcrStatus(Guid documentRevisionId, DocumentOcrStatusUpdateRequest data)
        {
            var result = HttpHelper.PutNoCustomerAlias<DocumentOcrStatusUpdateResponse>(GlobalConfiguration.RoutesDocApi.OcrStatus, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, data, documentRevisionId);
            return result.Meta.StatusCode == System.Net.HttpStatusCode.OK ? result : null;
        }

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
