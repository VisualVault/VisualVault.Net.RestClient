
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace VVRestApi.Vault.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class AwsS3Provider : IContentProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentProvider"></param>
        /// <exception cref="Exception"></exception>
        public AwsS3Provider(IContentProvider contentProvider)
        {
            try
            {
                if (contentProvider.ContentProviderType == ContentProviderType.AwsS3Provider)
                {
                    LoadData(contentProvider);
                }
                else
                {
                    throw new Exception("Content Provider Type must be AwsS3Provider");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Content Provider Type must be AwsS3Provider", ex);
            }
        }

        public AwsS3ProviderProperties AwsS3ProviderProperties { get; set; }
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

            JToken awsPath;
            this.ProviderInstance.TryGetValue("path", StringComparison.CurrentCultureIgnoreCase, out awsPath);

            JToken bucketName;
            this.ProviderInstance.TryGetValue("bucketName", StringComparison.CurrentCultureIgnoreCase, out bucketName);

            JToken accessKey;
            this.ProviderInstance.TryGetValue("accessKey", StringComparison.CurrentCultureIgnoreCase, out accessKey);

            JToken secretKey;
            this.ProviderInstance.TryGetValue("secretKey", StringComparison.CurrentCultureIgnoreCase, out secretKey);

            JToken awsRegionSystemName;
            this.ProviderInstance.TryGetValue("awsRegionSystemName", StringComparison.CurrentCultureIgnoreCase,
                out awsRegionSystemName);

            JToken serverSideEncryptionMethod;
            this.ProviderInstance.TryGetValue("serverSideEncryptionMethod", StringComparison.CurrentCultureIgnoreCase,
                out serverSideEncryptionMethod);

            JToken serverSideEncryptionKmsKey;
            this.ProviderInstance.TryGetValue("serverSideEncryptionKmsKey", StringComparison.CurrentCultureIgnoreCase,
                out serverSideEncryptionKmsKey);

            JToken useDefaultAwsKmsEncryptionKmsKey;
            this.ProviderInstance.TryGetValue("useDefaultAwsKmsEncryptionKmsKey",
                StringComparison.CurrentCultureIgnoreCase, out useDefaultAwsKmsEncryptionKmsKey);


            this.AwsS3ProviderProperties = new AwsS3ProviderProperties
            {
                Path = awsPath?.ToString() ?? "",
                BucketName = bucketName?.ToString() ?? "",
                AccessKey = accessKey?.ToString() ?? "",
                SecretKey = secretKey?.ToString() ?? "",
                AwsRegionSystemName = awsRegionSystemName?.ToString() ?? "",
                ServerSideEncryptionMethod = serverSideEncryptionMethod?.ToString() ?? "",
                ServerSideEncryptionKmsKey = serverSideEncryptionKmsKey?.ToString() ?? "",
                UseDefaultAwsKmsEncryptionKmsKey = bool.Parse(useDefaultAwsKmsEncryptionKmsKey?.ToString() ?? "false")
            };
        }
    }
}
