using System;
using System.Collections.Generic;
using System.Dynamic;
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

        public Group UpdateGroupName(Guid groupId, string groupName)
        {
            var postData = new List<KeyValuePair<string, string>>
            {       
                new KeyValuePair<string, string>("name", groupName),
            };

            return HttpHelper.Put<Group>(VVRestApi.GlobalConfiguration.Routes.GroupsId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, groupId);
        }

        public Group UpdateGroupDescription(Guid groupId, string groupDescription)
        {
            //var postData = new List<KeyValuePair<string, string>>
            //{       
            //    new KeyValuePair<string, string>("description", groupDescription),
            //};
            dynamic postData = new ExpandoObject();

            if (!String.IsNullOrWhiteSpace(groupDescription))
            {
                postData.description = groupDescription;
            }
            
            return HttpHelper.Put<Group>(VVRestApi.GlobalConfiguration.Routes.GroupsId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, groupId);

        }
    }
}