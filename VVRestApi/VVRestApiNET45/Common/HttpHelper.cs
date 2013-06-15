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
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Security.Authentication;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using VVRestApi.Common.Logging;
    using VVRestApi.Common.Messaging;

    public static class HttpHelper
    {
        #region Methods

        public static T Delete<T>(string virtualPath, string queryString, SessionToken token, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Delete(virtualPath, queryString, token, virtualPathArgs);
            return ConvertToRestTokenObject<T>(token, resultData);
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
            string signature = HttpHelper.CreateAuthorization(client.DefaultRequestHeaders, new Uri(url), "DELETE", string.Empty, token.DeveloperKey, token.DeveloperSecret);
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
        public static T Get<T>(string virtualPath, string queryString, bool expand, string fields, SessionToken token, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Get(virtualPath, queryString, expand, fields, token, virtualPathArgs);
            var result = ConvertToRestTokenObject<T>(token, resultData);

            return result;
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
        public static JObject Get(string virtualPath, string queryString, bool expand, string fields, SessionToken token, params object[] virtualPathArgs)
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = token.CreateUrl(string.Format(virtualPath, virtualPathArgs), queryString, HttpMethod.Get, fields, expand);
            string signature = HttpHelper.CreateAuthorization(client.DefaultRequestHeaders, new Uri(url), "GET", string.Empty, token.DeveloperKey, token.DeveloperSecret);
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

        public static void OutputCurlCommand(HttpClient client, HttpMethod method, string url, StringContent content, bool writeAllHeaders = false)
        {
            if (GlobalEvents.IsListening(LogLevelType.Debug))
            {
                StringBuilder sbCurl = new StringBuilder("CURL equivalent command: " + Environment.NewLine);
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
                    //if (writeAllHeaders)
                    //{
                    //    writeHeader = true;
                    //}
                    //else if (header.Key.StartsWith("x-", StringComparison.OrdinalIgnoreCase) || header.Key.StartsWith("Authorization"))
                    //{
                    //    writeHeader = true;
                    //}

                    if (writeHeader)
                    {
                        sbCurl.AppendFormat("\t-H \"{0}: {1}\" \\{2}", header.Key, header.Value.Aggregate((i, j) => i + " " + j), Environment.NewLine);
                    }

                }


                if (content != null)
                {
                    var jsonData = string.Empty;
                    Task task = content.ReadAsStringAsync().ContinueWith(async f =>
                        {
                            jsonData = f.Result;
                        });

                    task.Wait();

                    if (!String.IsNullOrWhiteSpace(jsonData))
                    {
                        sbCurl.AppendFormat("\t-d \'{0}\' \\ {1}", jsonData.Replace(Environment.NewLine, Environment.NewLine + "\t\t"), Environment.NewLine);
                    }
                }

                sbCurl.AppendLine(url);

                LogEventManager.Debug(sbCurl.ToString());
            }
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
        public static List<T> GetListResult<T>(string virtualPath, string queryString, bool expand, string fields, SessionToken token, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Get(virtualPath, queryString, expand, fields, token, virtualPathArgs);
            List<T> result = ConvertToRestTokenObjectList<T>(token, resultData);

            return result;
        }

        public static T Post<T>(string virtualPath, string queryString, SessionToken token, object postData, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Post(virtualPath, queryString, token, postData, virtualPathArgs);
            var result = ConvertToRestTokenObject<T>(token, resultData);

            return result;
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


            string signature = HttpHelper.CreateAuthorization(client.DefaultRequestHeaders, new Uri(url), "POST", jsonToPost, token.DeveloperKey, token.DeveloperSecret);
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
            string signature = HttpHelper.CreateAuthorization(client.DefaultRequestHeaders, new Uri(url), "PUT", jsonToPut, token.DeveloperKey, token.DeveloperSecret);
            client.DefaultRequestHeaders.Add("X-Authorization", signature);

            var content = new StringContent(jsonToPut);
            content.Headers.ContentType.MediaType = "application/json";

            OutputCurlCommand(client, HttpMethod.Put, url, null);


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
            T result = default(T);

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
            }

            return result;
        }

        private static List<T> ConvertToRestTokenObjectList<T>(SessionToken token, JObject resultData) where T : RestObject, new()
        {
            var result = new List<T>();

            if (resultData != null)
            {
                JToken dataNode = resultData["data"];
                if (dataNode != null)
                {
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
                //return string.Format("{0} to Url: {1}{3}{2}{3}Result JSON data: null", method, targetUrl, message, Environment.NewLine);
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
            //if (LogEventManager.IsListening(LogLevelType.Verbose))
            //{
            //    if (postData != null)
            //    {
            //        string jsonToPost = JsonConvert.SerializeObject(postData, GlobalConfiguration.GetJsonSerializerSettings());
            //        string message = string.Format("{0} to Url: {1}{3}{2}{3}JSON data for {0}: {4}", method, targetUrl, string.Empty, Environment.NewLine, jsonToPost);

            //        LogEventManager.Verbose(message);
            //    }
            //}
        }



        #endregion

        #region Authorization Signature

        private static string HexEncode(string toEncode)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
            var sb = new StringBuilder();
            for (int i = 0; i < toEncodeAsBytes.Length; i++)
            {
                sb.Append(toEncodeAsBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        private static string GetCanonicalQueryString(Uri target)
        {


            StringBuilder result = new StringBuilder();

            StringBuilder cresources = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(target.Query))
            {
                var nvc = System.Web.HttpUtility.ParseQueryString(target.Query);
                if (nvc.HasKeys())
                {
                    var items = new List<KeyValuePair<string, string>>();
                    foreach (var key in nvc.AllKeys)
                    {
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            items.Add(new KeyValuePair<string, string>(key.ToLowerInvariant(), nvc[key] ?? string.Empty));
                        }

                    }


                    //Sort by key and value
                    items.Sort((firstPair, nextPair) =>
                    {
                        var keyCompare = firstPair.Key.CompareTo(nextPair.Key);
                        if (keyCompare == 0)
                        {
                            //IF the keys are equal, compare the values
                            keyCompare = firstPair.Value.CompareTo(nextPair.Value);
                        }

                        return keyCompare;
                    });

                    Dictionary<string, string> itemsToSort = new Dictionary<string, string>();
                    foreach (var sortedItem in items)
                    {
                        string key = sortedItem.Key.ToLower().Trim();
                        string newValue = sortedItem.Value.ToLower().Trim().Replace(Environment.NewLine, " ");

                        if (itemsToSort.ContainsKey(key))
                        {
                            itemsToSort[key] = itemsToSort[key] + "," + newValue;
                        }
                        else
                        {
                            itemsToSort.Add(key, newValue);
                        }
                    }

                    var sortedItems = itemsToSort.OrderBy(c => c.Key);
                    foreach (var sortedItem in sortedItems)
                    {
                        if (cresources.Length > 0)
                        {
                            cresources.Append(string.Format(@"&\n{0}={1}", System.Web.HttpUtility.UrlDecode(sortedItem.Key), System.Web.HttpUtility.UrlDecode(sortedItem.Value)));
                        }
                        else
                        {
                            cresources.Append(string.Format(@"{0}={1}", System.Web.HttpUtility.UrlDecode(sortedItem.Key), System.Web.HttpUtility.UrlDecode(sortedItem.Value)));
                        }

                    }
                }
            }

            result.Append(cresources);

            return result.ToString();
        }

        private static string GetCanonicalUri(Uri target)
        {


            StringBuilder result = new StringBuilder();
            string url = target.AbsolutePath.ToLower();
            string bucket = url.Substring(url.IndexOf("/api/", System.StringComparison.OrdinalIgnoreCase));
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

        private static string GetCanonicalSignedHeaders(HttpRequestHeaders headers)
        {
            StringBuilder sbHeader = new StringBuilder();
            List<string> headersToSort = new List<string>();
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
            foreach (var sortedHeader in headersToSort)
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

        private static string GetCanonicalHeaders(HttpRequestHeaders headers)
        {
            StringBuilder sbHeader = new StringBuilder();
            Dictionary<string, string> headersToSort = new Dictionary<string, string>();
            foreach (var httpRequestHeader in headers)
            {
                string key = httpRequestHeader.Key.ToLower().Trim();
                if (!key.StartsWith("x-vva-"))
                {
                    foreach (var value in httpRequestHeader.Value)
                    {
                        string newValue = value.ToLower().Trim().Replace(Environment.NewLine, " ");

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

            var sortedHeaders = headersToSort.OrderBy(c => c.Key);
            foreach (var sortedHeader in sortedHeaders)
            {
                sbHeader.Append(string.Format("{0}:{1}\n", sortedHeader.Key, sortedHeader.Value));
            }

            return sbHeader.ToString();


        }

        public static string CreateAuthorization(HttpRequestHeaders headers, Uri target, string method, string data, string developerKey, string developerSecret)
        {
            string payloadHash = string.Empty;
            string requestDate = DateTime.UtcNow.ToString("o");
            headers.Add("X-VV-RequestDate", requestDate);

            string algorithm = "VVA-HMAC-SHA256";
            if (!string.IsNullOrEmpty(data))
            {
                payloadHash = CreateHmacHash(data, algorithm);
            }

            string canonicalHeaders = GetCanonicalHeaders(headers);
            string canonicalSignedHeaders = GetCanonicalSignedHeaders(headers);
            string canonicalQueryString = GetCanonicalQueryString(target);
            string canonicalUri = GetCanonicalUri(target);

            string canonicalRequest = string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", method, canonicalUri, canonicalQueryString, canonicalHeaders, canonicalSignedHeaders, payloadHash);
            string hashedCanonicalRequest = CreateHmacHash(canonicalRequest, algorithm);

            string stringToSign = string.Format("{0}\n{1}\n{2}\n{3}", algorithm, requestDate, developerKey, hashedCanonicalRequest);

            var nonce = Guid.NewGuid();
            var kSignature = CreateHmacSignature(CreateUtf8(nonce + developerKey), CreateUtf8(requestDate), algorithm);

            string signature = HexEncode(CreateHmacSignature(CreateUtf8(stringToSign), CreateUtf8(kSignature), algorithm));

            string authorization = String.Format("Algorithm={0}, Credential={1}, SignedHeaders={2}, Nonce={3}, Signature={4}", algorithm, developerKey, canonicalSignedHeaders, nonce, signature);

            return authorization;
        }

        private static string CreateUtf8(string input)
        {
            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(input);

            // Convert utf-8 bytes to a string.
            string output = System.Text.Encoding.UTF8.GetString(utf8Bytes);

            return output;
        }

        private static string CreateHmacSignature(string data, string key, string algorithm)
        {
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            HMAC hasher = null;
            algorithm = algorithm.ToLower();
            switch (algorithm)
            {
                case "vva-hmac-md5":
                    hasher = new HMACMD5(keyBytes);
                    break;
                case "vva-hmac-ripemd160":
                    hasher = new HMACRIPEMD160(keyBytes);
                    break;
                case "vva-hmac-sha1":
                    hasher = new HMACSHA1(keyBytes);
                    break;
                case "vva-hmac-sha256":
                    hasher = new HMACSHA256(keyBytes);
                    break;
                case "vva-hmac-sha384":
                    hasher = new HMACSHA384(keyBytes);
                    break;
                case "vva-hmac-sha512":
                    hasher = new HMACSHA512(keyBytes);
                    break;
                default:
                    throw new AuthenticationException("Invalid signature algorithm: " + algorithm);

                    break;
            }


            byte[] inputBytes = Encoding.ASCII.GetBytes(data);
            byte[] output = hasher.ComputeHash(inputBytes);

            hasher.Clear();
            return Convert.ToBase64String(output);
        }

        private static string CreateHmacHash(string input, string algorithm)
        {
            // Use input string to calculate MD5 hash
            algorithm = algorithm.ToLower();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = new byte[] { };

            switch (algorithm)
            {
                case "vva-hmac-md5":
                    hashBytes = MD5.Create().ComputeHash(inputBytes);
                    break;
                case "vva-hmac-ripemd160":
                    hashBytes = RIPEMD160.Create().ComputeHash(inputBytes);
                    break;
                case "vva-hmac-sha1":
                    hashBytes = SHA1.Create().ComputeHash(inputBytes);
                    break;
                case "vva-hmac-sha256":
                    hashBytes = SHA256.Create().ComputeHash(inputBytes);
                    break;
                case "vva-hmac-sha384":
                    hashBytes = SHA384.Create().ComputeHash(inputBytes);
                    break;
                case "vva-hmac-sha512":
                    hashBytes = SHA512.Create().ComputeHash(inputBytes);
                    break;
                default:
                    throw new AuthenticationException("Invalid signature algorithm: " + algorithm);
                    break;
            }


            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
                // To force the hex string to upper-case letters instead of
                // lower-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("X2")); 
            }
            return sb.ToString();
        }

        #endregion
    }
}