using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Forms
{
    using System;
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class FormTemplatesManager : VVRestApi.Common.BaseApi
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
            return HttpHelper.Get<FormTemplate>(GlobalConfiguration.Routes.FormTemplates, string.Format("q=formTemplateName eq '{0}'", formTemplateName), options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
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
    }
}