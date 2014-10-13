// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestManager.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VVRestApi.Common.Messaging;

namespace VVRestApi.Common
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    ///     Allows you to make REST API calls to VisualVault. These calls will create the correct headers and query strings for you to make authenticated calls.
    /// </summary>
    public class RestManager : BaseApi
    {
        #region Constructors and Destructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="api"></param>
        public RestManager(BaseApi api)
        {
            this.Populate(api.ClientSecrets, api.ApiTokens);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     DELETE to the server
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'. You can find paths that are used internally in VVRestApi.GlobalConfiguration.Routes .</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="virtualPathArgs">The parameters to replace tokens in the virtualPath with.</param>
        /// <returns></returns>
        public JObject Delete(string virtualPath, string queryString, params object[] virtualPathArgs)
        {
            return HttpHelper.Delete(virtualPath, queryString, GetUrlParts(), this.ApiTokens, virtualPathArgs);
        }

        /// <summary>
        ///     GET to the server
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'. You can find paths that are used internally in VVRestApi.GlobalConfiguration.Routes .</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="options"> 
        /// Expand: If set to true, the request will return all available fields.
        /// Fields: A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.
        /// </param>
        /// <param name="virtualPathArgs">The parameters to replace tokens in the virtualPath with.</param>
        /// <returns></returns>
        public JObject Get(string virtualPath, string queryString, RequestOptions options, params object[] virtualPathArgs)
        {
            return HttpHelper.Get(virtualPath, queryString, options, GetUrlParts(), this.ApiTokens, virtualPathArgs);
        }

        /// <summary>
        ///     POSTs to the server using Json data
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'. You can find paths that are used internally in VVRestApi.GlobalConfiguration.Routes .</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="postData">The data to POST.</param>
        /// <param name="virtualPathArgs">The parameters to replace tokens in the virtualPath with.</param>
        /// <returns></returns>
        public JObject Post(string virtualPath, string queryString, object postData, params object[] virtualPathArgs)
        {
            return HttpHelper.Post(virtualPath, queryString, GetUrlParts(), this.ApiTokens, postData, virtualPathArgs);
        }

        /// <summary>
        ///     PUT to the server using Json data
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'. You can find paths that are used internally in VVRestApi.GlobalConfiguration.Routes .</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="putData">The data to PUT.</param>
        /// <param name="virtualPathArgs">The parameters to replace tokens in the virtualPath with.</param>
        /// <returns></returns>
        public JObject Put(string virtualPath, string queryString, object putData, params object[] virtualPathArgs)
        {
            return HttpHelper.Post(virtualPath, queryString, GetUrlParts(), this.ApiTokens, putData, virtualPathArgs);
        }

        #endregion
    }
}