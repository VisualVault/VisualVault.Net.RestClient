using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using VVRestApi.Common;
using VVRestApi.Vault.Library;

namespace VVRestApi.Documents
{
    public class DocApiDocument : RestObject
    {
        /// <summary>
        /// revision id of the document
        /// </summary>
        [JsonProperty(PropertyName = "dhId")]
        public Guid Id { get; set; }

        /// <summary>
        /// documentId of the document
        /// </summary>
        [JsonProperty(PropertyName = "dlId")]
        public Guid DocumentId { get; set; }

        /// <summary>
        /// name of the document
        /// </summary>
        [JsonProperty(PropertyName = "dhDocId")]
        public string Name { get; set; }

        /// <summary>
        /// description of the document
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// current document state (Released/Unreleased)
        /// </summary>
        [JsonProperty(PropertyName = "dhState")]
        public DocumentState ReleaseState { get; set; }

        /// <summary>
        /// current checkin status (CheckedIn/CheckedOut)
        /// </summary>
        [JsonProperty(PropertyName = "dhStatus")]
        public CheckInStatus CheckinStatus { get; set; }

        /// <summary>
        /// revision of the document
        /// </summary>
        [JsonProperty(PropertyName = "displayRev")]
        public string Revision { get; set; }

        /// <summary>
        /// next review date of the document
        /// </summary>
        [JsonProperty(PropertyName = "dhReviewDate")]
        public DateTime ReviewDate { get; set; }

        /// <summary>
        /// expiration of the document
        /// </summary>
        [JsonProperty(PropertyName = "dhExpDate")]
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// archive state of the document
        /// </summary>
        [JsonProperty(PropertyName = "dhArchive")]
        public ArchiveType Archive { get; set; }

        /// <summary>
        /// abstract description of the document
        /// </summary>
        [JsonProperty(PropertyName = "dhAbstract")]
        public string Abstract { get; set; }

        /// <summary>
        /// keywords of the document
        /// </summary>
        [JsonProperty(PropertyName = "dhKeywords")]
        public string Keywords { get; set; }

        /// <summary>
        /// Id of the folder containing this Document
        /// </summary>
        [JsonProperty(PropertyName = "folderId")]
        public Guid FolderId { get; set; }

        /// <summary>
        /// name of the folder containing this Document
        /// </summary>
        [JsonProperty(PropertyName = "folderName")]
        public string FolderName { get; set; }

        /// <summary>
        /// folder path of the document
        /// </summary>
        [JsonProperty(PropertyName = "folderPath")]
        public string FolderPath { get; set; }

        /// <summary>
        /// Id of the file for this document
        /// </summary>
        [JsonProperty(PropertyName = "ddId")]
        public Guid FileId { get; set; }

        /// <summary>
        /// filename of the document's file
        /// </summary>
        [JsonProperty(PropertyName = "ddFileName")]
        public string Filename { get; set; }

        /// <summary>
        /// size of the document's file
        /// </summary>
        [JsonProperty(PropertyName = "ddFileSize")]
        public int FileSize { get; set; }

        /// <summary>
        /// extension name of the document's file
        /// </summary>
        [JsonProperty(PropertyName = "ddExtension")]
        public string Extension { get; set; }

        /// <summary>
        /// content type of the document
        /// </summary>
        [JsonProperty(PropertyName = "ddContentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// name of the user who checked in document
        /// </summary>
        [JsonProperty(PropertyName = "checkedInBy")]
        public string CheckedInBy { get; set; }

        /// <summary>
        /// Id of the user who checked out document
        /// </summary>
        [JsonProperty(PropertyName = "checkedOutUsId")]
        public Guid CheckedOutById { get; set; }

        /// <summary>
        /// name of the user who checked out document
        /// </summary>
        [JsonProperty(PropertyName = "checkedOutBy")]
        public string CheckedOutBy { get; set; }

        /// <summary>
        /// date the document was checked out
        /// </summary>
        [JsonProperty(PropertyName = "checkOutDate")]
        public DateTime? CheckOutDate { get; set; }

        /// <summary>
        /// describes the reason for the new revision of a document
        ///// </summary>
        //[JsonProperty(PropertyName = "changeText")]
        //public string ChangeText { get; set; }

        /// <summary>
        /// date the document was created
        /// </summary>
        [JsonProperty(PropertyName = "dhCreateDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Id of the user that created document
        /// </summary>
        [JsonProperty(PropertyName = "dhCreateBy")]
        public Guid CreateById { get; set; }

        /// <summary>
        /// name of the user who created document
        /// </summary>
        [JsonProperty(PropertyName = "createByDisplayName")]
        public string CreateBy { get; set; }

        /// <summary>
        /// date the document was modified
        /// </summary>
        [JsonProperty(PropertyName = "dhModifyDate")]
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// Id of the user that modified document
        /// </summary>
        [JsonProperty(PropertyName = "dhModifyBy")]
        public Guid ModifyById { get; set; }

        /// <summary>
        /// name of the user who modified document
        /// </summary>
        [JsonProperty(PropertyName = "modifyByDisplayName")]
        public string ModifyBy { get; set; }

        /// <summary>
        /// returns true if this document has been marked by user as a favorite
        /// </summary>
        [JsonProperty(PropertyName = "isFavorite")]
        public bool IsFavorite { get; set; }

        /// <summary>
        /// returns the number of times a document has been shared by a user
        /// </summary>
        [JsonProperty(PropertyName = "shareCount")]
        public int ShareCount { get; set; }

        /// <summary>
        /// True if the Document has been shared with the user
        /// </summary>
        [JsonProperty(PropertyName = "sharedWithMe")]
        public bool SharedWithMe { get; set; }

        /// <summary>
        /// returns the data values of the form instance fields
        /// </summary>
        public IEnumerable<IndexField> IndexFields { get; set; }
    }

    public class IndexField
    {
        public Guid DefinitionId { get; set; }
        public string Label { get; set; }
        public FolderIndexFieldType FieldType { get; set; }
        public int OrdinalPosition { get; set; }
        public bool Required { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }
    }
}
