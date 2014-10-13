<<<<<<< HEAD
﻿using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Sites
{
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
=======
﻿namespace VVRestApi.Vault.Sites
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json.Linq;

    using VVRestApi.Common;

>>>>>>> origin/master
    public class SitesManager : VVRestApi.Common.BaseApi
    {
        internal SitesManager(VaultApi api)
        {
<<<<<<< HEAD
            base.Populate(api.ClientSecrets, api.ApiTokens);
=======
            base.Populate(api.CurrentToken);
>>>>>>> origin/master
        }

        /// <summary>
        /// Gets a site by name, returns null if none exists
        /// </summary>
        /// <param name="siteName">The name of the site to get</param>
<<<<<<< HEAD
        /// <param name="options"> </param>
        /// <returns></returns>
        public Site GetSite(string siteName, RequestOptions options = null)
        {
            return HttpHelper.Get<Site>(VVRestApi.GlobalConfiguration.Routes.Sites, "q=[name] eq '" + siteName + "'", options, GetUrlParts(),this.ClientSecrets, this.ApiTokens);
=======
        /// <param name="fields">A comma-delimited list of field names to return.</param>
        /// <returns></returns>
        public Site GetSite(string siteName, RequestOptions options = null)
        {
            return HttpHelper.Get<Site>(VVRestApi.GlobalConfiguration.Routes.Sites, "q=[name] eq '" + siteName + "'", options, this.CurrentToken);
>>>>>>> origin/master
        }

        /// <summary>
        /// Gets all the sites
        /// </summary>
<<<<<<< HEAD
        /// <param name="options"> </param>
        /// <returns></returns>
        public Page<Site> GetSites(RequestOptions options = null)
        {
            return HttpHelper.GetPagedResult<Site>(VVRestApi.GlobalConfiguration.Routes.Sites, string.Empty, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
        }
=======
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <returns></returns>
        public Page<Site> GetSites(RequestOptions options = null)
        {
            return HttpHelper.GetPagedResult<Site>(VVRestApi.GlobalConfiguration.Routes.Sites, string.Empty, options, this.CurrentToken);
        }

       
>>>>>>> origin/master
    }
}