namespace VVRestApi.Vault.Forms
{
    using System;
    using System.Dynamic;

    using Newtonsoft.Json;

    using VVRestApi.Common;

    public class FormInstance : RestObject
    {
        /// <summary>
        /// Name of the form instance
        /// </summary>
        [JsonProperty(PropertyName = "InstanceName")]
        public string InstanceName { get; set; }

        /// <summary>
        /// Name of the form instance
        /// </summary>
        [JsonProperty(PropertyName = "RevisionId")]
        public Guid RevisionId { get; set; }

        /// <summary>
        /// Date the form instance was last modified
        /// </summary>
        [JsonProperty(PropertyName = "ModifyDate")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// ID of the user that last modified the form instance
        /// </summary>
        [JsonProperty(PropertyName = "ModifyById")]
        public Guid ModifyById { get; set; }

        /// <summary>
        /// Display name of the user that last modified the form instance
        /// </summary>
        [JsonProperty(PropertyName = "ModifyBy")]
        public string ModifyBy { get; set; }

        /// <summary>
        /// Date the form instance was created
        /// </summary>
        [JsonProperty(PropertyName = "CreateDate")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// ID of the user that created the form instance
        /// </summary>
        [JsonProperty(PropertyName = "CreateById")]
        public Guid CreateById { get; set; }

        /// <summary>
        /// Display name of the user that created the form instance
        /// </summary>
        [JsonProperty(PropertyName = "CreateBy")]
        public string CreateBy { get; set; }

        [JsonIgnore]
        public Guid FormTemplateRevisionId { get; set; }

        public FormInstance CreateNewRevision(ExpandoObject formInstance)
        {
            return HttpHelper.Post<FormInstance>(GlobalConfiguration.Routes.FormTemplatesIdActionId, string.Empty, this.CurrentToken, formInstance, this.FormTemplateRevisionId, "forms", this.RevisionId);
        }
    }
}