namespace VVRestApi.Vault.Forms
{
    /// <summary>
    /// Manages form instances
    /// </summary>
    public class FormInstancesManager : VVRestApi.Common.BaseApi
    {
        internal FormInstancesManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

    }
}