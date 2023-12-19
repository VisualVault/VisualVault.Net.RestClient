using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using VVRestApi.Common;

namespace VVRestApi.Vault.Configuration
{
    /// <summary>
    /// Represents the configuration of the studio api for a customer database
    /// </summary>
    public class StudioApiConfig : RestObject
    {
        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "studioApiUrl")]
        public string StudioApiUrl { get; set; }
    }
}
