using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Administration.Customers;

namespace VVRestApi.Vault.Library
{
    public class SignatureRequestManager : BaseApi
    {
        internal SignatureRequestManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        public DocumentSignatureRequest AddDocumentSignatureRequest(Guid dlId, List<Guid> usIdList, string requestTitle, string message)
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

            dynamic postData = new ExpandoObject();
            postData.users = jarray;
            postData.baseUrl = EmailShareUrl;
            postData.requestTitle = requestTitle;
            postData.message = message;

            return HttpHelper.Post<DocumentSignatureRequest>(GlobalConfiguration.Routes.DocumentsIdSignatures, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, dlId);

        }

        public List<DocumentSignatureRequest> GetSignatureRequestsForDocument(Guid dlId, RequestOptions options = null)
        {
            return HttpHelper.GetListResult<DocumentSignatureRequest>(GlobalConfiguration.Routes.DocumentsIdSignatures, "", options, GetUrlParts(), ClientSecrets, this.ApiTokens, dlId);
        }

        public DocumentSignatureRequest GetSignatureRequest(int signatureRequestId, RequestOptions options = null)
        {
            return HttpHelper.Get<DocumentSignatureRequest>(GlobalConfiguration.Routes.DocumentsSignaturesId, "", options, GetUrlParts(), ClientSecrets, this.ApiTokens, signatureRequestId);
        }

        public DocumentSignatureRequest SignDocumentSignatureRequest(int signatureRequestId)
        {
            dynamic postData = new ExpandoObject();
            postData.signDocument = true;

            return HttpHelper.Put<DocumentSignatureRequest>(GlobalConfiguration.Routes.DocumentsSignaturesId, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, signatureRequestId);
        }

        public DocumentSignatureRequest CancelDocumentSignatureRequest(int signatureRequestId)
        {
            dynamic postData = new ExpandoObject();
            postData.cancelRequest = true;

            return HttpHelper.Put<DocumentSignatureRequest>(GlobalConfiguration.Routes.DocumentsSignaturesId, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, signatureRequestId);
        }









    }
}