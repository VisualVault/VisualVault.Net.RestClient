using System.Collections.Generic;
using Newtonsoft.Json;

namespace VVRestApi.Documents
{
    public class DocApiCreateDocumentRequest
    {
        [JsonProperty(PropertyName = "folderId")]
        public string FolderId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "revision")]
        public string Revision { get; set; }

        [JsonProperty(PropertyName = "documentState")]
        public string DocumentState { get; set; }

        [JsonProperty(PropertyName = "checkInDocumentState")]
        public string CheckInDocumentState { get; set; }

        [JsonProperty(PropertyName = "filename")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "fileLength")]
        public long? FileLength { get; set; }

        [JsonProperty(PropertyName = "fileBytes")]
        public byte[] FileBytes { get; set; }

        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }

        [JsonProperty(PropertyName = "indexFields")]
        public Dictionary<string, string> IndexFields { get; set; }

        [JsonProperty(PropertyName = "docType")]
        public string DocType { get; set; }

        [JsonProperty(PropertyName = "confidence")]
        public decimal? Confidence { get; set; }

        [JsonProperty(PropertyName = "keywords")]
        public string Keywords { get; set; }

        [JsonProperty(PropertyName = "abstract")]
        public string Abstract { get; set; }

        [JsonProperty(PropertyName = "changeText")]
        public string ChangeText { get; set; }
    }
}
