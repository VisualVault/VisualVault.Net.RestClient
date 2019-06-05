// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormTemplate.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Forms
{
    using System;
    using System.Dynamic;
    using Newtonsoft.Json;
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public enum FormTemplateStatus
    {
        /// <summary>
        /// 
        /// </summary>
        CheckedIn = 1,

        /// <summary>
        /// 
        /// </summary>
        CheckedOut = 2
    }

    /// <summary>
    /// 
    /// </summary>
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
        public FormTemplateMeta GetFormFields(RequestOptions options = null)
        {
            return HttpHelper.Get<FormTemplateMeta>(GlobalConfiguration.Routes.FormTemplatesIdAction, string.Empty, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, this.Id, "fields");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formInstance"></param>
        /// <returns></returns>
        public FormInstance CreateNewFormInstance(ExpandoObject formInstance)
        {
            var result = HttpHelper.Post<FormInstance>(GlobalConfiguration.Routes.FormTemplatesIdAction, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, formInstance, this.RevisionId, "forms");
            result.FormTemplateRevisionId = this.RevisionId;

            return result;
        }

        /// <summary>
        /// Gets the form data instances for the template
        /// </summary>
        /// <returns></returns>
        public Page<FormInstance> GetFormDataInstances(RequestOptions options = null)
        {
            var results = HttpHelper.GetPagedResult<FormInstance>(GlobalConfiguration.Routes.FormTemplatesIdAction, string.Empty, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, this.Id, "forms");
            foreach (var result in results.Items)
            {
                result.FormTemplateRevisionId = this.RevisionId;

            }
            return results;
        }

        /// <summary>
        /// Gets a form instance by ID if it exists and belongs to the form template
        /// </summary>
        /// <param name="revisionId">The revision ID of the form instance to return</param>
        /// <param name="options"> </param>
        /// <returns></returns>
        public FormInstance GetFormDataInstance(Guid revisionId, RequestOptions options = null)
        {
            var result = HttpHelper.Get<FormInstance>(GlobalConfiguration.Routes.FormTemplatesIdActionId, string.Empty, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, this.Id, "forms", revisionId);
            result.FormTemplateRevisionId = this.RevisionId;

            return result;
        }

    }
}