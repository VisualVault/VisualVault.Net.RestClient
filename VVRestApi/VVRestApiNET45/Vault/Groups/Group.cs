// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Group.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Groups
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Net;
    using Newtonsoft.Json;
    using VVRestApi.Common;
    using VVRestApi.Common.Extensions;
    using VVRestApi.Common.Logging;
    using VVRestApi.Vault.Users;

    /// <summary>
    /// 
    /// </summary>
    public class Group : RestObject
    {
        #region Constructors and Destructors

        /// <summary>
        /// 
        /// </summary>
        public Group()
        {
            this.Description = string.Empty;
            this.Name = string.Empty;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// A description of the group
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        ///     The Id of the site
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; internal set; }

        /// <summary>
        /// The name of the group
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The description of the group
        /// </summary>
        [JsonProperty(PropertyName = "siteId")]
        public string SiteId { get; internal set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds a user to the group. The group must have the SiteId in order to do this operation
        /// </summary>
        /// <param name="userToAdd">User to add to the group</param>
        /// <returns></returns>
        public User AddUser(User userToAdd)
        {
            return AddUser(userToAdd.Id);
        }

        /// <summary>
        /// Adds a user to the group. The group must have the SiteId in order to do this operation
        /// </summary>
        /// <param name="idOfUser">ID of the user to add to the group</param>
        /// <returns></returns>
        public User AddUser(Guid idOfUser)
        {
            dynamic usersToAdd = new ExpandoObject();
            List<Guid> userIds = new List<Guid> { idOfUser };

            usersToAdd.UserIds = userIds;

            var result = HttpHelper.Post<User>(GlobalConfiguration.Routes.GroupsIdAction, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, usersToAdd, this.Id, "users");
            return result;
        }


        /// <summary>
        /// Adds multiple users to the group. The group must have the SiteId in order to do this operation
        /// </summary>
        /// <param name="userIds">List of User Ids to add to the group</param>
        /// <returns></returns>
        public List<User> AddUsers(List<Guid> userIds)
        {
            dynamic usersToAdd = new ExpandoObject();

            usersToAdd.UserIds = userIds.Distinct().ToList();

            var result = HttpHelper.PostListResult<User>(GlobalConfiguration.Routes.GroupsIdAction, string.Empty, GetUrlParts(), this.ClientSecrets, this.ApiTokens, usersToAdd, this.Id, "users");
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public bool IsInGroup(User currentUser)
        {
            return this.IsInGroup(currentUser.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOfUser"></param>
        /// <returns></returns>
        public bool IsInGroup(Guid idOfUser)
        {
            bool isInGroup = false;

            var result = HttpHelper.Get(GlobalConfiguration.Routes.GroupsIdActionId, string.Empty, null, GetUrlParts(), this.ApiTokens, this.Id, "users", idOfUser);

            if (result.IsHttpStatus(HttpStatusCode.OK))
            {
                isInGroup = true;
            }
            else if (result.IsHttpStatus(HttpStatusCode.NotFound))
            {
                isInGroup = false;
            }
            else
            {
                LogEventManager.Error("Failed to correctly determine if the user was in the group. There was an error on the server.");
            }

            return isInGroup;
        }
        #endregion
    }
}