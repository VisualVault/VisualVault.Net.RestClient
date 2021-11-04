using System;
using Newtonsoft.Json.Linq;
using VVRestApi.Common;
using VVRestApi.Common.Messaging;
using VVRestApi.Vault.Users;
using System.Collections.Generic;

namespace VVRestApi.Vault.CustomQueries
{
    public class CustomQueryManager : BaseApi
    {
        internal CustomQueryManager(VaultApi api)
        {
            base.Populate(api.ClientSecrets, api.ApiTokens);
        }
        /// <summary>
        /// returns the result of the saved query
        /// </summary>
        /// <param name="queryName"></param>
        /// <param name="options"></param>
        /// <param name="filter"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public JArray GetCustomQueryResults(string queryName, RequestOptions options = null, string filter = null, string sortBy = null, string sortDirection = null, Dictionary<string,string> queryParameters = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }
            var queryString = "";

            List<string> queryStrings = new List<string>();
            queryStrings.Add($"queryName={UrlEncode(queryName)}");
            if (filter != null)
            {
                queryStrings.Add($"filter={UrlEncode(filter)}");
            }
            if (sortBy != null)
            {
                queryStrings.Add($"sort={UrlEncode(sortBy)}");
            }
            if (sortDirection != null)
            {
                queryStrings.Add($"sortDir={UrlEncode(sortDirection)}");
            }
            if (queryParameters != null && queryParameters.Count > 0)
            {
                JArray queryParametersJArray = new JArray();
                foreach (var queryParam in queryParameters)
                {
                    JObject queryParamJObject = new JObject();
                    queryParamJObject["parameterName"] = queryParam.Key;
                    queryParamJObject["value"] = queryParam.Value;
                    queryParametersJArray.Add(queryParamJObject);
                }
                queryStrings.Add($"params={UrlEncode(queryParametersJArray.ToString(Newtonsoft.Json.Formatting.None))}");
            }
            queryString = String.Join("&", queryStrings);

            var results = HttpHelper.Get(GlobalConfiguration.Routes.CustomQuery, queryString, options, GetUrlParts(), this.ApiTokens, this.ClientSecrets);
            var data = results.Value<JArray>("data");
            return data;
        }

        /// <summary>
        /// returns the result of a saved query
        /// </summary>
        /// <param name="queryId"></param>
        /// <param name="options"></param>
        /// <param name="filter"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public dynamic GetCustomQueryResults(Guid queryId, RequestOptions options = null, string filter = null, string sortBy = null, string sortDirection = null, Dictionary<string, string> queryParameters = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            var queryString = "";
            List<string> queryStrings = new List<string>();
            if (filter != null)
            {
                queryStrings.Add($"filter={UrlEncode(filter)}");
            }
            if (sortBy != null)
            {
                queryStrings.Add($"sort={UrlEncode(sortBy)}");
            }
            if (sortDirection != null)
            {
                queryStrings.Add($"sortDir={UrlEncode(sortDirection)}");
            }
            if (queryParameters != null && queryParameters.Count > 0)
            {
                JArray queryParametersJArray = new JArray();
                foreach (var queryParam in queryParameters)
                {
                    JObject queryParamJObject = new JObject();
                    queryParamJObject["parameterName"] = queryParam.Key;
                    queryParamJObject["value"] = queryParam.Value;
                    queryParametersJArray.Add(queryParamJObject);
                }
                queryStrings.Add($"params={UrlEncode(queryParametersJArray.ToString(Newtonsoft.Json.Formatting.None))}");
            }
            queryString = String.Join("&", queryStrings);

            var results = HttpHelper.Get(GlobalConfiguration.Routes.CustomQueryId, queryString, options, GetUrlParts(), this.ApiTokens, this.ClientSecrets, queryId);
            var data = results.Value<JArray>("data");
            return data;
        }

        /// <summary>
        /// returns the result of a saved query as a <see cref="Page{CustomQueryResult}"/> object
        /// </summary>
        /// <param name="queryName"></param>
        /// <param name="options"></param>
        /// <param name="filter"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public Page<CustomQueryResult> GetCustomQueryPagedResults(string queryName, RequestOptions options = null, string filter = null, string sortBy = null, string sortDirection = null, Dictionary<string, string> queryParameters = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            var queryString = "";
            List<string> queryStrings = new List<string>();
            queryStrings.Add($"queryName={UrlEncode(queryName)}");
            if (filter != null)
            {
                queryStrings.Add($"filter={UrlEncode(filter)}");
            }
            if (sortBy != null)
            {
                queryStrings.Add($"sort={UrlEncode(sortBy)}");
            }
            if (sortDirection != null)
            {
                queryStrings.Add($"sortDir={UrlEncode(sortDirection)}");
            }
            if (queryParameters != null && queryParameters.Count > 0)
            {
                JArray queryParametersJArray = new JArray();
                foreach (var queryParam in queryParameters)
                {
                    JObject queryParamJObject = new JObject();
                    queryParamJObject["parameterName"] = queryParam.Key;
                    queryParamJObject["value"] = queryParam.Value;
                    queryParametersJArray.Add(queryParamJObject);
                }
                queryStrings.Add($"params={UrlEncode(queryParametersJArray.ToString(Newtonsoft.Json.Formatting.None))}");
            }
            queryString = String.Join("&", queryStrings);

            var results = HttpHelper.GetPagedResult<CustomQueryResult>(GlobalConfiguration.Routes.CustomQuery, queryString, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens);
            return results;
        }

        /// <summary>
        /// returns the result of a saved query as a <see cref="Page{CustomQueryResult}"/> object
        /// </summary>
        /// <param name="queryId"></param>
        /// <param name="options"></param>
        /// <param name="filter"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        public Page<CustomQueryResult> GetCustomQueryPagedResults(Guid queryId, RequestOptions options = null, string filter = null, string sortBy = null, string sortDirection = null, Dictionary<string, string> queryParameters = null)
        {
            if (options != null && !string.IsNullOrWhiteSpace(options.Fields))
            {
                options.Fields = UrlEncode(options.Fields);
            }

            var queryString = "";
            List<string> queryStrings = new List<string>();
            if (filter != null)
            {
                queryStrings.Add($"filter={UrlEncode(filter)}");
            }
            if (sortBy != null)
            {
                queryStrings.Add($"sort={UrlEncode(sortBy)}");
            }
            if (sortDirection != null)
            {
                queryStrings.Add($"sortDir={UrlEncode(sortDirection)}");
            }
            if(queryParameters != null && queryParameters.Count > 0)
            {
                JArray queryParametersJArray = new JArray();
                foreach(var queryParam in queryParameters)
                {
                    JObject queryParamJObject = new JObject();
                    queryParamJObject["parameterName"] = queryParam.Key;
                    queryParamJObject["value"] = queryParam.Value;
                    queryParametersJArray.Add(queryParamJObject);
                }
                queryStrings.Add($"params={UrlEncode(queryParametersJArray.ToString(Newtonsoft.Json.Formatting.None))}");
            }
            queryString = String.Join("&", queryStrings);

            var results = HttpHelper.GetPagedResult<CustomQueryResult>(GlobalConfiguration.Routes.CustomQueryId, queryString, options, GetUrlParts(), this.ClientSecrets, this.ApiTokens, queryId);
            return results;
        }

    }
}