// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormTemplate.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace VVRestApi.Vault.Forms
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;

    using Newtonsoft.Json;

    using VVRestApi.Common;

    public enum FormTemplateStatus
    {
        CheckedIn = 1,

        CheckedOut = 2
    }

    public class FormTemplate : RestObject
    {
        #region Public Properties

        /// <summary>
        ///     The name of the user that created the template
        /// </summary>
        [JsonProperty(PropertyName = "CreateBy")]
        public string CreateBy { get; set; }

        /// <summary>
        ///     ID of the user that created the template
        /// </summary>
        [JsonProperty(PropertyName = "CreateById")]
        public Guid CreateById { get; set; }

        /// <summary>
        ///     The date/time the template was created
        /// </summary>
        [JsonProperty(PropertyName = "CreateDate")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        ///     A description of the template
        /// </summary>
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        /// <summary>
        ///     Id of the form template
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; internal set; }

        /// <summary>
        ///     Name of the user that last modifed the template
        /// </summary>
        [JsonProperty(PropertyName = "ModifyBy")]
        public string ModifyBy { get; set; }

        /// <summary>
        ///     ID of the user that last modified the template
        /// </summary>
        [JsonProperty(PropertyName = "ModifyById")]
        public Guid ModifyById { get; set; }

        /// <summary>
        ///     The date/time the form template was last modified
        /// </summary>
        [JsonProperty(PropertyName = "modifyDate")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        ///     Name of the form template
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     The revision of the template
        /// </summary>
        [JsonProperty(PropertyName = "Revision")]
        public string Revision { get; set; }

        /// <summary>
        ///     Name of the form template
        /// </summary>
        [JsonProperty(PropertyName = "revisionId")]
        public Guid RevisionId { get; internal set; }

        /// <summary>
        ///     Name of the form template
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public FormTemplateStatus Status { get; set; }

        #endregion

        /// <summary>
        /// Gets the form field for the template. Use these field names when creating a new form instance.
        /// </summary>
        /// <returns></returns>
        public FormTemplateMeta GetFormFields()
        {
            return HttpHelper.Get<FormTemplateMeta>(GlobalConfiguration.Routes.FormTemplatesIdAction, string.Empty, true, string.Empty, this.CurrentToken, this.Id, "fields");

        }

        public FormInstance CreateNewFormInstance(ExpandoObject formInstance)
        {
            var result = HttpHelper.Post<FormInstance>(GlobalConfiguration.Routes.FormTemplatesIdAction, string.Empty, this.CurrentToken, formInstance, this.RevisionId, "forms");
            result.FormTemplateRevisionId = this.RevisionId;

            return result;
        }

        /// <summary>
        /// Gets the form data instances for the template
        /// </summary>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <param name="query">A query against the form template fields.</param>
        /// <returns></returns>
        public List<FormInstance> GetFormDataInstances(bool expand = false, string fields = "", string query = "")
        {
            var results = HttpHelper.GetListResult<FormInstance>(GlobalConfiguration.Routes.FormTemplatesIdAction, string.Empty, true, string.Empty, this.CurrentToken, this.Id, "forms");
            foreach (var result in results)
            {
                result.FormTemplateRevisionId = this.RevisionId;

            }
            return results;
        }

        /// <summary>
        /// Gets a form instance by ID if it exists and belongs to the form template
        /// </summary>
        /// <param name="revisionId">The revision ID of the form instance to return</param>
        /// <returns></returns>
        public FormInstance GetFormDataInstance(Guid revisionId)
        {
            var result = HttpHelper.Get<FormInstance>(GlobalConfiguration.Routes.FormTemplatesIdActionId, string.Empty, true, string.Empty, this.CurrentToken, this.Id, "forms", revisionId);
            result.FormTemplateRevisionId = this.RevisionId;

            return result;
        }

    }
}