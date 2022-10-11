using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Common;

namespace VVRestApi.Vault.Configuration
{
    /// <summary>
    /// Represents the configuration of the forms api for a customer database
    /// </summary>
    public class FormsApiConfig : RestObject
    {
        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "formsApiUrl")]
        public string FormsApiUrl { get; set; }
    }
}
