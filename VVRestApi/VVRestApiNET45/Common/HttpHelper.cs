// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpHelper.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace VVRestApi.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Authentication;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using VVRestApi.Common.Logging;
    using VVRestApi.Common.Messaging;

    public static class HttpHelper
    {
        #region Public Methods and Operators

        public static T Delete<T>(string virtualPath, string queryString, SessionToken token, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Delete(virtualPath, queryString, token, virtualPathArgs);
            return ConvertToRestTokenObject<T>(token, resultData);
        }

        public static ApiMetaData DeleteReturnMeta(string virtualPath, string queryString, SessionToken token, params object[] virtualPathArgs)
        {
            JObject resultData = Delete(virtualPath, queryString, token, virtualPathArgs);
            return ConvertToRestTokenToApiMetaData(token, resultData);
        }

        private static ApiMetaData ConvertToRestTokenToApiMetaData(SessionToken token, JObject resultData)
        {
            ApiMetaData result = null;

            if (resultData != null)
            {
                result = new ApiMetaData();
                result.StatusCode = HttpStatusCode.NoContent;

                result = resultData["meta"].ToObject<ApiMetaData>();
            }

            return result;
        }

        /// <summary>
        ///     Trigger a call to do an HttpDelete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="virtualPath">The virtual path, with optional format tokens: ~/SomeEndpoint/{0}/</param>
        /// <param name="queryString"></param>
        /// <param name="token"></param>
        /// <param name="virtualPathParameters"></param>
        /// <returns></returns>
        public static JObject Delete(string virtualPath, string queryString, SessionToken token, params object[] virtualPathArgs)
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = token.CreateUrl(string.Format(virtualPath, virtualPathArgs), queryString, HttpMethod.Delete);
            string signature = CreateAuthorization(client.DefaultRequestHeaders, new Uri(url), "DELETE", string.Empty, token.DeveloperKey, token.DeveloperSecret);
            client.DefaultRequestHeaders.Add("X-Authorization", signature);

            OutputCurlCommand(client, HttpMethod.Delete, url, null);

            Task task = client.DeleteAsync(url).ContinueWith(async taskwithresponse =>
                {
                    try
                    {
                        JObject result = await taskwithresponse.Result.Content.ReadAsAsync<JObject>();
                        resultData = ProcessResultData(result, url, HttpMethod.Delete);
                    }
                    catch (Exception ex)
                    {
                        HandleTaskException(taskwithresponse, ex, HttpMethod.Delete);
                    }
                });

            task.Wait();

            return resultData;
        }

        public static List<T> DeleteListResult<T>(string virtualPath, string queryString, SessionToken token, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Delete(virtualPath, queryString, token, virtualPathArgs);
            return ConvertToRestTokenObjectList<T>(token, resultData);
        }

        /// <summary>
        ///     GET the virtual path with the querystring and field options
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <param name="token">The current token.</param>
        /// <param name="virtualPathArgs">The arguments to replace the tokens ({0},{1}, etc.) in the virtual path</param>
        /// <returns></returns>
        public static T Get<T>(string virtualPath, string queryString, RequestOptions options, SessionToken token, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Get(virtualPath, queryString, options, token, virtualPathArgs);
            var result = ConvertToRestTokenObject<T>(token, resultData);

            return result;
        }  
        
        public static ApiMetaData GetReturnMeta(string virtualPath, string queryString, RequestOptions options, SessionToken token, params object[] virtualPathArgs) 
        {
            JObject resultData = Get(virtualPath, queryString, options, token, virtualPathArgs);
            return ConvertToRestTokenToApiMetaData(token, resultData);
        }

        /// <summary>
        ///     GET the virtual path with the querystring and field options
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <param name="token">The current token.</param>
        /// <param name="virtualPathArgs">The arguments to replace the tokens ({0},{1}, etc.) in the virtual path</param>
        /// <returns></returns>
        public static JObject Get(string virtualPath, string queryString, RequestOptions options, SessionToken token, params object[] virtualPathArgs)
        {
            if (options == null)
            {
                options = new RequestOptions();
            }

            options.PrepForRequest();

            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = token.CreateUrl(string.Format(virtualPath, virtualPathArgs), options.GetQueryString(queryString), HttpMethod.Get, options.Fields, options.Expand);
            string signature = CreateAuthorization(client.DefaultRequestHeaders, new Uri(url), "GET", string.Empty, token.DeveloperKey, token.DeveloperSecret);
            client.DefaultRequestHeaders.Add("X-Authorization", signature);

            OutputCurlCommand(client, HttpMethod.Get, url, null);

            Task task = client.GetAsync(url).ContinueWith(async taskwithresponse =>
                {
                    try
                    {
                        JObject result = await taskwithresponse.Result.Content.ReadAsAsync<JObject>();
                        resultData = ProcessResultData(result, url, HttpMethod.Get);
                    }
                    catch (Exception ex)
                    {
                        HandleTaskException(taskwithresponse, ex, HttpMethod.Get);
                    }
                });

            task.Wait();

            return resultData;
        }

        /// <summary>
        ///     GET a List of T back. Use when you are expecting and array of results.
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <param name="token">The current token.</param>
        /// <param name="virtualPathArgs">The arguments to replace the tokens ({0},{1}, etc.) in the virtual path</param>
        /// <returns></returns>
        public static Page<T> GetPagedResult<T>(string virtualPath, string queryString, RequestOptions options, SessionToken token, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JToken resultData = Get(virtualPath, queryString, options, token, virtualPathArgs);


            Page<T> result = ConvertToRestTokenObjectPage<T>(token, resultData);

            return result;
        }

        public static void OutputCurlCommand(HttpClient client, HttpMethod method, string url, StringContent content, bool writeAllHeaders = false)
        {
            if (GlobalEvents.IsListening(LogLevelType.Debug))
            {
                var sbCurl = new StringBuilder("CURL equivalent command: " + Environment.NewLine);
                sbCurl.Append("curl ");
                if (method == HttpMethod.Get)
                {
                    sbCurl.AppendLine(@"-X GET \");
                }
                else if (method == HttpMethod.Delete)
                {
                    sbCurl.AppendLine(@"-X DELETE \");
                }
                else if (method == HttpMethod.Post)
                {
                    sbCurl.AppendLine(@"-X POST \");
                }
                else if (method == HttpMethod.Put)
                {
                    sbCurl.AppendLine(@"-X PUT \");
                }

                if (content != null)
                {
                    sbCurl.AppendFormat("\t-H \"Content-Type: {0}\" \\{1}", content.Headers.ContentType.MediaType, Environment.NewLine);
                }

                foreach (var header in client.DefaultRequestHeaders)
                {
                    bool writeHeader = true;

                    // if (writeAllHeaders)
                    // {
                    // writeHeader = true;
                    // }
                    // else if (header.Key.StartsWith("x-", StringComparison.OrdinalIgnoreCase) || header.Key.StartsWith("Authorization"))
                    // {
                    // writeHeader = true;
                    // }
                    if (writeHeader)
                    {
                        sbCurl.AppendFormat("\t-H \"{0}: {1}\" \\{2}", header.Key, header.Value.Aggregate((i, j) => i + " " + j), Environment.NewLine);
                    }
                }

                if (content != null)
                {
                    string jsonData = string.Empty;
                    Task task = content.ReadAsStringAsync().ContinueWith(async f => { jsonData = f.Result; });

                    task.Wait();

                    if (!string.IsNullOrWhiteSpace(jsonData))
                    {
                        sbCurl.AppendFormat("\t-d \'{0}\' \\ {1}", jsonData.Replace(Environment.NewLine, Environment.NewLine + "\t\t"), Environment.NewLine);
                    }
                }

                sbCurl.AppendLine(url);

                LogEventManager.Debug(sbCurl.ToString());
            }
        }

        public static T Post<T>(string virtualPath, string queryString, SessionToken token, object postData, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Post(virtualPath, queryString, token, postData, virtualPathArgs);
            var result = ConvertToRestTokenObject<T>(token, resultData);

            return result;
        }

        public static ApiMetaData PostReturnMeta(string virtualPath, string queryString, SessionToken token, object postData, params object[] virtualPathArgs)
        {
            JObject resultData = Post(virtualPath, queryString, token, postData, virtualPathArgs);
            return ConvertToRestTokenToApiMetaData(token, resultData);
        }

        /// <summary>
        ///     Posts to the server using Json data
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="token">The current token.</param>
        /// <param name="postData">The data to post.</param>
        /// <param name="virtualPathArgs">The parameters to replace tokens in the virtualPath with.</param>
        /// <returns></returns>
        public static JObject Post(string virtualPath, string queryString, SessionToken token, object postData, params object[] virtualPathArgs)
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = token.CreateUrl(string.Format(virtualPath, virtualPathArgs), queryString, HttpMethod.Post);
            token.PrepPostPutHeaders(client);
            PreProcessPostData(postData, url, HttpMethod.Post);

            string jsonToPost = string.Empty;
            if (postData != null)
            {
                jsonToPost = JsonConvert.SerializeObject(postData, GlobalConfiguration.GetJsonSerializerSettings());
            }

            string signature = CreateAuthorization(client.DefaultRequestHeaders, new Uri(url), "POST", jsonToPost, token.DeveloperKey, token.DeveloperSecret);
            client.DefaultRequestHeaders.Add("X-Authorization", signature);

            var content = new StringContent(jsonToPost);
            content.Headers.ContentType.MediaType = "application/json";

            OutputCurlCommand(client, HttpMethod.Post, url, content);

            Task task = client.PostAsync(url, content).ContinueWith(async taskwithresponse =>
                {
                    try
                    {
                        JObject result = await taskwithresponse.Result.Content.ReadAsAsync<JObject>();
                        resultData = ProcessResultData(result, url, HttpMethod.Post);
                    }
                    catch (Exception ex)
                    {
                        HandleTaskException(taskwithresponse, ex, HttpMethod.Post);
                    }
                });

            task.Wait();

            return resultData;
        }

        public static List<T> PostListResult<T>(string virtualPath, string queryString, SessionToken token, object postData, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Post(virtualPath, queryString, token, postData, virtualPathArgs);
            return ConvertToRestTokenObjectList<T>(token, resultData);
        }

        /// <summary>
        ///     Processes the resultData and extracts the status code
        /// </summary>
        /// <param name="resultData"></param>
        /// <param name="targetUrl"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static JObject ProcessResultData(JObject resultData, string targetUrl, HttpMethod method)
        {
            var statusCode = HttpStatusCode.NotFound;

            if (resultData == null)
            {
                LogEventManager.Warn(GenerateMessage("Http response returned null.", null, targetUrl, method));
                statusCode = HttpStatusCode.ExpectationFailed;
                return null;
            }

            JObject jData = resultData;

            try
            {
                if (jData != null)
                {
                    if (jData["meta"] != null)
                    {
                        if (jData["meta"]["status"] != null)
                        {
                            statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), ((int)jData["meta"]["status"]).ToString());

                            if (!IsAffirmativeStatusCode(statusCode))
                            {
                                LogEventManager.Warn(GenerateMessage("JSON response did not return an affirmative response.", resultData, targetUrl, method));
                            }
                            else
                            {
                                LogEventManager.Verbose(GenerateMessage(string.Empty, resultData, targetUrl, method));
                            }
                        }
                        else
                        {
                            LogEventManager.Warn(GenerateMessage("JSON response did not define 'status' (HttpStatusCode) on the 'meta' node.", resultData, targetUrl, method));
                        }
                    }
                    else
                    {
                        LogEventManager.Warn(GenerateMessage("JSON response did not define 'meta'.", resultData, targetUrl, method));
                    }
                }
                else
                {
                    LogEventManager.Warn(GenerateMessage("Unable to preprocess the returned data because it could not be cast to a JSON Object.", resultData, targetUrl, method));
                }
            }
            catch (Exception ex)
            {
                if (jData != null)
                {
                    LogEventManager.Error(GenerateMessage("Error processing the JSON result.", resultData, targetUrl, method), ex);
                }
                else
                {
                    LogEventManager.Error("Error processing the JSON result.", ex);
                }
            }

            return jData;
        }

        public static T Put<T>(string virtualPath, string queryString, SessionToken token, object postData, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Put(virtualPath, queryString, token, postData, virtualPathArgs);
            var result = ConvertToRestTokenObject<T>(token, resultData);

            return result;
        }

        public static ApiMetaData PutReturnMeta(string virtualPath, string queryString, SessionToken token, object postData, params object[] virtualPathArgs)
        {
            JObject resultData = Put(virtualPath, queryString, token, postData, virtualPathArgs);
            return ConvertToRestTokenToApiMetaData(token, resultData);
        }

        /// <summary>
        ///     Posts to the server using Json data
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="token">The current token.</param>
        /// <param name="postData">The data to post.</param>
        /// <param name="virtualPathArgs">The parameters to replace tokens in the virtualPath with.</param>
        /// <returns></returns>
        public static JObject Put(string virtualPath, string queryString, SessionToken token, object postData, params object[] virtualPathArgs)
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = token.CreateUrl(string.Format(virtualPath, virtualPathArgs), queryString, HttpMethod.Put);
            token.PrepPostPutHeaders(client);

            PreProcessPostData(postData, url, HttpMethod.Put);
            string jsonToPut = string.Empty;
            if (postData != null)
            {
                jsonToPut = JsonConvert.SerializeObject(postData, GlobalConfiguration.GetJsonSerializerSettings());
            }

            string signature = CreateAuthorization(client.DefaultRequestHeaders, new Uri(url), "PUT", jsonToPut, token.DeveloperKey, token.DeveloperSecret);
            client.DefaultRequestHeaders.Add("X-Authorization", signature);

            var content = new StringContent(jsonToPut);
            content.Headers.ContentType.MediaType = "application/json";

            OutputCurlCommand(client, HttpMethod.Put, url, content);

            Task task = client.PutAsync(url, content).ContinueWith(async taskwithresponse =>
                {
                    try
                    {
                        JObject result = await taskwithresponse.Result.Content.ReadAsAsync<JObject>();
                        resultData = ProcessResultData(result, url, HttpMethod.Post);
                    }
                    catch (Exception ex)
                    {
                        HandleTaskException(taskwithresponse, ex, HttpMethod.Put);
                    }
                });

            task.Wait();

            return resultData;
        }

        public static List<T> PutListResult<T>(string virtualPath, string queryString, SessionToken token, object postData, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Put(virtualPath, queryString, token, postData, virtualPathArgs);
            return ConvertToRestTokenObjectList<T>(token, resultData);
        }

        #endregion

        #region Methods

        private static void CleanupVirtualPathArgs(object[] virtualPathArgs)
        {
            if (virtualPathArgs != null && virtualPathArgs.Length > 0)
            {
                for (int i = 0; i < virtualPathArgs.Length; i++)
                {
                    if (virtualPathArgs[i] is string)
                    {
                        virtualPathArgs[i] = WebUtility.UrlEncode((string)virtualPathArgs[i]);
                    }
                }

                if (virtualPathArgs[virtualPathArgs.Length - 1] is string)
                {
                    var pathArg = (string)virtualPathArgs[virtualPathArgs.Length - 1];
                    if (pathArg.Contains("."))
                    {
                        pathArg = pathArg.Replace(".", "%2E");

                        virtualPathArgs[virtualPathArgs.Length - 1] = pathArg;
                    }
                }
            }
        }

        private static T ConvertToRestTokenObject<T>(SessionToken token, JObject resultData) where T : RestObject, new()
        {
            T result = null;

            if (resultData != null)
            {

                JToken dataNode = resultData["data"];
                if (dataNode != null)
                {
                    if (dataNode.Type == JTokenType.Array)
                    {
                        if (dataNode.First != null)
                        {
                            result = JsonConvert.DeserializeObject<T>(dataNode.First.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                            if (result != null)
                            {
                                result.PopulateSessionToken(token);
                                result.Meta = resultData["meta"].ToObject<ApiMetaData>(); // JsonConvert.DeserializeObject<ApiMetaData>(resultData["meta"].ToString(), GlobalConfiguration.GetJsonSerializerSettings()); 
                            }
                        }
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<T>(dataNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                        if (result != null)
                        {
                            result.PopulateSessionToken(token);
                            result.Meta = resultData["meta"].ToObject<ApiMetaData>();
                        }
                    }
                }
                else
                {
                    JToken metaNode = resultData["meta"];
                    if (metaNode != null)
                    {
                        LogEventManager.Error(string.Format("No data returned: {0}{1}", Environment.NewLine, metaNode.ToString()));
                    
                    }
                }
            }

            return result;
        }

        private static Page<T> ConvertToRestTokenObjectPage<T>(SessionToken token, JToken resultData) where T : RestObject, new()
        {
            var result = new Page<T>(token);
            List<T> resultSet = new List<T>();
            if (resultData != null)
            {
                JToken dataNode = resultData["data"];
                if (dataNode != null)
                {
                    var paginationToken = resultData.SelectToken("pagination", false);
                    if (paginationToken != null)
                    {
                        result.Meta = resultData["meta"].ToObject<ApiMetaData>();

                        //Pull all the paged data information
                        result.First = paginationToken["first"].Value<string>();
                        result.Last = paginationToken["last"].Value<string>();
                        result.Limit = paginationToken["limit"].Value<int>();
                        result.Next = paginationToken["next"].Value<string>();
                        result.Previous = paginationToken["previous"].Value<string>();
                        result.TotalRecords = paginationToken["totalRecords"].Value<int>();
                    }


                    if (dataNode.Type == JTokenType.Array)
                    {
                        foreach (JToken dataItem in dataNode.Children())
                        {
                            var item = JsonConvert.DeserializeObject<T>(dataItem.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                            if (item != null)
                            {
                                item.PopulateSessionToken(token);
                                item.Meta = resultData["meta"].ToObject<ApiMetaData>();
                            }

                            resultSet.Add(item);
                        }
                    }
                    else
                    {
                        var item = JsonConvert.DeserializeObject<T>(dataNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                        if (item != null)
                        {
                            item.PopulateSessionToken(token);
                            item.Meta = resultData["meta"].ToObject<ApiMetaData>();
                        }

                        resultSet.Add(item);
                    }
                }
            }

            result.Items = resultSet;

            return result;
        }

        private static List<T> ConvertToRestTokenObjectList<T>(SessionToken token, JToken resultData) where T : RestObject, new()
        {
            var result = new List<T>();
            if (resultData != null)
            {
                JToken dataNode = resultData["data"];
                if (dataNode != null)
                {
                    var dataTypeToken = dataNode.SelectToken("dataType", false);
                    if (dataTypeToken != null)
                    {
                        string dataType = dataTypeToken.Value<string>();

                        if (dataType.Equals("PagedData", StringComparison.OrdinalIgnoreCase))
                        {
                            //The data for paged data resides in items
                            JToken itemsNode = dataNode.SelectToken("items", false);

                            if (itemsNode != null)
                            {
                                dataNode = itemsNode;
                            }
                        }
                    }


                    if (dataNode.Type == JTokenType.Array)
                    {
                        foreach (JToken dataItem in dataNode.Children())
                        {
                            var item = JsonConvert.DeserializeObject<T>(dataItem.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                            if (item != null)
                            {
                                item.PopulateSessionToken(token);
                                item.Meta = resultData["meta"].ToObject<ApiMetaData>();
                            }

                            result.Add(item);
                        }
                    }
                    else
                    {
                        var item = JsonConvert.DeserializeObject<T>(dataNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                        if (item != null)
                        {
                            item.PopulateSessionToken(token);
                            item.Meta = resultData["meta"].ToObject<ApiMetaData>();
                        }

                        result.Add(item);
                    }
                }
            }

            return result;
        }



        private static string GenerateMessage(string message, object resultData, string targetUrl, HttpMethod method)
        {
            if (resultData == null)
            {
                // return string.Format("{0} to Url: {1}{3}{2}{3}Result JSON data: null", method, targetUrl, message, Environment.NewLine);
                return string.Format("{0}Result JSON data: null", Environment.NewLine);
            }
            else
            {
                return string.Format("{0}Result JSON data:{0}{1}", Environment.NewLine, resultData);
            }
        }

        private static void HandleTaskException(Task<HttpResponseMessage> taskwithresponse, Exception exception, HttpMethod method)
        {
            try
            {
                if (taskwithresponse.Result.Content != null)
                {
                    LogEventManager.Warn(method + " to " + taskwithresponse.Result.Content + " error. Original exception: " + exception);
                    if (taskwithresponse.Result.Content.Headers.ContentType.MediaType.StartsWith("text/", StringComparison.OrdinalIgnoreCase))
                    {
                        Task task = taskwithresponse.Result.Content.ReadAsStringAsync().ContinueWith(async taskReader => { LogEventManager.Error("Response could not be converted to JSON because it was of type: " + taskwithresponse.Result.Content.Headers.ContentType.MediaType + ". Response:" + taskReader.Result); });

                        task.Wait();
                    }
                    else
                    {
                        LogEventManager.Error("Unable to parse response of type: " + taskwithresponse.Result.Content.Headers.ContentType.MediaType);
                    }
                }
                else
                {
                    LogEventManager.Error("Response for " + method + " unavailable to parse for error messages.");
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Error("Error occurred while trying to handle exception from " + method + " operation", ex);
            }
        }

        private static bool IsAffirmativeStatusCode(HttpStatusCode statusCode)
        {
            bool result = false;
            switch (statusCode)
            {
                case HttpStatusCode.Continue:
                case HttpStatusCode.SwitchingProtocols:
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                case HttpStatusCode.Accepted:
                case HttpStatusCode.NonAuthoritativeInformation:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.ResetContent:
                case HttpStatusCode.PartialContent:
                    result = true;
                    break;
            }

            return result;
        }

        private static void PreProcessPostData(object postData, string targetUrl, HttpMethod method)
        {
            // if (LogEventManager.IsListening(LogLevelType.Verbose))
            // {
            // if (postData != null)
            // {
            // string jsonToPost = JsonConvert.SerializeObject(postData, GlobalConfiguration.GetJsonSerializerSettings());
            // string message = string.Format("{0} to Url: {1}{3}{2}{3}JSON data for {0}: {4}", method, targetUrl, string.Empty, Environment.NewLine, jsonToPost);

            // LogEventManager.Verbose(message);
            // }
            // }
        }

        #endregion

        #region Authorization Signature

        #region Constants

        /// <summary>
        ///     The Set of accepted and valid Url characters per RFC3986.
        ///     Characters outside of this set will be encoded.
        /// </summary>
        internal const string ValidUrlCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        /// <summary>
        ///     The Set of accepted and valid Url characters per RFC1738.
        ///     Characters outside of this set will be encoded.
        /// </summary>
        internal const string ValidUrlCharactersRFC1738 = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.";

        #endregion

        #region Static Fields

        internal static Dictionary<int, string> RFCEncodingSchemes = new Dictionary<int, string> { { 3986, ValidUrlCharacters }, { 1738, ValidUrlCharactersRFC1738 } };

        #endregion

        #region Public Methods and Operators

        public static string CreateAuthorization(HttpRequestHeaders headers, Uri target, string method, string data, string developerKey, string developerSecret)
        {
            string payloadHash = string.Empty;
            string requestDate = DateTime.UtcNow.ToString("o");
            headers.Add("X-VV-RequestDate", requestDate);

            string algorithm = "VVA-HMAC-SHA256";
            if (!string.IsNullOrEmpty(data))
            {
                payloadHash = HexEncodeHash(data, algorithm);
            }

            string canonicalHeaders = GetCanonicalHeaders(headers);
            string canonicalSignedHeaders = GetCanonicalSignedHeaders(headers);
            string canonicalQueryString = GetCanonicalQueryString(target);
            string canonicalUri = GetCanonicalUri(target);

            string canonicalRequest = string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", method, canonicalUri, canonicalQueryString, canonicalHeaders, canonicalSignedHeaders, payloadHash);
            string hashedCanonicalRequest = HexEncodeHash(canonicalRequest, algorithm);

            string stringToSign = string.Format("{0}\n{1}\n{2}\n{3}", algorithm, requestDate, developerKey, hashedCanonicalRequest);

            Guid nonce = Guid.NewGuid();
            string kSignature = CreateHmacSignature(CreateUtf8(nonce + developerSecret), CreateUtf8(requestDate), algorithm);

            string signature = CreateHmacSignature(CreateUtf8(stringToSign), CreateUtf8(kSignature), algorithm);

            string authorization = string.Format("Algorithm={0}, Credential={1}, SignedHeaders={2}, Nonce={3}, Signature={4}", algorithm, developerKey, canonicalSignedHeaders, nonce, signature);

            return authorization;
        }

        #endregion

        #region Methods

        internal static string ToHex(byte[] data, bool lowercase)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString(lowercase ? "x2" : "X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        ///     URL encodes a string per RFC3986. If the path property is specified,
        ///     the accepted path characters {/+:} are not encoded.
        /// </summary>
        /// <param name="data">The string to encode</param>
        /// <param name="path">Whether the string is a URL path or not</param>
        /// <returns>The encoded string</returns>
        internal static string UrlEncode(string data, bool path)
        {
            return UrlEncode(3986, data, path);
        }

        /// <summary>
        ///     URL encodes a string per the specified RFC. If the path property is specified,
        ///     the accepted path characters {/+:} are not encoded.
        /// </summary>
        /// <param name="rfcNumber">RFC number determing safe characters</param>
        /// <param name="data">The string to encode</param>
        /// <param name="path">Whether the string is a URL path or not</param>
        /// <returns>The encoded string</returns>
        /// <remarks>
        ///     Currently recognised RFC versions are 1738 (Dec '94) and 3986 (Jan '05).
        ///     If the specified RFC is not recognised, 3986 is used by default.
        /// </remarks>
        internal static string UrlEncode(int rfcNumber, string data, bool path)
        {
            var encoded = new StringBuilder(data.Length * 2);
            string validUrlCharacters;
            if (!RFCEncodingSchemes.TryGetValue(rfcNumber, out validUrlCharacters))
            {
                validUrlCharacters = ValidUrlCharacters;
            }

            string unreservedChars = string.Concat(validUrlCharacters, path ? "/:" : string.Empty);

            foreach (char symbol in Encoding.UTF8.GetBytes(data))
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    encoded.Append(symbol);
                }
                else
                {
                    encoded.Append("%").Append(string.Format("{0:X2}", (int)symbol));
                }
            }

            return encoded.ToString();
        }

        private static string CreateHmacSignature(string data, string key, string algorithm)
        {
            KeyedHashAlgorithm hasher = null;
            algorithm = algorithm.ToLower();
            switch (algorithm)
            {
                case "vva-hmac-md5":
                    hasher = KeyedHashAlgorithm.Create("HMACMD5");
                    break;
                case "vva-hmac-sha1":
                    hasher = KeyedHashAlgorithm.Create("HMACSHA1");
                    break;
                case "vva-hmac-sha256":
                    hasher = KeyedHashAlgorithm.Create("HMACSHA256");
                    break;
                case "vva-hmac-sha384":
                    hasher = KeyedHashAlgorithm.Create("HMACSHA384");
                    break;
                case "vva-hmac-sha512":
                    hasher = KeyedHashAlgorithm.Create("HMACSHA512");
                    break;
                default:
                    throw new AuthenticationException("Invalid signature algorithm: " + algorithm);

                    break;
            }

            hasher.Key = Encoding.UTF8.GetBytes(key);

            byte[] output = hasher.ComputeHash(Encoding.UTF8.GetBytes(data));

            return ToHex(output, true);
        }

        private static string CreateUtf8(string input)
        {
            byte[] utf8Bytes = Encoding.UTF8.GetBytes(input);

            // Convert utf-8 bytes to a string.
            string output = Encoding.UTF8.GetString(utf8Bytes);

            return output;
        }

        private static string GetCanonicalHeaders(HttpRequestHeaders headers)
        {
            var sbHeader = new StringBuilder();
            var headersToSort = new Dictionary<string, string>();
            foreach (var httpRequestHeader in headers)
            {
                string key = httpRequestHeader.Key.ToLower().Trim();
                if (!key.StartsWith("x-vva-"))
                {
                    foreach (string value in httpRequestHeader.Value)
                    {
                        string newValue = value.Replace(Environment.NewLine, " ");

                        if (headersToSort.ContainsKey(key))
                        {
                            headersToSort[key] = headersToSort[key] + "," + newValue;
                        }
                        else
                        {
                            headersToSort.Add(key, newValue);
                        }
                    }
                }
            }

            IOrderedEnumerable<KeyValuePair<string, string>> sortedHeaders = headersToSort.OrderBy(c => c.Key);
            foreach (var sortedHeader in sortedHeaders)
            {
                sbHeader.Append(string.Format("{0}:{1}\n", sortedHeader.Key, sortedHeader.Value));
            }

            string output = sbHeader.ToString();
            if (output.EndsWith("\n"))
            {
                output = output.Substring(0, output.Length - 1);
            }

            return output;
        }

        private static string GetCanonicalQueryString(Uri target)
        {
            string result = string.Empty;

            var cresources = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(target.Query))
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(target.Query);
                if (nvc.HasKeys())
                {
                    var keys = new string[nvc.Keys.Count];
                    keys = nvc.AllKeys;
                    Array.Sort(keys);

                    var data = new StringBuilder(512);
                    foreach (string key in keys)
                    {
                        string value = nvc[key];
                        if (value != null)
                        {
                            data.Append(key);
                            data.Append('=');
                            data.Append(UrlEncode(value, false));
                            data.Append('&');
                        }
                    }

                    result = data.ToString();
                    if (result.Length > 0)
                    {
                        result = result.Remove(result.Length - 1);
                    }
                }
            }

            return result;
        }

        private static string GetCanonicalSignedHeaders(HttpRequestHeaders headers)
        {
            var sbHeader = new StringBuilder();
            var headersToSort = new List<string>();
            foreach (var httpRequestHeader in headers)
            {
                string key = httpRequestHeader.Key.ToLower().Trim();
                if (!key.StartsWith("x-vva-"))
                {
                    if (!headersToSort.Contains(key))
                    {
                        headersToSort.Add(key);
                    }
                }
            }

            headersToSort.Sort();
            foreach (string sortedHeader in headersToSort)
            {
                if (sbHeader.Length > 0)
                {
                    sbHeader.Append(string.Format(";{0}", sortedHeader));
                }
                else
                {
                    sbHeader.Append(string.Format("{0}", sortedHeader));
                }
            }

            return sbHeader.ToString();
        }

        private static string GetCanonicalUri(Uri target)
        {
            var result = new StringBuilder();
            string url = target.AbsolutePath;
            string bucket = url.Substring(url.IndexOf("/api/", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(bucket))
            {
                if (!bucket.StartsWith("/"))
                {
                    bucket = string.Format("/{0}", bucket);
                }

                if (bucket.EndsWith("/"))
                {
                    bucket = bucket.TrimEnd('/');
                }

                result.Append(bucket);
            }

            return result.ToString();
        }

        private static string HexEncode(string toEncodeBytes)
        {
            byte[] toEncode = Encoding.UTF8.GetBytes(toEncodeBytes);
            return ToHex(toEncode, true);
        }

        private static string HexEncodeHash(string data, string algorithm)
        {
            HashAlgorithm hashAlgorithm = null;

            algorithm = algorithm.ToLower();
            switch (algorithm)
            {
                case "vva-hmac-md5":
                    hashAlgorithm = HashAlgorithm.Create("MD5");
                    break;
                case "vva-hmac-sha1":
                    hashAlgorithm = HashAlgorithm.Create("SHA1");
                    break;
                case "vva-hmac-sha256":
                    hashAlgorithm = HashAlgorithm.Create("SHA256");
                    break;
                case "vva-hmac-sha384":
                    hashAlgorithm = HashAlgorithm.Create("SHA384");
                    break;
                case "vva-hmac-sha512":
                    hashAlgorithm = HashAlgorithm.Create("SHA512");
                    break;
                default:
                    throw new AuthenticationException("Invalid signature algorithm: " + algorithm);

                    break;
            }

            return ToHex(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(data)), true);
        }

        #endregion

        #endregion
    }
}