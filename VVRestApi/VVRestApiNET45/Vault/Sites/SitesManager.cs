using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Sites
{
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class SitesManager : VVRestApi.Common.BaseApi
    {
        internal SitesManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }

        /// <summary>
        /// Gets a site by name, returns null if none exists
        /// </summary>
        /// <param name="siteName">The name of the site to get</param>
        /// <param name="options"> </param>
        /// <returns></returns>
        public Site GetSite(string siteName, RequestOptions options = null)
        {
            return HttpHelper.Get<Site>(VVRestApi.GlobalConfiguration.Routes.Sites, "q=[name] eq '" + siteName + "'", options, GetUrlParts(),this.ClientSecrets, this.ApiTokens);
        }

        /// <summary>
        /// Gets all the sites
        /// </summary>
        /// <param name="options"> </param>
        /// <returns></returns>
        public Page<Site> GetSites(RequestOptions options = null)
        {
            return HttpHelper.GetPagedResult<Site>(VVRestApi.GlobalConfiguration.Routes.Sites, string.Empty, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }
    }
}