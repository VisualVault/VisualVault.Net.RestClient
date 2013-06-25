namespace VVRestApi.Vault.Users
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

        /// <summary>
        /// Gets the current user based on the CurrentToken
        /// </summary>
        /// <param name="refresh">If set to true, the a call will be made to the server to refresh the current user properties.</param>
        /// <returns></returns>
        public User GetCurrentUser(bool refresh = false)
        {
            if (_currentUser == null || refresh == true)
            {
                RequestOptions options = new RequestOptions();
                options.Expand = true;
                _currentUser = HttpHelper.Get<User>(GlobalConfiguration.Routes.UsersId, string.Empty, options, this.CurrentToken, Guid.Empty);
            }

            return _currentUser;
        }

       
    }
}