using Newtonsoft.Json;
using VVRestApi.Common;

namespace VVRestApi.Vault.Configuration
{
    public class ObjectsApiConfig : RestObject
    {
        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "apiUrl")]
        public string ObjectsApiUrl { get; set; }
    }
}
