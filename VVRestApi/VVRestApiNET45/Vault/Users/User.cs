// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Users
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using VVRestApi.Common;
    using VVRestApi.Common.Extensions;

    /// <summary>
    /// 
    /// </summary>
    public class User : RestObject
    {
        #region Constructors and Destructors

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// Gets a login token that can be used in a url to give access as that user to VisualVault. You can get an access token for another user if you are a VaultAccess account, otherwise you are limited to your own account.
        /// </summary>
        /// <returns></returns>
        public string GetWebLoginToken(DateTime? expirationDateUtc = null, bool formatInUrl = false, RequestOptions options = null)
        {
            var webLoginToken = string.Empty;
            string query = string.Empty;
            if (expirationDateUtc.HasValue)
            {
                query = "expiration=" + expirationDateUtc.Value.ToString("o");
            }
            var result = HttpHelper.Get(GlobalConfiguration.Routes.UsersIdAction, query, options, GetUrlParts(), this.ApiTokens, this.Id, "webToken");
            if (result != null)
            {
                var meta = result.GetMetaData();
                if (meta != null)
                {
                    if (meta.IsAffirmativeStatus())
                    {
                        JToken data = result.GetData();
                        webLoginToken = data["webToken"].Value<string>();
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(webLoginToken) && formatInUrl)
            {
                webLoginToken = GetUrlParts().BaseUrl.Replace("/api/v1/", "/VVLogin?token=" + webLoginToken);
            }

            return webLoginToken;
        }

        /// <summary>
        /// Gets a login token that can be used in a VisualVault web application url to bypass the login prompt. Use case is a service account passing through a user's credentials entered into a login form (using HTTPS of course).
        /// </summary>
        /// <returns></returns>
        public string GetWebLoginToken(string userId, string password, DateTime? expirationDateUtc = null, bool formatInUrl = false, RequestOptions options = null)
        {
            var webLoginToken = string.Empty;
            string query = string.Empty;
            if (expirationDateUtc.HasValue)
            {
                query = "expiration=" + expirationDateUtc.Value.ToString("o");
            }

            if (query.Length > 0)
            {
                query += "&";
            }

            query += "u={0}&p={1}";

            var result = HttpHelper.Get(GlobalConfiguration.Routes.UsersIdAction, query, options, GetUrlParts(), this.ApiTokens, this.Id, "webToken");
            if (result != null)
            {
                var meta = result.GetMetaData();
                if (meta != null)
                {
                    if (meta.IsAffirmativeStatus())
                    {
                        JToken data = result.GetData();
                        webLoginToken = data["webToken"].Value<string>();
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(webLoginToken) && formatInUrl)
            {
                webLoginToken = GetUrlParts().BaseUrl.Replace("/api/v1/", "/VVLogin?token=" + webLoginToken);
            }

            return webLoginToken;
        }

        /// <summary>
        /// Gets a login token that can be used in a url to give access as that user to VisualVault. You can get an access token for another user if you are a VaultAccess account, otherwise you are limited to your own account.
        /// </summary>
        /// <param name="redirectUrl">The url you want to redirect to within VisualVault, such as "DocumentLibrary"</param>
        /// <param name="expirationDateUtc">A UTC DateTime of when the token will expire. Null will set it to five minutes.</param>
        /// <param name="options"></param>
        /// <returns></returns>
        public string GetWebLoginToken(string redirectUrl, DateTime? expirationDateUtc = null, RequestOptions options = null)
        {
            var webLoginToken = string.Empty;
            string query = string.Empty;
            if (expirationDateUtc.HasValue)
            {
                query = "expiration=" + expirationDateUtc.Value.ToString("o");
            }
            var result = HttpHelper.Get(GlobalConfiguration.Routes.UsersIdAction, query, options, GetUrlParts(), this.ApiTokens, this.Id, "webToken");
            if (result != null)
            {
                var meta = result.GetMetaData();
                if (meta != null)
                {
                    if (meta.IsAffirmativeStatus())
                    {
                        JToken data = result.GetData();
                        webLoginToken = data["webToken"].Value<string>();
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(webLoginToken))
            {
                webLoginToken = GetUrlParts().BaseUrl.Replace("/api/v1/", "/VVLogin?token=" + webLoginToken);
                if (!String.IsNullOrWhiteSpace(redirectUrl))
                {
                    webLoginToken += "&returnUrl=" + this.UrlEncode(redirectUrl);
                }
            }

            return webLoginToken;
        }
    }
}