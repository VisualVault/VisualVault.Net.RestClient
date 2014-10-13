<<<<<<< HEAD
﻿using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Users
{
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
=======
﻿namespace VVRestApi.Vault.Users
{
    using VVRestApi.Common;

>>>>>>> origin/master
    public class UsersManager : VVRestApi.Common.BaseApi
    {
        internal UsersManager(VaultApi api)
        {
<<<<<<< HEAD
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }


        /// <summary>
        /// Gets a page of users with no filter
        /// </summary>
        /// <param name="options"> </param>
        /// <returns></returns>
        public Page<User> GetUsers(RequestOptions options = null)
        {
            var results = HttpHelper.GetPagedResult<User>(GlobalConfiguration.Routes.Users, string.Empty, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
            
            return results;
=======
            base.Populate(api.CurrentToken);
>>>>>>> origin/master
        }

        /// <summary>
        /// Gets a user by their username
        /// </summary>
        /// <param name="username"></param>
<<<<<<< HEAD
        /// <param name="options"> </param>
        /// <returns></returns>
        public User GetUser(string username, RequestOptions options = null)
        {
            return HttpHelper.Get<User>(GlobalConfiguration.Routes.Users, string.Format("q=[userid] eq '{0}'", username), options, GetUrlParts(),this.ClientSecrets, this.ApiTokens);
=======
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <returns></returns>
        public User GetUser(string username, RequestOptions options = null)
        {
            return HttpHelper.Get<User>(GlobalConfiguration.Routes.Users, string.Format("q=[userid] eq '{0}'", username), options, this.CurrentToken);
>>>>>>> origin/master
        }
    }
}