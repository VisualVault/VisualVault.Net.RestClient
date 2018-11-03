using Newtonsoft.Json;
using VVRestApi.Common;

namespace VVRestApi.Vault.DocumentViewer
{
    public class DocumentViewerUrlResult : RestObject
    {
        [JsonProperty(PropertyName = "documentUrl")]
        public string DocumentUrl { get; set; }

        [JsonProperty(PropertyName = "documentViewerUrl")]
        public string DocumentViewerUrl { get; set; }

    }
}