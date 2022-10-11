using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Common;

namespace VVRestApi.Forms.FormInstances
{
    public class FormInstance : RestObject
    {
        /// <summary>
        /// Name of the form instance
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Revision ID of the form instance
        /// </summary>
        [JsonProperty(PropertyName = "formId")]
        public Guid FormId { get; set; }

        /// <summary>
        /// Revision ID of the form instance
        /// </summary>
        [JsonProperty(PropertyName = "revision")]
        public long Revision { get; set; }

        /// <summary>
        /// The page to redirect the user to upon successful saving of a form
        /// </summary>
        [JsonProperty(PropertyName = "confirmationPage")]
        public string ConfirmationPage { get; set; }
    }
}
