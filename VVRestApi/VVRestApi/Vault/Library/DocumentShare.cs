using System;
using Newtonsoft.Json;
using VVRestApi.Common;

namespace VVRestApi.Vault.Library
{
    public class DocumentShare : RestObject
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "authKey")]
        public string AuthKey { get; set; }

        [JsonProperty(PropertyName = "documentId")]
        public Guid DocumentId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "shareLinkType")]
        public DocumentShareLinkType ShareLinkType { get; set; }
        
        [JsonProperty(PropertyName = "userDisplayName")]
        public string UserDisplayName { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public Guid UserId { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty(PropertyName = "linkRole")]
        public RoleType LinkRole { get; set; }

        [JsonProperty(PropertyName = "isPublicLink")]
        public bool IsPublicLink { get; set; }

        [JsonProperty(PropertyName = "linkEnabled")]
        public bool LinkEnabled { get; set; }

        [JsonProperty(PropertyName = "shareLink")]
        public string ShareLink { get; set; }

        [JsonProperty(PropertyName = "createBy")]
        public string CreateBy { get; set; }

        [JsonProperty(PropertyName = "createById")]
        public Guid CreateById { get; set; }

        [JsonProperty(PropertyName = "createDate")]
        public DateTime CreateDate { get; set; }
    }
}