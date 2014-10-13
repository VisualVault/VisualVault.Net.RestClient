<<<<<<< HEAD
﻿using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Users
{
    using System;
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class CurrentUserManager : VVRestApi.Common.BaseApi
    {
        internal CurrentUserManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        private User _currentUser;
=======
﻿namespace VVRestApi.Vault.Users
{
    using System;

    using Newtonsoft.Json.Linq;

    using VVRestApi.Common;

    public class CurrentUserManager: VVRestApi.Common.BaseApi
    {
        internal CurrentUserManager(VaultApi api)
        {
            base.Populate(api.CurrentToken);
        }

        private User _currentUser = null;
>>>>>>> origin/master

        /// <summary>
        /// Gets the current user based on the CurrentToken
        /// </summary>
        /// <param name="refresh">If set to true, the a call will be made to the server to refresh the current user properties.</param>
        /// <returns></returns>
        public User GetCurrentUser(bool refresh = false)
        {
<<<<<<< HEAD
            if (_currentUser == null || refresh)
            {
                RequestOptions options = new RequestOptions {Expand = true};
                _currentUser = HttpHelper.Get<User>(GlobalConfiguration.Routes.UsersId, string.Empty, options, GetUrlParts(), this.ClientSecrets,this.ApiTokens, Guid.Empty);
=======
            if (_currentUser == null || refresh == true)
            {
                RequestOptions options = new RequestOptions();
                options.Expand = true;
                _currentUser = HttpHelper.Get<User>(GlobalConfiguration.Routes.UsersId, string.Empty, options, this.CurrentToken, Guid.Empty);
>>>>>>> origin/master
            }

            return _currentUser;
        }
<<<<<<< HEAD
=======

       
>>>>>>> origin/master
    }
}