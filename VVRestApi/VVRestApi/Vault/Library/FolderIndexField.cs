using System;
using VVRestApi.Common;

namespace VVRestApi.Vault.Library
{
    public class FolderIndexField : RestObject
    {
        public Guid Id { get; set; }
        public int DefinitionPkey { get; set; }
        public Guid FolderId { get; set; }
        public FolderIndexFieldType FieldType { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public int OrdinalPosition { get; set; }
        public int FolderOrdinalPosition { get; set; }
        public bool Required { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid QueryId { get; set; }
        public string QueryValueField { get; set; }
        public string QueryDisplayField { get; set; }
        public Guid DropDownListId { get; set; }
        public string DefaultValue { get; set; }
        public bool GlobalRequired { get; set; }
        public Guid GlobalConnectionID { get; set; }
        public Guid GlobalQueryID { get; set; }
        public string GlobalQueryValueField { get; set; }
        public string GlobalQueryDisplayField { get; set; }
        public Guid GlobalDropDownListId { get; set; }
        public string GlobalDefaultValue { get; set; }
        public string CreateBy { get; set; }
        public Guid CreateById { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public Guid ModifyById { get; set; }
        public DateTime ModifyDate { get; set; }

    }
}