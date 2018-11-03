
using System;
using Newtonsoft.Json.Linq;

namespace VVRestApi.Vault.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class FileSystemProvider : IContentProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentProvider"></param>
        /// <exception cref="Exception"></exception>
        public FileSystemProvider(IContentProvider contentProvider)
        {
            try
            {
                if (contentProvider.ContentProviderType == ContentProviderType.FileSystemProvider)
                {
                    LoadData(contentProvider);
                }
                else
                {
                    throw new Exception("Content Provider Type must be FileSystemProvider");
                }
            }
            catch (Exception ex)
            {
               throw new Exception("Content Provider Type must be FileSystemProvider", ex); 
            }
        }

        public FileSystemProviderProperties FileSystemProviderProperties { get; set; }
        public ContentProviderType ContentProviderType { get; set; }
        public Guid Id { get; set; }
        public Guid DefinitionId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDefaultArchivedProvider { get; set; }
        public bool IsDefaultDeletedProvider { get; set; }
        public JObject ProviderInstance { get; set; }

        private void LoadData(IContentProvider provider)
        {
            this.ContentProviderType = provider.ContentProviderType;
            this.Id = provider.Id;
            this.DefinitionId = provider.DefinitionId;
            this.Name = provider.Name;
            this.IsDefault = provider.IsDefault;
            this.IsDefaultArchivedProvider = provider.IsDefaultArchivedProvider;
            this.IsDefaultDeletedProvider = provider.IsDefaultDeletedProvider;
            this.ProviderInstance = provider.ProviderInstance;

            JToken uncPath;
            this.ProviderInstance.TryGetValue("path", StringComparison.CurrentCultureIgnoreCase, out uncPath);

            JToken domainName;
            this.ProviderInstance.TryGetValue("domainName", StringComparison.CurrentCultureIgnoreCase, out domainName);

            JToken userName;
            this.ProviderInstance.TryGetValue("userName", StringComparison.CurrentCultureIgnoreCase, out userName);

            JToken password;
            this.ProviderInstance.TryGetValue("password", StringComparison.CurrentCultureIgnoreCase, out password);

            JToken maxFolderDepth;
            this.ProviderInstance.TryGetValue("maxFolderDepth", StringComparison.CurrentCultureIgnoreCase,out maxFolderDepth);

            JToken maxSubFolders;
            this.ProviderInstance.TryGetValue("maxSubFolders", StringComparison.CurrentCultureIgnoreCase,out maxSubFolders);

            JToken maxFileCount;
            this.ProviderInstance.TryGetValue("maxFileCount", StringComparison.CurrentCultureIgnoreCase,out maxFileCount);

            this.FileSystemProviderProperties = new FileSystemProviderProperties
            {
                Path = uncPath?.ToString() ?? "",
                DomainName = domainName?.ToString() ?? "",
                UserName = userName?.ToString() ?? "",
                Password = password?.ToString() ?? "",
                MaxFolderDepth = int.Parse(maxFolderDepth?.ToString() ?? "2000"),
                MaxSubFolders = int.Parse(maxSubFolders?.ToString() ?? "2000"),
                MaxFileCount = int.Parse(maxFileCount?.ToString() ?? "2000"),
            };

        }
    }
}
