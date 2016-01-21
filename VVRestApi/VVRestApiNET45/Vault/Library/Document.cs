using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VVRestApi.Common;

namespace VVRestApi.Vault.Library
{
    /// <summary>
    /// Document object
    /// </summary>
    public class Document : RestObject
    {
        /// <summary>
        /// revision id of the document
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        /// <summary>
        /// documentId of the document
        /// </summary>
        [JsonProperty(PropertyName = "documentId")]
        public Guid DocumentId { get; set; }

        /// <summary>
        /// name of the document
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// description of the document
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        
        /// <summary>
        /// current document state (Released/Unreleased)
        /// </summary>
        [JsonProperty(PropertyName = "releaseState")]
        public DocumentState ReleaseState { get; set; }
        
        /// <summary>
        /// current checkin status (CheckedIn/CheckedOut)
        /// </summary>
        [JsonProperty(PropertyName = "checkinStatus")]
        public CheckInStatus CheckinStatus { get; set; }
        
        /// <summary>
        /// revision of the document
        /// </summary>
        [JsonProperty(PropertyName = "revision")]
        public string Revision { get; set; }

        /// <summary>
        /// next review date of the document
        /// </summary>
        [JsonProperty(PropertyName = "reviewDate")]
        public DateTime ReviewDate { get; set; }

        /// <summary>
        /// expiration of the document
        /// </summary>
        [JsonProperty(PropertyName = "expireDate")]
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// archive state of the document
        /// </summary>
        [JsonProperty(PropertyName = "archive")]
        public ArchiveType Archive { get; set; }

        /// <summary>
        /// abstract description of the document
        /// </summary>
        [JsonProperty(PropertyName = "abstract")]
        public string Abstract { get; set; }

        /// <summary>
        /// keywords of the document
        /// </summary>
        [JsonProperty(PropertyName = "keywords")]
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
        [JsonProperty(PropertyName = "fileId")]
        public Guid FileId { get; set; }

        /// <summary>
        /// filename of the document's file
        /// </summary>
        [JsonProperty(PropertyName = "filename")]
        public string Filename { get; set; }

        /// <summary>
        /// size of the document's file
        /// </summary>
        [JsonProperty(PropertyName = "fileSize")]
        public int FileSize { get; set; }

        /// <summary>
        /// extension name of the document's file
        /// </summary>
        [JsonProperty(PropertyName = "extension")]
        public string Extension { get; set; }

        /// <summary>
        /// content type of the document
        /// </summary>
        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }
        
        /// <summary>
        /// name of the user who checked in document
        /// </summary>
        [JsonProperty(PropertyName = "checkedInBy")]
        public string CheckedInBy { get; set; }

        /// <summary>
        /// Id of the user who checked out document
        /// </summary>
        [JsonProperty(PropertyName = "checkedOutById")]
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
        public DateTime CheckOutDate { get; set; }
        
        /// <summary>
        /// date the document was created
        /// </summary>
        [JsonProperty(PropertyName = "createDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Id of the user that created document
        /// </summary>
        [JsonProperty(PropertyName = "createById")]
        public Guid CreateById { get; set; }

        /// <summary>
        /// name of the user who created document
        /// </summary>
        [JsonProperty(PropertyName = "createBy")]
        public string CreateBy { get; set; }
        
        /// <summary>
        /// date the document was modified
        /// </summary>
        [JsonProperty(PropertyName = "modifyDate")]
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// Id of the user that modified document
        /// </summary>
        [JsonProperty(PropertyName = "modifyById")]
        public Guid ModifyById { get; set; }

        /// <summary>
        /// name of the user who modified document
        /// </summary>
        [JsonProperty(PropertyName = "modifyBy")]
        public string ModifyBy { get; set; }

        /// <summary>
        /// returns true if this document has been marked by user as a favorite
        /// </summary>
        [JsonProperty(PropertyName = "isfavorite")]
        public bool IsFavorite { get; set; }
    }
}
