using Newtonsoft.Json;
using VVRestApi.Common;

namespace VVRestApi.Vault.Library
{
    public class IndexFieldSelectOption : RestObject
    {
        [JsonProperty(PropertyName = "display")]
        public string Display { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}