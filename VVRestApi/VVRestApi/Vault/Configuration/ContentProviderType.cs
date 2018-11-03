using System;

namespace VVRestApi.Vault.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum ContentProviderType
    {
        None = 0,
        VisualVaultProvider = 1,
        FileSystemProvider = 3,
        AwsS3Provider = 4
    }
}
