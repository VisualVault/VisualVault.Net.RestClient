using System;
using VVRestApi.Common;

namespace VVRestApi.Vault.Library
{
    public class DocumentIndexField : RestObject
    {
        public Guid Id { get; set; }
        public Guid DhId { get; set; }
        public Guid FieldId { get; set; }
        public FolderIndexFieldType FieldType { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public int OrdinalPosition { get; set; }
        public bool Required { get; set; }
        public string Units { get; set; }
        public string Value { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Guid CreateById { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public Guid ModifyById { get; set; }

    }
}