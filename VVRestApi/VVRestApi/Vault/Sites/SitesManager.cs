using System;
using System.Dynamic;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Sites
{
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class SitesManager : BaseApi
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
            return HttpHelper.Get<Site>(GlobalConfiguration.Routes.Sites, "q=[name] eq '" + siteName + "'", options, GetUrlParts(),this.ClientSecrets, this.ApiTokens);
        }

        /// <summary>
        /// Gets all the sites
        /// </summary>
        /// <param name="options"> </param>
        /// <returns></returns>
        public Page<Site> GetSites(RequestOptions options = null)
        {
            return HttpHelper.GetPagedResult<Site>(GlobalConfiguration.Routes.Sites, string.Empty, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }

        /// <summary>
        /// creates a new group in the site
        /// </summary>
        /// <returns></returns>
        public Site CreateSite(string siteName, string description)
        {
            dynamic postData = new ExpandoObject();
            postData.name = siteName;
            postData.description = description;
            postData.type = "Location";

            return HttpHelper.Post<Site>(GlobalConfiguration.Routes.Sites, "", GetUrlParts(), this.ClientSecrets, this.ApiTokens, postData);
        }
    }
}