namespace VVRestApi.Vault.Groups
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupsManager : VVRestApi.Common.BaseApi
    {
        internal GroupsManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }
    }
}