using System.Collections.Generic;
using System.Dynamic;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Forms
{
    using System;
    using System.IO;
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class FormTemplatesManager : BaseApi
    {
        internal FormTemplatesManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// Getes all the form templates the current user has access to
        /// </summary>
        /// <param name="options"> </param>
        /// <returns></returns>
        public Page<FormTemplate> GetFormTemplates(RequestOptions options = null)
        {
            return HttpHelper.GetPagedResult<FormTemplate>(GlobalConfiguration.Routes.FormTemplates, string.Empty, options, this.GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        /// <summary>
        /// Gets a form template that matches the given form template name
        /// </summary>
        /// <param name="formTemplateName">The name of the form template to get</param>
        /// <param name="options"> </param>
        /// <returns></returns>
        public FormTemplate GetFormTemplate(string formTemplateName, RequestOptions options = null)
        {
            return HttpHelper.Get<FormTemplate>(GlobalConfiguration.Routes.FormTemplates, string.Format("q=[name] eq '{0}'", formTemplateName), options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        /// <summary>
        /// Gets a form template that matches the given formTemplateId
        /// </summary>
        /// <param name="formTemplateId">The ID of the form template</param>
        /// <param name="options"> </param>
        /// <returns></returns>
        public FormTemplate GetFormTemplate(Guid formTemplateId, RequestOptions options = null)
        {
            return HttpHelper.Get<FormTemplate>(GlobalConfiguration.Routes.FormTemplatesId, string.Empty, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, formTemplateId);
        }

        public FormTemplate ImportFormTemplate(Guid formTemplateId, Stream formTemplateXmlFile)
        {
            var fileAttachments = new List<KeyValuePair<string, Stream>>
            {
                new KeyValuePair<string, Stream>("file", formTemplateXmlFile)
            };

            return HttpHelper.PutMultiPart<FormTemplate>(GlobalConfiguration.Routes.FormTemplatesIdAction, string.Empty, GetUrlParts(), this.ApiTokens, this.ClientSecrets, null, fileAttachments, formTemplateId, "import");
        }
            


        /// <summary>
        /// 
        /// </summary>
        /// <param name="formTemplateId"></param>
        /// <param name="formInstanceId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public object GetFormInstanceData(Guid formTemplateId, Guid formInstanceId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            
            return HttpHelper.Get<FormInstance>(GlobalConfiguration.Routes.FormTemplatesFormsId, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, formTemplateId, formInstanceId.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formTemplateId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<FormInstance> GetFormInstanceData(Guid formTemplateId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.GetListResult<FormInstance>(GlobalConfiguration.Routes.FormTemplatesForms, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, formTemplateId);
        }


        public FormInstance CreateNewFormInstance(Guid formTemplateId, List<KeyValuePair<string, object>> fieldValues)
        {
            var postData = new ExpandoObject() as IDictionary<string, object>;
            foreach (var keyValuePair in fieldValues)
            {
                postData.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return HttpHelper.Post<FormInstance>(GlobalConfiguration.Routes.FormTemplatesIdForms, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, formTemplateId);
        }

        public FormInstance CreateNewFormInstanceRevision(Guid formTemplateId,Guid formInstanceRevisionId ,List<KeyValuePair<string, object>> fieldValues)
        {
            var postData = new ExpandoObject() as IDictionary<string, object>;
            foreach (var keyValuePair in fieldValues)
            {
                postData.Add(keyValuePair.Key, keyValuePair.Value);
            }

            //return HttpHelper.Post<FormInstance>(GlobalConfiguration.Routes.FormTemplatesIdForms, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, formTemplateId);
            return HttpHelper.Post<FormInstance>(GlobalConfiguration.Routes.FormTemplatesIdActionId, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, formTemplateId, "forms", formInstanceRevisionId);
        }

    }
}