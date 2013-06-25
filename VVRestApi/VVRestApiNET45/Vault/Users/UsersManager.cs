namespace VVRestApi.Vault.Users
{
    using VVRestApi.Common;

    public class UsersManager : VVRestApi.Common.BaseApi
    {
        internal UsersManager(VaultApi api)
        {
            base.Populate(api.CurrentToken);
        }

        /// <summary>
        /// Gets a user by their username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <returns></returns>
        public User GetUser(string username, RequestOptions options = null)
        {
            return HttpHelper.Get<User>(GlobalConfiguration.Routes.Users, string.Format("q=[userid] eq '{0}'", username), options, this.CurrentToken);
        }
    }
}