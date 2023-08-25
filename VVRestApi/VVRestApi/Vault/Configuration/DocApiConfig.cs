using Newtonsoft.Json;
using VVRestApi.Common;

namespace VVRestApi.Vault.Configuration
{
    public class DocApiConfig : RestObject
    {
        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "apiUrl")]
        public string DocApiUrl { get; set; }
    }
}
