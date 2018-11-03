using System;
using VVRestApi.Common;

namespace VVRestApi.Vault.Library
{
    public class DocumentApprovalRequest : RestObject
    {
        public int Id { get; set; }
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; }

        public string RequestTitle { get; set; }

        public Guid AssignedToId { get; set; }
        public string AssignedToName { get; set; }

        public Guid RequestedApproverId { get; set; }
        public string RequestedApproverName { get; set; }

        public bool PastDueEnabled { get; set; }
        public DateTime ApprovalDueByDate { get; set; }

        public DocumentApprovalRequestState ApprovalRequestStatus { get; set; }

        public DateTime ApprovedDate { get; set; }

        public DateTime CanceledDate { get; set; }
        public Guid CanceledById { get; set; }
        public string CanceledByName { get; set; }

        public string RejectComment { get; set; }
        public DateTime RejectedDate { get; set; }
        public Guid RejectedById { get; set; }
        public string RejectedByName { get; set; }

        public DateTime CreateDate { get; set; }
        public Guid CreateById { get; set; }
        public string CreateByName { get; set; }
    }
}