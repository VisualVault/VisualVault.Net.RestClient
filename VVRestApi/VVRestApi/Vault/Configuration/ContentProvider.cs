using System;
using Newtonsoft.Json.Linq;


namespace VVRestApi.Vault.Configuration
{
    public class ContentProvider : IContentProvider
    {
        public ContentProviderType ContentProviderType { get; set; }
        public Guid Id { get; set; }
        public Guid DefinitionId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDefaultArchivedProvider { get; set; }
        public bool IsDefaultDeletedProvider { get; set; }
        public JObject ProviderInstance { get; set; }
    }
}
