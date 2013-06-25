// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Site.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApi.Vault.Sites
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using VVRestApi.Common;
    using VVRestApi.Vault.Groups;
    using VVRestApi.Vault.Users;

    public class Site : RestObject
    {
        public Site()
        {
            this.Description = string.Empty;
            this.Name = string.Empty;
            this.SiteType = string.Empty;
        }

        #region Public Properties

        /// <summary>
        ///     A description of the site
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        ///     The Id of the site
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; internal set; }

        /// <summary>
        ///     THe name of the site
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     The type of the site.
        /// </summary>
        [JsonProperty(PropertyName = "siteType")]
        public string SiteType { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates a new group in the site
        /// </summary>
        /// <param name="groupName">The name of the group to create</param>
        /// <param name="description">A description of the group</param>
        /// <returns></returns>
        public Group CreateGroup(string groupName, string description = "")
        {
            if (String.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentNullException("groupName", "groupName is required");
            }

            dynamic newGroup = new ExpandoObject();
            newGroup.Name = groupName;
            if (!String.IsNullOrWhiteSpace(description))
            {
                newGroup.Description = description;
            }
            newGroup.SiteId = Id;

            return HttpHelper.Post<Group>(GlobalConfiguration.Routes.Groups, string.Empty, this.CurrentToken, newGroup);
        }


        /// <summary>
        /// Creates a new user in the site. The user must be unique across the entire instance of VisualVault.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="middleInitial"></param>
        /// <param name="lastName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="passwordExpireDate">If null, the password will never expire</param>
        /// <returns></returns>
        public User CreateUser(string username, string password, string firstName, string middleInitial, string lastName, string emailAddress, DateTime? passwordExpireDate = null)
        {
            dynamic newUser = new ExpandoObject();
            if (passwordExpireDate.HasValue)
            {
                newUser.passwordExpires = passwordExpireDate.ToString();
                newUser.passwordNeverExpires = false;
            }
            else
            {
                newUser.passwordNeverExpires = true;
            }

            newUser.userId = username;
            newUser.password = password;
            newUser.firstName = firstName;
            newUser.middleInitial = middleInitial;
            newUser.lastName = lastName;
            newUser.emailAddress = emailAddress;

            return HttpHelper.Post<User>(GlobalConfiguration.Routes.SitesIdAction, string.Empty, this.CurrentToken, newUser, this.Id, "users");
        }

        /// <summary>
        /// Gets a group that belongs to the site
        /// </summary>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        public Group GetGroup(string groupName, RequestOptions options = null)
        {
            return HttpHelper.Get<Group>(GlobalConfiguration.Routes.SitesIdAction, string.Format("q=[name] eq '{0}'", groupName), options, this.CurrentToken, this.Id, "groups");
        }

        #endregion

        /// <summary>
        /// Gets a user by name if they belong to the site
        /// </summary>
        /// <param name="userId">The userId to get</param>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <returns></returns>
        public User GetUser(string userId, RequestOptions options = null)
        {
            return HttpHelper.Get<User>(GlobalConfiguration.Routes.SitesIdAction, string.Format("q=[userId] eq '{0}'", userId), options, this.CurrentToken, this.Id, "users");
        }
    }
}