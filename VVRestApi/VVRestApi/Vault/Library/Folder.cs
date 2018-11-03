namespace VVRestApi.Vault.Library
{
    using System;
    using Newtonsoft.Json;
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class Folder : RestObject
    {
        /// <summary>
        ///     The Id of the folder
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; internal set; }

        /// <summary>
        ///     THe name of the folder
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     The last date the folder was modified
        /// </summary>
        [JsonProperty(PropertyName = "ModifyDate")]
        public string ModifyDate { get; set; }

        /// <summary>
        ///     The date the folder was created
        /// </summary>
        [JsonProperty(PropertyName = "CreateDate")]
        public string CreateDate { get; set; }

        /// <summary>
        ///     The ID of the user that created the folder
        /// </summary>
        [JsonProperty(PropertyName = "CreateById")]
        public Guid CreateById { get; set; }

        /// <summary>
        ///     The ID of the user that last modified the folder
        /// </summary>
        [JsonProperty(PropertyName = "ModifyById")]
        public Guid ModifyById { get; set; }
        
        /// <summary>
        ///     The ID of the top level folder, also known as the document library. If the ParentFolderId and TopLevelFolderId are the same, this is a root level folder.
        /// </summary>
        [JsonProperty(PropertyName = "TopLevelFolderId")]
        public Guid TopLevelFolderId { get; set; }

        /// <summary>
        ///     The ID of the parent folder. If the ParentFolderId and TopLevelFolderId are the same, this is a root level folder.
        /// </summary>
        [JsonProperty(PropertyName = "ParentFolderId")]
        public Guid ParentFolderId { get; set; }

        /// <summary>
        ///    The type of the folder. Store = 0, TLD = 1, Container = 2, [Obsolete]SubContainer = 3, Folder = 4, [Obsolete]Template = 5, NotAFolder = 6
        /// </summary>
        [JsonProperty(PropertyName = "FolderType")]
        public FolderTypes FolderType { get; set; }

        /// <summary>
        ///     The status of the folder. Active = 0, Deleted = 1
        /// </summary>
        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }

        /// <summary>
        ///     The path of the folder
        /// </summary>
        [JsonProperty(PropertyName = "FolderPath")]
        public string FolderPath { get; set; }
        
    }
}