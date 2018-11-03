using System;
using VVRestApi.Common;

namespace VVRestApi.Vault.Library
{
    public class DocumentSignatureRequest : RestObject
    {
        public int Id { get; set; }
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string RequestTitle { get; set; }
        public Guid RequestedSignerId { get; set; }
        public string RequestedSignerName { get; set; }
        public bool PastDueEnabled { get; set; }
        public DateTime SignatureDueByDate { get; set; }
        public SignatureRequestStatus SignatureRequestStatus { get; set; }
        public DateTime SignedDate { get; set; }
        public DateTime CanceledDate { get; set; }
        public Guid CanceledById { get; set; }
        public string CanceledByName { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CreateById { get; set; }
    }
}