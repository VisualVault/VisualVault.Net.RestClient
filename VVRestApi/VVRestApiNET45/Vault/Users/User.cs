// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApi.Vault.Users
{
    using System;

    using Newtonsoft.Json;

    using VVRestApi.Common;

    public class User : RestObject
    {
        #region Constructors and Destructors

        public User()
        {
            this.UserId = string.Empty;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Email address of the user
        /// </summary>
        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; internal set; }

        /// <summary>
        /// True if the user is enabled, false if the user is disabled
        /// </summary>
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; internal set; }

        /// <summary>
        /// First name of the user
        /// </summary>
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; internal set; }

        /// <summary>
        /// The Id of the user.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; internal set; }


        /// <summary>
        /// The last IP address of the user when they logged in through the Web UI.
        /// </summary>
        [JsonProperty(PropertyName = "lastIpAddress")]
        public string LastIpAddress { get; internal set; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; internal set; }


        /// <summary>
        /// Middle initial of the user
        /// </summary>
        [JsonProperty(PropertyName = "middleInitial")]
        public string MiddleInitial { get; internal set; }

        /// <summary>
        ///     The display name / alias of the user
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; internal set; }

        /// <summary>
        /// Date and time when the password will expire, if it is set - otherwise it is null
        /// </summary>
        [JsonProperty(PropertyName = "passwordExpires")]
        public DateTime? PasswordExpires { get; internal set; }

        /// <summary>
        /// If true, the password will never expire.
        /// </summary>
        [JsonProperty(PropertyName = "passwordNeverExpires")]
        public bool PasswordNeverExpires { get; internal set; }
        
        /// <summary>
        /// The ID of the primary site that the user belongs to
        /// </summary>
        [JsonProperty(PropertyName = "siteId")]
        public Guid SiteId { get; internal set; }

        /// <summary>
        ///  Login name of the user
        /// </summary>
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        /// <summary>
        /// If set to true, the user ID will never have to be reset.
        /// </summary>
        [JsonProperty(PropertyName = "userIdNeverExpires")]
        public bool UserIdNeverExpires { get; internal set; }

        /// <summary>
        /// Date and time when the User ID will expire, if it is set - otherwise it is null
        /// </summary>
        [JsonProperty(PropertyName = "UserIdExpires")]
        public DateTime? UserIdExpires { get; internal set; }

        #endregion
    }
}