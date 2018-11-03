using System;
using Newtonsoft.Json;
using VVRestApi.Common;

namespace VVRestApi.Vault.Configuration
{
    public class VaultDbConnections : RestObject
    {
        [JsonProperty(PropertyName = "vaultConnection")]
        public string VaultConnection { get; set; }

        [JsonProperty(PropertyName = "vaultFormsConnection")]
        public string VaultFormsConnection { get; set; }
    }
}