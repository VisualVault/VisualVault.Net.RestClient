using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Users
{
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class UsersManager : VVRestApi.Common.BaseApi
    {
        internal UsersManager(VaultApi api)
        {
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
        }

        /// <summary>
        /// Gets a user by their username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="options"> </param>
        /// <returns></returns>
        public User GetUser(string username, RequestOptions options = null)
        {
            return HttpHelper.Get<User>(GlobalConfiguration.Routes.Users, string.Format("q=[userid] eq '{0}'", username), options, GetUrlParts(),this.ClientSecrets, this.ApiTokens);
        }
    }
}