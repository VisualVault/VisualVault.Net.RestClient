namespace VVRestApi.Vault.Groups
{
    using System;

    using Newtonsoft.Json.Linq;

    public class GroupsManager: VVRestApi.Common.BaseApi
    {
        internal GroupsManager(VaultApi api)
        {
            base.Populate(api.CurrentToken);
        }
       
    }
}