using System;
using Newtonsoft.Json;
using VVRestApi.Common;

namespace VVRestApi.Vault.Library
{
    public class DocumentShare : RestObject
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "documentId")]
        public Guid DocumentId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public Guid UserId { get; set; }

        [JsonProperty(PropertyName = "securityRole")]
        public RoleType SecurityRole { get; set; }

        [JsonProperty(PropertyName = "createBy")]
        public string CreateBy { get; set; }

        [JsonProperty(PropertyName = "createById")]
        public Guid CreateById { get; set; }

        [JsonProperty(PropertyName = "createDate")]
        public DateTime CreateDate { get; set; }
    }
}