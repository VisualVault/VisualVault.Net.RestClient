using System.ComponentModel;
using Newtonsoft.Json;

namespace VVRestApi.Common.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiErrorMessage
    {
        /// <summary>
        ///     The error code that can be referenced against the help
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        ///     A more descriptive message for a developer
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "developerMessage")]
        public string DeveloperMessage { get; set; }

        /// <summary>
        ///     The end user message
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "moreInfo")]
        public string MoreInfo { get; set; }

        /// <summary>
        ///     A short reason for the error
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }

    }
}