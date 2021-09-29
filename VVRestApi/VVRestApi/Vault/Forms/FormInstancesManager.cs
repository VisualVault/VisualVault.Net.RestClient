using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault.Library;

namespace VVRestApi.Vault.Forms
{
    /// <summary>
    /// Manages form instances
    /// </summary>
    public class FormInstancesManager : BaseApi
    {
        internal FormInstancesManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// returns the list of related documents to a form instance
        /// </summary>
        /// <returns></returns>
        public List<Document> GetDocumentsRelatedToFormInstance(Guid formInstanceId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.GetListResult<Document>(GlobalConfiguration.Routes.FormInstanceIdDocuments, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, formInstanceId);
        }

        public JObject RelateDocumentToFormInstance(Guid formInstanceId, Guid documentRevisionId, RequestOptions options = null)
        {
            dynamic postData = new ExpandoObject();
            var queryString = string.Format("relateToId={0}", UrlEncode(documentRevisionId.ToString()));

            return HttpHelper.PutResponse(GlobalConfiguration.Routes.FormInstanceIdRelateDocument, queryString, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, formInstanceId);
        }

        public List<FormInstance> GetFormInstancesRelatedToFormInstance(Guid formInstanceId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            return HttpHelper.GetListResult<FormInstance>(GlobalConfiguration.Routes.FormInstanceIdForms, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, formInstanceId);
        }

        public JObject RelateFormToFormInstance(Guid formInstanceId, Guid formInstanceId2, RequestOptions options = null)
        {
            dynamic postData = new ExpandoObject();
            var queryString = string.Format("relateToId={0}", UrlEncode(formInstanceId2.ToString()));

            return HttpHelper.PutResponse(GlobalConfiguration.Routes.FormInstanceIdRelateForm, queryString, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, formInstanceId);
        }

        public JObject RelateProjectToFormInstance(Guid formInstanceId, Guid projectId, RequestOptions options = null)
        {
            dynamic postData = new ExpandoObject();
            var queryString = string.Format("relateToId={0}", UrlEncode(projectId.ToString()));

            return HttpHelper.PutResponse(GlobalConfiguration.Routes.FormInstanceIdRelateProject, queryString, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, formInstanceId);
        }

        public JObject UnRelateDocumentToFormInstance(Guid formInstanceId, Guid docId, RequestOptions options = null)
        {
            dynamic postData = new ExpandoObject();
            var queryString = string.Format("relateToId={0}", UrlEncode(docId.ToString()));

            return HttpHelper.PutResponse(GlobalConfiguration.Routes.FormInstanceIdUnRelateDocument, queryString, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, formInstanceId);
        }

        public JObject UnRelateFormToFormInstance(Guid formInstanceId, Guid formId2, RequestOptions options = null)
        {
            dynamic postData = new ExpandoObject();
            var queryString = string.Format("relateToId={0}", UrlEncode(formId2.ToString()));

            return HttpHelper.PutResponse(GlobalConfiguration.Routes.FormInstanceIdUnRelateForm, queryString, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, formInstanceId);
        }

        public JObject DeleteFormInstance(Guid formInstanceId, RequestOptions options = null)
        {
            var queryString = string.Empty;

            return HttpHelper.Delete(GlobalConfiguration.Routes.FormInstanceIdDeleteForm, queryString, GetUrlParts(), this.ApiTokens, this.ClientSecrets, formInstanceId);
        }

        public JObject UnRelateProjectToFormInstance(Guid formInstanceId, Guid projectId, RequestOptions options = null)
        {
            dynamic postData = new ExpandoObject();
            var queryString = string.Format("relateToId={0}", UrlEncode(projectId.ToString()));

            return HttpHelper.PutResponse(GlobalConfiguration.Routes.FormInstanceIdUnRelateProject, queryString, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, formInstanceId);
        }

    }
}