using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VVRestApi.Common;

namespace VVRestApi.Vault.Version
{
    public class VaultVersionInfo : RestObject
    {

        public VaultVersionInfo()
        {
            ProgModifiedDate = DateTime.Now;
            ProgCreateDate = DateTime.Now;
            DBModifiedDate = DateTime.Now;
            DBCreateDate = DateTime.Now;
            DBVersion = "-1";
            ProgVersion = "1.0.0";
            UtcOffset = 0;
        }

        [JsonProperty(PropertyName = "DBCreateDate")]
        public DateTime DBCreateDate { get; set; }

        [JsonProperty(PropertyName = "DBModifiedDate")]
        public DateTime DBModifiedDate { get; set; }

        [JsonProperty(PropertyName = "DBVersion")]
        public string DBVersion { get; set; }

        [JsonProperty(PropertyName = "ProgCreateDate")]
        public DateTime ProgCreateDate { get; set; }
        
        [JsonProperty(PropertyName = "ProgModifiedDate")]
        public DateTime ProgModifiedDate { get; set; }

        [JsonProperty(PropertyName = "ProgVersion")]
        public string ProgVersion { get; set; }

        [JsonProperty(PropertyName = "UtcOffset")]
        public int UtcOffset { get; set; }
    }
}
