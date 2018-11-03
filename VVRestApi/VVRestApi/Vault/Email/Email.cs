using Newtonsoft.Json;
using VVRestApi.Common;

namespace VVRestApi.Vault.Email
{
    /// <summary>
    /// 
    /// </summary>
    public class Email : RestObject
    {        
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "recipients")]
        public string Recipients { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "ccrecipients")]
        public string CcRecipients { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "hasattachments")]
        public bool HasAttachments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Email()
        {
            Recipients = string.Empty;
            CcRecipients = string.Empty;
            Subject = string.Empty;
            Body = string.Empty;
            HasAttachments = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="ccRecipients"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="hasAttachments"></param>
        public Email(string recipients, string ccRecipients, string subject, string body, bool hasAttachments)
        {
            Recipients = recipients;
            CcRecipients = ccRecipients;
            Subject = subject;
            Body = body;
            HasAttachments = hasAttachments;
        }        
    }
}
