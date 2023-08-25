using System;
using VVRestApi.Common;
using VVRestApi.Vault.Library;

namespace VVRestApi.Documents
{
    public class DocumentOcrStatus : RestObject
    {
        public Guid Id { get; set; }

        public DocumentOcrCheckResult OcrStatus { get; set; }

        public IndexOcrType OcrType { get; set; }
    }
}
