using System.Collections.Generic;
using Newtonsoft.Json.Linq;
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
    public class FormInstance : RestObject
    {
        public HashSet<string> FormInstanceProperties = new HashSet<string>
            {
                "instancename",
                "revisionid",
                "modifydate",
                "modifybyid",
                "modifyby",
                "createdate",
                "createbyid",
                "createby",
                "href",
                "datatype"
            };

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

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public Guid FormTemplateRevisionId { get; set; }

        /// <summary>
        /// returns the data values of the form instance fields
        /// </summary>
        public List<KeyValuePair<string, string>> Fields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formInstance"></param>
        /// <returns></returns>
        public FormInstance CreateNewRevision(ExpandoObject formInstance)
        {
            return HttpHelper.Post<FormInstance>(GlobalConfiguration.Routes.FormTemplatesIdActionId, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, formInstance, this.FormTemplateRevisionId, "forms", this.RevisionId);
        }

        internal override void PopulateData(JToken data)
        {            
            Fields = new List<KeyValuePair<string, string>>();

            var jobject = data as JObject;
            if (jobject != null)
            {
                foreach (var dataProperty in jobject)
                {
                    if (!FormInstanceProperties.Contains(dataProperty.Key.ToLower()))
                    {
                        Fields.Add(new KeyValuePair<string, string>(dataProperty.Key, dataProperty.Value.ToString()));
                    }
                }
            }            
        }

        //private List<string> GetFormInstancePropertyNames()
        //{
        //    return new List<string>
        //    {
        //        "instanceName",
        //        "revisionId",
        //        "modifyDate",
        //        "modifyById",
        //        "createDate",
        //        "createById",
        //        "createBy"
        //    };
        //} 
    }
}