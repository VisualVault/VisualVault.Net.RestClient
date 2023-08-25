using VVRestApi.Vault.Library;

namespace VVRestApi.Documents
{
    public class DocumentOcrStatusUpdateRequest
    {
        public OcrErrorCodeType OcrErrorCode { get; set; }

        public OcrStatusType OcrStatus { get; set; }

        public int PageCount { get; set; }

        public int WordCount { get; set; }
    }
}
