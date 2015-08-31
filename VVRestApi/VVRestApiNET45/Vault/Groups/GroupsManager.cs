using System;
using System.Collections.Generic;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault.Users;

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

        public List<User> GetGroupMembers(Guid groupId, RequestOptions options = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            return HttpHelper.GetListResult<User>(VVRestApi.GlobalConfiguration.Routes.GroupsIdUsers, "", options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, groupId);
        }
    }
}