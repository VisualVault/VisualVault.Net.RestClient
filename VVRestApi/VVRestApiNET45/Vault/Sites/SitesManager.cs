namespace VVRestApi.Vault.Sites
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json.Linq;

    using VVRestApi.Common;

    public class SitesManager : VVRestApi.Common.BaseApi
    {
        internal SitesManager(VaultApi api)
        {
            base.Populate(api.CurrentToken);
        }

        /// <summary>
        /// Gets a site by name, returns null if none exists
        /// </summary>
        /// <param name="siteName">The name of the site to get</param>
        /// <param name="fields">A comma-delimited list of field names to return.</param>
        /// <returns></returns>
        public Site GetSite(string siteName, bool expand = false, string fields = "")
        {
            return HttpHelper.Get<Site>(VVRestApi.GlobalConfiguration.Routes.Sites, "q=[name] eq '" + siteName + "'", expand, fields, this.CurrentToken);
        }

        /// <summary>
        /// Gets all the sites
        /// </summary>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <returns></returns>
        public Page<Site> GetSites(bool expand = false, string fields = "")
        {
            return HttpHelper.GetPagedResult<Site>(VVRestApi.GlobalConfiguration.Routes.Sites, string.Empty, expand, fields, this.CurrentToken);
        }

       
    }
}