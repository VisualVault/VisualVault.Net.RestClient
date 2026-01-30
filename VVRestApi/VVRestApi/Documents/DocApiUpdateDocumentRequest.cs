using System.Collections.Generic;
using Newtonsoft.Json;

namespace VVRestApi.Documents
{
    public class DocApiUpdateDocumentRequest
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "displayRev")]
        public string DisplayRev { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "abstract")]
        public string Abstract { get; set; }

        [JsonProperty(PropertyName = "changeText")]
        public string ChangeText { get; set; }

        [JsonProperty(PropertyName = "archive")]
        public string Archive { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "indexFields")]
        public Dictionary<string, string> IndexFields { get; set; }

        [JsonProperty(PropertyName = "docType")]
        public string DocType { get; set; }

        [JsonProperty(PropertyName = "confidence")]
        public decimal? Confidence { get; set; }
    }
}
