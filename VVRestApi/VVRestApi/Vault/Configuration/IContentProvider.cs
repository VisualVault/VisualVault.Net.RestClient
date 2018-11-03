using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VVRestApi.Vault.Configuration
{
    public interface IContentProvider
    {
        [JsonProperty(PropertyName = "contentProviderType")]
        ContentProviderType ContentProviderType { get; set; }

        [JsonProperty(PropertyName = "id")]
        Guid Id { get; set; }

        [JsonProperty(PropertyName = "definitionId")]
        Guid DefinitionId { get; set; }

        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }

        [JsonProperty(PropertyName = "isDefault")]
        bool IsDefault { get; set; }

        [JsonProperty(PropertyName = "isDefaultArchivedProvider")]
        bool IsDefaultArchivedProvider { get; set; }

        [JsonProperty(PropertyName = "isDefaultDeletedProvider")]
        bool IsDefaultDeletedProvider { get; set; }

        [JsonProperty(PropertyName = "providerInstance")]
        JObject ProviderInstance { get; set; }
    }
}