namespace VVRestApi.Vault.Forms
{
    using System;
    using System.Collections.Generic;

    using VVRestApi.Common;

    public class FormTemplatesManager: VVRestApi.Common.BaseApi
    {
        internal FormTemplatesManager(VaultApi api)
        {
            base.Populate(api.CurrentToken);
        }

        /// <summary>
        /// Getes all the form templates the current user has access to
        /// </summary>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <param name="query">A query against the form template fields.</param>
        /// <returns></returns>
        public List<FormTemplate> GetFormTemplates(bool expand = false, string fields = "", string query = "")
        {
            return HttpHelper.GetListResult<FormTemplate>(GlobalConfiguration.Routes.FormTemplates, string.IsNullOrWhiteSpace(query) ? string.Empty : "q=" + query, expand, fields, this.CurrentToken);
        }

        /// <summary>
        /// Gets a form template that matches the given form template name
        /// </summary>
        /// <param name="formTemplateName">The name of the form template to get</param>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <returns></returns>
        public FormTemplate GetFormTemplate(string formTemplateName, bool expand = false, string fields = "")
        {
            return HttpHelper.Get<FormTemplate>(GlobalConfiguration.Routes.FormTemplates, string.Format("q=formTemplateName eq '{0}'", formTemplateName), expand, fields, this.CurrentToken);
        }

        /// <summary>
        /// Gets a form template that matches the given formTemplateId
        /// </summary>
        /// <param name="formTemplateId">The ID of the form template</param>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <returns></returns>
        public FormTemplate GetFormTemplate(Guid formTemplateId, bool expand, string fields = "")
        {
            return HttpHelper.Get<FormTemplate>(GlobalConfiguration.Routes.FormTemplatesId, string.Empty, expand, fields, this.CurrentToken, formTemplateId);
        }
    }
}