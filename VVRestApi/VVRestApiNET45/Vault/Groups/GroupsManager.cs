using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
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

        public List<NotifyUser> AddUserToGroup(Guid groupId, Guid usId)
        {
            dynamic postData = new ExpandoObject();

            return HttpHelper.PutListResult<NotifyUser>(VVRestApi.GlobalConfiguration.Routes.GroupsIdUsersId, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, groupId, usId);
        }

        public List<NotifyUser> AddUserToGroup(Guid groupId, List<Guid> usIds)
        {
            dynamic postData = new ExpandoObject();
            var sb = new StringBuilder();
            foreach (var usId in usIds)
            {
                if (sb.Length > 0) sb.Append(",");
                sb.Append(usId);
            }
            postData.userIds = sb.ToString();

            return HttpHelper.PutListResult<NotifyUser>(VVRestApi.GlobalConfiguration.Routes.GroupsIdUsers, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData, groupId);
        }

        public void RemoveGroupMember(Guid groupId, string userName)
        {
            var queryString = "userName=" + this.UrlEncode(userName);
            var result = HttpHelper.Delete(VVRestApi.GlobalConfiguration.Routes.GroupsIdUsers, queryString, GetUrlParts(), this.ApiTokens, groupId);
        }

        public void RemoveGroupMember(Guid groupId, Guid memberId)
        {
            var result = HttpHelper.Delete(VVRestApi.GlobalConfiguration.Routes.GroupsIdUsersId, "", GetUrlParts(), this.ApiTokens, groupId, memberId);
        }
    }
}