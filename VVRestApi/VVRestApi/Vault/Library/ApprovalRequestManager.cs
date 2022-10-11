using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Administration.Customers;

namespace VVRestApi.Vault.Library
{
    public class ApprovalRequestManager : BaseApi
    {
        internal ApprovalRequestManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        public DocumentApprovalRequest AddDocumentApprovalRequest(Guid dlId, List<Guid> usIdList, string requestTitle, string message)
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

            return HttpHelper.Post<DocumentApprovalRequest>(GlobalConfiguration.Routes.DocumentsIdApprovals, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, dlId);

        }

        public List<DocumentApprovalRequest> GetApprovalRequestsForDocument(Guid dlId, RequestOptions options = null)
        {
            return HttpHelper.GetListResult<DocumentApprovalRequest>(GlobalConfiguration.Routes.DocumentsIdApprovals, "", options, GetUrlParts(), ClientSecrets, this.ApiTokens, dlId);
        }

        public DocumentApprovalRequest GetApprovalRequest(int approvalRequestId, RequestOptions options = null)
        {
            return HttpHelper.Get<DocumentApprovalRequest>(GlobalConfiguration.Routes.DocumentsApprovalsId, "", options, GetUrlParts(), ClientSecrets, this.ApiTokens, approvalRequestId);
        }

        public DocumentApprovalRequest ApproveDocumentApprovalRequest(int approvalRequestId)
        {
            dynamic postData = new ExpandoObject();
            postData.approve = true;

            return HttpHelper.Put<DocumentApprovalRequest>(GlobalConfiguration.Routes.DocumentsApprovalsId, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, approvalRequestId);
        }

        public DocumentApprovalRequest CancelDocumentApprovalRequest(int approvalRequestId)
        {
            dynamic postData = new ExpandoObject();
            postData.cancel = true;

            return HttpHelper.Put<DocumentApprovalRequest>(GlobalConfiguration.Routes.DocumentsApprovalsId, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, approvalRequestId);
        }

        public DocumentApprovalRequest RejectDocumentApprovalRequest(int approvalRequestId, string comment)
        {
            dynamic postData = new ExpandoObject();
            postData.reject = true;
            postData.comment = comment;

            return HttpHelper.Put<DocumentApprovalRequest>(GlobalConfiguration.Routes.DocumentsApprovalsId, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, approvalRequestId);
        }

        public DocumentApprovalRequest ReassignDocumentApprovalRequest(int approvalRequestId, Guid usId)
        {
            dynamic postData = new ExpandoObject();
            postData.reassign = true;
            postData.user = usId.ToString();
            postData.baseUrl = EmailBaseUrl;

            return HttpHelper.Put<DocumentApprovalRequest>(GlobalConfiguration.Routes.DocumentsApprovalsId, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, approvalRequestId);
        }

        public DocumentApprovalRequest ResubmitDocumentApprovalRequest(int approvalRequestId)
        {
            dynamic postData = new ExpandoObject();
            postData.resubmit = true;

            return HttpHelper.Put<DocumentApprovalRequest>(GlobalConfiguration.Routes.DocumentsApprovalsId, "", GetUrlParts(), ClientSecrets, this.ApiTokens, postData, approvalRequestId);
        }





    }
}