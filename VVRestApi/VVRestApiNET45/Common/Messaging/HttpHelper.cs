// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpHelper.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VVRestApi.Common.Logging;

namespace VVRestApi.Common.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpHelper
    {
        #region HTTP Requests (GET,POST,PUT,DELETE)

        /// <summary>
        /// Calls a public endpoint with no access token
        /// </summary>
        /// <returns></returns>
        public static T GetPublicNoCustomerAliases<T>(string virtualPath, string queryString, UrlParts urlParts, params object[] virtualPathArgs) where T : RestObject, new()
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), queryString, "", false, false);

            OutputCurlCommand(client, HttpMethod.Get, url, null);

            //var task = client.GetStringAsync(url).ContinueWith(taskWithResponse =>
            //{
            //    resultData = taskWithResponse.Result;
            //});

            //Task task = client.GetAsync(url).ContinueWith(async taskwithresponse =>
            //{
            //    try
            //    {
            //        resultData = await taskwithresponse.Result.Content.ReadAsStringAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        HandleTaskException(taskwithresponse, ex, HttpMethod.Get);
            //    }
            //});

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

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


            T resultEntity = default(T);

            if (resultData != null)
            {

                JToken dataNode = resultData["data"];
                if (dataNode != null)
                {
                    if (dataNode.Type == JTokenType.Array)
                    {
                        if (dataNode.First != null)
                        {
                            resultEntity = JsonConvert.DeserializeObject<T>(dataNode.First.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                            if (resultEntity != null)
                            {
                                //resultEntity.PopulateAccessToken(clientSecrets, apiTokens);
                                resultEntity.Meta = resultData["meta"].ToObject<ApiMetaData>();
                                resultEntity.PopulateData(dataNode);
                            }
                        }
                    }
                    else
                    {
                        resultEntity = JsonConvert.DeserializeObject<T>(dataNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                        if (resultEntity != null)
                        {
                            //resultEntity.PopulateAccessToken(clientSecrets, apiTokens);
                            resultEntity.Meta = resultData["meta"].ToObject<ApiMetaData>();
                        }
                    }
                }
                else
                {
                    JToken metaNode = resultData["meta"];
                    if (metaNode != null)
                    {
                        LogEventManager.Error(string.Format("No data returned: {0}{1}", Environment.NewLine, metaNode));

                    }
                }
            }
            
            return resultEntity;
        }

        public static JObject GetPublicNoCustomerAliases(string virtualPath, string queryString, UrlParts urlParts, params object[] virtualPathArgs)
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), queryString, "", false, false);

            OutputCurlCommand(client, HttpMethod.Get, url, null);

            //var task = client.GetStringAsync(url).ContinueWith(taskWithResponse =>
            //{
            //    resultData = taskWithResponse.Result;
            //});

            //Task task = client.GetAsync(url).ContinueWith(async taskwithresponse =>
            //{
            //    try
            //    {
            //        resultData = await taskwithresponse.Result.Content.ReadAsStringAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        HandleTaskException(taskwithresponse, ex, HttpMethod.Get);
            //    }
            //});

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            Task task = client.GetAsync(url).ContinueWith(async taskwithresponse =>
            {
                try
                {
                    JObject result = await taskwithresponse.Result.Content.ReadAsAsync<JObject>();
                    resultData = ProcessResultData(result, url, HttpMethod.Get);
                    resultData = ProcessResultData(result, url, HttpMethod.Get);
                }
                catch (Exception ex)
                {
                    HandleTaskException(taskwithresponse, ex, HttpMethod.Get);
                }
            });

            task.Wait();


            

            if (resultData != null)
            {

                JToken dataNode = resultData["data"];
                if (dataNode != null)
                {
                    //if (dataNode.Type == JTokenType.Array)
                    //{
                    //    if (dataNode.First != null)
                    //    {
                    //        resultEntity = JsonConvert.DeserializeObject<T>(dataNode.First.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                    //        if (resultEntity != null)
                    //        {
                    //            //resultEntity.PopulateAccessToken(clientSecrets, apiTokens);
                    //            resultEntity.Meta = resultData["meta"].ToObject<ApiMetaData>();
                    //            resultEntity.PopulateData(dataNode);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    resultEntity = JsonConvert.DeserializeObject<T>(dataNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                    //    if (resultEntity != null)
                    //    {
                    //        //resultEntity.PopulateAccessToken(clientSecrets, apiTokens);
                    //        resultEntity.Meta = resultData["meta"].ToObject<ApiMetaData>();
                    //    }
                    //}
                }
                else
                {
                    JToken metaNode = resultData["meta"];
                    if (metaNode != null)
                    {
                        LogEventManager.Error(string.Format("No data returned: {0}{1}", Environment.NewLine, metaNode));

                    }
                }
            }

            return resultData;
        }

        /// <summary>
        /// HTTP GET with Authorization Header, Querystring, and field options.
        /// GET returns JSON or XML data depending upon the ContentType HTTP Header.
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="options"> </param>
        /// <param name="urlParts"> </param>
        /// <param name="apiTokens">The OAuth Access token.</param>
        /// <param name="clientSecrets">Client secrets needed if refresh necessary.</param>
        /// <param name="virtualPathArgs">The arguments to replace the tokens ({0},{1}, etc.) in the virtual path</param>
        /// <returns></returns>
        public static JObject Get(string virtualPath, string queryString, RequestOptions options, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, params object[] virtualPathArgs)
        {
            if (options == null)
            {
                options = new RequestOptions();
            }

            options.PrepForRequest();

            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), options.GetQueryString(queryString), options.Fields, options.Expand);

            if (apiTokens.AccessTokenExpiration < DateTime.UtcNow)
            {
                apiTokens = RefreshAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret, apiTokens.RefreshToken).Result;
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiTokens.AccessToken);

            OutputCurlCommand(client, HttpMethod.Get, url, null);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

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
        /// HTTP GET with Authorization Header, Querystring, and field options.
        /// GET returns JSON or XML data depending upon the ContentType HTTP Header.
        /// </summary>
        public static JObject Get(string virtualPath, string queryString, RequestOptions options, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, bool includeAliases, params object[] virtualPathArgs)
        {
            if (options == null)
            {
                options = new RequestOptions();
            }

            options.PrepForRequest();

            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = "";
            if (virtualPathArgs == null)
            {
                url = CreateUrl(urlParts, virtualPath, options.GetQueryString(queryString), options.Fields, options.Expand, includeAliases);
            }
            else
            {
                url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), options.GetQueryString(queryString), options.Fields, options.Expand, includeAliases);
            }

            if (apiTokens.AccessTokenExpiration < DateTime.UtcNow)
            {
                apiTokens = RefreshAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret, apiTokens.RefreshToken).Result;
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiTokens.AccessToken);

            OutputCurlCommand(client, HttpMethod.Get, url, null);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

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
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="options"></param>
        /// <param name="urlParts"></param>
        /// <param name="apiTokens"></param>
        /// <param name="clientSecrets"></param>
        /// <param name="virtualPathArgs"></param>
        /// <returns></returns>
        public static Stream GetStream(string virtualPath, string queryString, RequestOptions options, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, params object[] virtualPathArgs)
        {
            if (options == null)
            {
                options = new RequestOptions();
            }

            options.PrepForRequest();

            var client = new HttpClient();

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), options.GetQueryString(queryString), options.Fields, options.Expand);

            if (apiTokens.AccessTokenExpiration < DateTime.UtcNow)
            {
                apiTokens = RefreshAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret, apiTokens.RefreshToken).Result;
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiTokens.AccessToken);

            OutputCurlCommand(client, HttpMethod.Get, url, null);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            Stream stream = client.GetStreamAsync(url).Result;

            return stream;
        }

        /// <summary>
        /// HTTP POST with Authorization Header and JSON body. POST verb is used to Insert data.
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="urlParts"> </param>
        /// <param name="apiTokens">The OAuth Access token.</param>
        /// <param name="postData">The data to post.</param>
        /// <param name="virtualPathArgs">The parameters to replace tokens in the virtualPath with.</param>
        /// <returns></returns>
        public static JObject Post(string virtualPath, string queryString, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, object postData, params object[] virtualPathArgs)
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), queryString);

            string jsonToPost = string.Empty;
            if (postData != null)
            {
                jsonToPost = JsonConvert.SerializeObject(postData, GlobalConfiguration.GetJsonSerializerSettings());
            }

            if (apiTokens.AccessTokenExpiration < DateTime.UtcNow)
            {
                apiTokens = RefreshAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret, apiTokens.RefreshToken).Result;
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiTokens.AccessToken);

            var content = new StringContent(jsonToPost);
            content.Headers.ContentType.MediaType = "application/json";

            OutputCurlCommand(client, HttpMethod.Post, url, content);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

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

        /// <summary>
        /// HTTP Multipart POST with Authorization Header and JSON body. Post a byte array and Form data in body.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="urlParts"></param>
        /// <param name="apiTokens"></param>
        /// <param name="postData"></param>
        /// <param name="filename"></param>
        /// <param name="file"></param>
        /// <param name="virtualPathArgs"></param>
        /// <returns></returns>
        public static JObject PostMultiPart(string virtualPath, string queryString, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, List<KeyValuePair<string, string>> postData, string filename, byte[] file, params object[] virtualPathArgs)
        {
            using (var client = new HttpClient())
            {
                            
                JObject resultData = null;

                CleanupVirtualPathArgs(virtualPathArgs);
                string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), queryString);

                if (apiTokens.AccessTokenExpiration < DateTime.UtcNow)
                {
                    apiTokens = RefreshAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret, apiTokens.RefreshToken).Result;
                }

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiTokens.AccessToken);

                using (var multiPartContent = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    if (postData != null)
                    {
                        var jsonToPost = JsonConvert.SerializeObject(postData, GlobalConfiguration.GetJsonSerializerSettings());
                        var content = new StringContent(jsonToPost);
                        OutputCurlCommand(client, HttpMethod.Post, url, content);

                        foreach (KeyValuePair<string, string> keyValuePair in postData)
                        {
                            multiPartContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                        }
                    }

                    multiPartContent.Add(new StreamContent(new MemoryStream(file)), "fileupload", filename);

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                    Task task = client.PostAsync(url, multiPartContent).ContinueWith(async taskwithresponse =>
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
            }
        }

        /// <summary>
        /// HTTP Multipart POST with Authorization Header and JSON body. Post a file stream and Form data in body.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="urlParts"></param>
        /// <param name="apiTokens"></param>
        /// <param name="postData"></param>
        /// <param name="filename"></param>
        /// <param name="fileStream"></param>
        /// <param name="virtualPathArgs"></param>
        /// <returns></returns>
        public static JObject PostMultiPart(string virtualPath, string queryString, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, List<KeyValuePair<string, string>> postData, string filename, Stream fileStream, params object[] virtualPathArgs)
        {
            using (var client = new HttpClient())
            {

                JObject resultData = null;

                CleanupVirtualPathArgs(virtualPathArgs);
                string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), queryString);

                if (apiTokens.AccessTokenExpiration < DateTime.UtcNow)
                {
                    apiTokens = RefreshAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret, apiTokens.RefreshToken).Result;
                }

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiTokens.AccessToken);

                using (var multiPartContent = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    if (postData != null)
                    {
                        var jsonToPost = JsonConvert.SerializeObject(postData, GlobalConfiguration.GetJsonSerializerSettings());
                        var content = new StringContent(jsonToPost);
                        OutputCurlCommand(client, HttpMethod.Post, url, content);

                        foreach (KeyValuePair<string, string> keyValuePair in postData)
                        {
                            multiPartContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                        }
                    }

                    multiPartContent.Add(new StreamContent(fileStream), "fileupload", filename);

                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                    Task task = client.PostAsync(url, multiPartContent).ContinueWith(async taskwithresponse =>
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
            }
        }


        /// <summary>
        /// Post data to public endpoint anonymously
        /// </summary>
        public static T PostPublicNoCustomerAliases<T>(string virtualPath, string queryString, UrlParts urlParts, object postData, params object[] virtualPathArgs) where T : RestObject, new()
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), queryString, "", false, false);

            string jsonToPost = string.Empty;
            if (postData != null)
            {
                jsonToPost = JsonConvert.SerializeObject(postData, GlobalConfiguration.GetJsonSerializerSettings());
            }


            var content = new StringContent(jsonToPost);
            content.Headers.ContentType.MediaType = "application/json";

            OutputCurlCommand(client, HttpMethod.Post, url, content);

            //var task = client.GetStringAsync(url).ContinueWith(taskWithResponse =>
            //{
            //    resultData = taskWithResponse.Result;
            //});

            //Task task = client.GetAsync(url).ContinueWith(async taskwithresponse =>
            //{
            //    try
            //    {
            //        resultData = await taskwithresponse.Result.Content.ReadAsStringAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        HandleTaskException(taskwithresponse, ex, HttpMethod.Get);
            //    }
            //});

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            Task task = client.PostAsync(url, content).ContinueWith(async taskwithresponse =>
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


            T resultEntity = default(T);

            if (resultData != null)
            {

                JToken dataNode = resultData["data"];
                if (dataNode != null)
                {
                    if (dataNode.Type == JTokenType.Array)
                    {
                        if (dataNode.First != null)
                        {
                            resultEntity = JsonConvert.DeserializeObject<T>(dataNode.First.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                            if (resultEntity != null)
                            {
                                //resultEntity.PopulateAccessToken(clientSecrets, apiTokens);
                                resultEntity.Meta = resultData["meta"].ToObject<ApiMetaData>();
                                resultEntity.PopulateData(dataNode);
                            }
                        }
                    }
                    else
                    {
                        resultEntity = JsonConvert.DeserializeObject<T>(dataNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                        if (resultEntity != null)
                        {
                            //resultEntity.PopulateAccessToken(clientSecrets, apiTokens);
                            resultEntity.Meta = resultData["meta"].ToObject<ApiMetaData>();
                        }
                    }
                }
                else
                {
                    JToken metaNode = resultData["meta"];
                    if (metaNode != null)
                    {
                        LogEventManager.Error(string.Format("No data returned: {0}{1}", Environment.NewLine, metaNode));

                    }
                }
            }

            return resultEntity;
        }

        public static JObject PostPublicNoCustomerAliases(string virtualPath, string queryString, UrlParts urlParts, object postData, params object[] virtualPathArgs)
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), queryString, "", false, false);

            string jsonToPost = string.Empty;
            if (postData != null)
            {
                jsonToPost = JsonConvert.SerializeObject(postData, GlobalConfiguration.GetJsonSerializerSettings());
            }


            var content = new StringContent(jsonToPost);
            content.Headers.ContentType.MediaType = "application/json";

            OutputCurlCommand(client, HttpMethod.Post, url, content);

            //var task = client.GetStringAsync(url).ContinueWith(taskWithResponse =>
            //{
            //    resultData = taskWithResponse.Result;
            //});

            //Task task = client.GetAsync(url).ContinueWith(async taskwithresponse =>
            //{
            //    try
            //    {
            //        resultData = await taskwithresponse.Result.Content.ReadAsStringAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        HandleTaskException(taskwithresponse, ex, HttpMethod.Get);
            //    }
            //});

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            Task task = client.PostAsync(url, content).ContinueWith(async taskwithresponse =>
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


            if (resultData != null)
            {

                JToken dataNode = resultData["data"];
                if (dataNode != null)
                {
                    //if (dataNode.Type == JTokenType.Array)
                    //{
                    //    if (dataNode.First != null)
                    //    {
                    //        resultEntity = JsonConvert.DeserializeObject<T>(dataNode.First.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                    //        if (resultEntity != null)
                    //        {
                    //            //resultEntity.PopulateAccessToken(clientSecrets, apiTokens);
                    //            resultEntity.Meta = resultData["meta"].ToObject<ApiMetaData>();
                    //            resultEntity.PopulateData(dataNode);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    resultEntity = JsonConvert.DeserializeObject<T>(dataNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                    //    if (resultEntity != null)
                    //    {
                    //        //resultEntity.PopulateAccessToken(clientSecrets, apiTokens);
                    //        resultEntity.Meta = resultData["meta"].ToObject<ApiMetaData>();
                    //    }
                    //}
                }
                else
                {
                    JToken metaNode = resultData["meta"];
                    if (metaNode != null)
                    {
                        //resultEntity = JsonConvert.DeserializeObject<ApiMetaData>(metaNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());
                        LogEventManager.Error(string.Format("No data returned: {0}{1}", Environment.NewLine, metaNode));

                    }
                }
            }

            return resultData;
        }

        public static JObject PutPublicNoCustomerAliases(string virtualPath, string queryString, UrlParts urlParts, Tokens apiTokens, object postData, params object[] virtualPathArgs)
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), queryString, "", false, false);

            string jsonToPost = string.Empty;
            if (postData != null)
            {
                jsonToPost = JsonConvert.SerializeObject(postData, GlobalConfiguration.GetJsonSerializerSettings());
            }


            var content = new StringContent(jsonToPost);
            content.Headers.ContentType.MediaType = "application/json";

            OutputCurlCommand(client, HttpMethod.Put, url, content);

            //var task = client.GetStringAsync(url).ContinueWith(taskWithResponse =>
            //{
            //    resultData = taskWithResponse.Result;
            //});

            //Task task = client.GetAsync(url).ContinueWith(async taskwithresponse =>
            //{
            //    try
            //    {
            //        resultData = await taskwithresponse.Result.Content.ReadAsStringAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        HandleTaskException(taskwithresponse, ex, HttpMethod.Get);
            //    }
            //});

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            Task task = client.PutAsync(url, content).ContinueWith(async taskwithresponse =>
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


            if (resultData != null)
            {

                JToken dataNode = resultData["data"];
                if (dataNode != null)
                {
                    //if (dataNode.Type == JTokenType.Array)
                    //{
                    //    if (dataNode.First != null)
                    //    {
                    //        resultEntity = JsonConvert.DeserializeObject<T>(dataNode.First.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                    //        if (resultEntity != null)
                    //        {
                    //            //resultEntity.PopulateAccessToken(clientSecrets, apiTokens);
                    //            resultEntity.Meta = resultData["meta"].ToObject<ApiMetaData>();
                    //            resultEntity.PopulateData(dataNode);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    resultEntity = JsonConvert.DeserializeObject<T>(dataNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                    //    if (resultEntity != null)
                    //    {
                    //        //resultEntity.PopulateAccessToken(clientSecrets, apiTokens);
                    //        resultEntity.Meta = resultData["meta"].ToObject<ApiMetaData>();
                    //    }
                    //}
                }
                else
                {
                    JToken metaNode = resultData["meta"];
                    if (metaNode != null)
                    {
                        LogEventManager.Error(string.Format("No data returned: {0}{1}", Environment.NewLine, metaNode));

                    }
                }
            }

            return resultData;
        }


        /// <summary>
        /// HTTP PUT with Authorization Header and JSON body. PUT verb is used to update data.
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="urlParts"> </param>
        /// <param name="apiTokens">The OAuth Access token.</param>
        /// <param name="postData">The data to post.</param>
        /// <param name="virtualPathArgs">The parameters to replace tokens in the virtualPath with.</param>
        /// <returns></returns>
        public static JObject Put(string virtualPath, string queryString, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, object postData, params object[] virtualPathArgs)
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), queryString);

            string jsonToPut = string.Empty;
            if (postData != null)
            {
                jsonToPut = JsonConvert.SerializeObject(postData, GlobalConfiguration.GetJsonSerializerSettings());
            }

            if (apiTokens.AccessTokenExpiration < DateTime.UtcNow)
            {
                apiTokens = RefreshAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret, apiTokens.RefreshToken).Result;
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiTokens.AccessToken);

            var content = new StringContent(jsonToPut);
            content.Headers.ContentType.MediaType = "application/json";

            OutputCurlCommand(client, HttpMethod.Put, url, content);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

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

        public static JObject Put(string virtualPath, string queryString, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, List<KeyValuePair<string, string>> postData, params object[] virtualPathArgs)
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), queryString);

            string jsonToPut = string.Empty;
            if (postData == null)
            {
                postData = new List<KeyValuePair<string, string>>();
                //jsonToPut = JsonConvert.SerializeObject(postData, GlobalConfiguration.GetJsonSerializerSettings());
            }

            if (apiTokens.AccessTokenExpiration < DateTime.UtcNow)
            {
                apiTokens = RefreshAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret, apiTokens.RefreshToken).Result;
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiTokens.AccessToken);

            //var content = new StringContent(jsonToPut);
            //content.Headers.ContentType.MediaType = "application/json";

            var formContent = new FormUrlEncodedContent(postData);

            //OutputCurlCommand(client, HttpMethod.Put, url, formContent);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            Task task = client.PutAsync(url, formContent).ContinueWith(async taskwithresponse =>
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



        /// <summary>
        /// HTTP DELETE with Authorization Header
        /// </summary>
        /// <param name="virtualPath">The virtual path, with optional format tokens: ~/SomeEndpoint/{0}/</param>
        /// <param name="queryString"></param>
        /// <param name="urlParts"> </param>
        /// <param name="apiTokens">The OAuth Access token.</param>
        /// <param name="virtualPathArgs"></param>
        /// <returns></returns>
        public static JObject Delete(string virtualPath, string queryString, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, params object[] virtualPathArgs)
        {
            var client = new HttpClient();

            JObject resultData = null;

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), queryString);

            if (apiTokens.AccessTokenExpiration < DateTime.UtcNow)
            {
                apiTokens = RefreshAccessToken(clientSecrets.OAuthTokenEndPoint, clientSecrets.ApiKey, clientSecrets.ApiSecret, apiTokens.RefreshToken).Result;
            }

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiTokens.AccessToken);

            OutputCurlCommand(client, HttpMethod.Delete, url, null);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

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

        #endregion

        #region OAuth Access/Refresh Tokens

        /// <summary>
        /// Authenticates with OAuth Authorization Server token end point using OAuth2 Client Credentials Grant Type
        /// </summary>
        /// <param name="oauthTokenEndPoint">OAuth2 Authorization Server Access/Refresh Token URL</param>
        /// <param name="apiKey">Issued by VisualVault, used to identify the client application/developer</param>
        /// <param name="apiSecret">Issued by VisualVault, used to authenticate the client application/developer</param>
        /// <returns></returns>
        public static async Task<Tokens> GetAccessToken(string oauthTokenEndPoint, string apiKey, string apiSecret)
        {
            var client = new HttpClient();

            var post = new Dictionary<string, string>
                           {
                               {"client_id", apiKey},
                               {"client_secret", apiSecret},
                               {"username", apiKey},
                               {"password", apiSecret},
                               {"grant_type", "client_credentials"}
                           };

            //string jsonToPost = JsonConvert.SerializeObject(post, GlobalConfiguration.GetJsonSerializerSettings());

            //var postContent = new StringContent(jsonToPost);
            //postContent.Headers.ContentType.MediaType = "application/json";

            //OutputCurlCommand(client, HttpMethod.Post, oauthTokenEndPoint, postContent);

            //var response = await client.PostAsync(oauthTokenEndPoint, new FormUrlEncodedContent(post));

            //var content = await response.Content.ReadAsStringAsync();

            

            var jsonToPost = JsonConvert.SerializeObject(post, GlobalConfiguration.GetJsonSerializerSettings());
            var content = new StringContent(jsonToPost);
            content.Headers.ContentType.MediaType = "application/json";
            OutputCurlCommand(client, HttpMethod.Post, oauthTokenEndPoint, content);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            JObject resultData = null;
            Task task = client.PostAsync(oauthTokenEndPoint, new FormUrlEncodedContent(post)).ContinueWith(async taskwithresponse =>
            {
                try
                {
                    JObject result = await taskwithresponse.Result.Content.ReadAsAsync<JObject>();
                    resultData = ProcessResultData(result, oauthTokenEndPoint, HttpMethod.Get);
                }
                catch (Exception ex)
                {
                    HandleTaskException(taskwithresponse, ex, HttpMethod.Get);
                }
            });
            task.Wait();

            Tokens apiTokens = new Tokens
            {
                AccessToken = resultData["access_token"].ToString(),
                RefreshToken = resultData["refresh_token"].ToString()
            };

            if (!string.IsNullOrEmpty(resultData["expires_in"].ToString()))
            {
                double expiration;
                if (double.TryParse(resultData["expires_in"].ToString(), out expiration))
                {
                    apiTokens.AccessTokenExpiration = DateTime.UtcNow.AddSeconds(expiration);
                }
            }

            return apiTokens;
        }

        /// <summary>
        /// Authenticates with OAuth Authorization Server token end point using OAuth2 Resource Owner Grant Type
        /// </summary>
        /// <param name="oauthTokenEndPoint">OAuth2 Authorization Server Access/Refresh Token URL</param>
        /// <param name="apiKey">Issued by VisualVault, used to identify the client application/developer</param>
        /// <param name="apiSecret">Issued by VisualVault, used to authenticate the client application/developer</param>
        /// <param name="userName">Resource Owner's user id.  Resource Owner is a user providing their credentials to a trusted client application.</param>
        /// <param name="password">Resource Owner's password</param>
        /// <returns></returns>
        public static async Task<Tokens> GetAccessToken(string oauthTokenEndPoint, string apiKey, string apiSecret, string userName, string password)
        {

            //uncomment to force request through proxy for debugging
            //var httpClientHandler = new HttpClientHandler
            //{
            //    Proxy = new WebProxy("http://localhost:8888", false),
            //    UseProxy = true
            //};

            //var client = new HttpClient(httpClientHandler);

            var client = new HttpClient();

            var post = new Dictionary<string, string>
                           {
                               {"client_id", apiKey},
                               {"client_secret", apiSecret},
                               {"username", userName},
                               {"password", password},
                               {"grant_type", "password"}
                           };

            //string jsonToPost = JsonConvert.SerializeObject(post, GlobalConfiguration.GetJsonSerializerSettings());

            //var postContent = new StringContent(jsonToPost);
            //postContent.Headers.ContentType.MediaType = "application/json";

            //OutputCurlCommand(client, HttpMethod.Post, oauthTokenEndPoint, postContent);

            //var response = await client.PostAsync(oauthTokenEndPoint, new FormUrlEncodedContent(post));

            //var content = await response.Content.ReadAsStringAsync();

            //var json = JObject.Parse(content);

            
            var jsonToPost = JsonConvert.SerializeObject(post, GlobalConfiguration.GetJsonSerializerSettings());
            var content = new StringContent(jsonToPost);
            content.Headers.ContentType.MediaType = "application/json";
            OutputCurlCommand(client, HttpMethod.Post, oauthTokenEndPoint, content);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            JObject resultData = null;
            Task task = client.PostAsync(oauthTokenEndPoint, new FormUrlEncodedContent(post)).ContinueWith(async taskwithresponse =>
            {
                try
                {
                    JObject result = await taskwithresponse.Result.Content.ReadAsAsync<JObject>();
                    resultData = ProcessResultData(result, oauthTokenEndPoint, HttpMethod.Get);
                }
                catch (Exception ex)
                {
                    HandleTaskException(taskwithresponse, ex, HttpMethod.Get);
                }
            });
            task.Wait();



            Tokens apiTokens = new Tokens
            {
                AccessToken = resultData["access_token"].ToString(),
                RefreshToken = resultData["refresh_token"].ToString()
            };

            if (!string.IsNullOrEmpty(resultData["expires_in"].ToString()))
            {
                double expiration;
                if (double.TryParse(resultData["expires_in"].ToString(), out expiration))
                {
                    apiTokens.AccessTokenExpiration = DateTime.UtcNow.AddSeconds(expiration);
                }
            }

            return apiTokens;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oauthTokenEndPoint">OAuth2 Authorization Server Access/Refresh Token URL</param>
        /// <param name="apiKey">Issued by VisualVault, used to identify the client application/developer</param>
        /// <param name="apiSecret">Issued by VisualVault, used to authenticate the client application/developer</param>
        /// <param name="refreshToken">Refresh token provided in the response from the token end point.  Refresh token has a longer life than the access token and may be used to request a new access token without providing credentials</param>
        /// <returns></returns>
        public static async Task<Tokens> RefreshAccessToken(string oauthTokenEndPoint, string apiKey, string apiSecret, string refreshToken)
        {
            var client = new HttpClient();

            var post = new Dictionary<string, string>
                {
                    {"client_id", apiKey},
                    {"client_secret", apiSecret},
                    {"refresh_token", refreshToken},
                    {"grant_type", "refresh_token"}
                };

            //var data = new StringContent(post.ToString());
            //OutputCurlCommand(client, HttpMethod.Post, oauthTokenEndPoint, data);

            //var response = await client.PostAsync(oauthTokenEndPoint, new FormUrlEncodedContent(post));

            //var content = await response.Content.ReadAsStringAsync();

            //var json = JObject.Parse(content);


            var jsonToPost = JsonConvert.SerializeObject(post, GlobalConfiguration.GetJsonSerializerSettings());
            var content = new StringContent(jsonToPost);
            content.Headers.ContentType.MediaType = "application/json";
            OutputCurlCommand(client, HttpMethod.Post, oauthTokenEndPoint, content);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            JObject resultData = null;
            Task task = client.PostAsync(oauthTokenEndPoint, new FormUrlEncodedContent(post)).ContinueWith(async taskwithresponse =>
            {
                try
                {
                    JObject result = await taskwithresponse.Result.Content.ReadAsAsync<JObject>();
                    resultData = ProcessResultData(result, oauthTokenEndPoint, HttpMethod.Get);
                }
                catch (Exception ex)
                {
                    HandleTaskException(taskwithresponse, ex, HttpMethod.Get);
                }
            });
            task.Wait();


            Tokens apiTokens = new Tokens
            {
                AccessToken = resultData["access_token"].ToString(),
                RefreshToken = resultData["refresh_token"].ToString()
            };

            if (!string.IsNullOrEmpty(resultData["expires_in"].ToString()))
            {
                double expiration;
                if (double.TryParse(resultData["expires_in"].ToString(), out expiration))
                {
                    apiTokens.AccessTokenExpiration = DateTime.UtcNow.AddSeconds(expiration);
                }
            }

            return apiTokens;
        }

        #endregion

        #region HTTP Helper Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="clientSecrets"> </param>
        /// <param name="urlParts"> </param>
        /// <param name="apiTokens"> </param>
        /// <param name="virtualPathArgs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Delete<T>(string virtualPath, string queryString, IClientSecrets clientSecrets, UrlParts urlParts, Tokens apiTokens, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Delete(virtualPath, queryString, urlParts, apiTokens, clientSecrets, virtualPathArgs);
            return ConvertToRestTokenObject<T>(clientSecrets, apiTokens, resultData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="urlParts"> </param>
        /// <param name="queryString"></param>
        /// <param name="apiTokens">The OAuth Access token.</param>
        /// <param name="virtualPathArgs"></param>
        /// <returns></returns>
        public static ApiMetaData DeleteReturnMeta(string virtualPath, string queryString, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, params object[] virtualPathArgs)
        {
            JObject resultData = Delete(virtualPath, queryString, urlParts, apiTokens, clientSecrets, virtualPathArgs);

            return ConvertRestResponseToApiMetaData(resultData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="clientSecrets"> </param>
        /// <param name="apiTokens"> </param>
        /// <param name="virtualPathArgs"></param>
        /// <param name="urlParts"> </param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> DeleteListResult<T>(string virtualPath, string queryString, UrlParts urlParts, IClientSecrets clientSecrets, Tokens apiTokens, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Delete(virtualPath, queryString, urlParts, apiTokens, clientSecrets, virtualPathArgs);

            return ConvertToRestTokenObjectList<T>(clientSecrets, apiTokens, resultData);
        }

        /// <summary>
        ///     GET the virtual path with the querystring and field options
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="options"> </param>
        /// <param name="urlParts"> </param>
        /// <param name="clientSecrets"> </param>
        /// <param name="apiTokens">The OAuth Access token.</param>
        /// <param name="virtualPathArgs">The arguments to replace the tokens ({0},{1}, etc.) in the virtual path</param>
        /// <returns></returns>
        public static T Get<T>(string virtualPath, string queryString, RequestOptions options, UrlParts urlParts, IClientSecrets clientSecrets, Tokens apiTokens, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Get(virtualPath, queryString, options, urlParts, apiTokens, clientSecrets, virtualPathArgs);
            var result = ConvertToRestTokenObject<T>(clientSecrets, apiTokens, resultData);

            return result;
        }

        public static T Get<T>(string virtualPath, string queryString, RequestOptions options, UrlParts urlParts, IClientSecrets clientSecrets, Tokens apiTokens, bool includeAliases, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Get(virtualPath, queryString, options, urlParts, apiTokens, clientSecrets, includeAliases, virtualPathArgs);
            var result = ConvertToRestTokenObject<T>(clientSecrets, apiTokens, resultData);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="options"></param>
        /// <param name="urlParts"> </param>
        /// <param name="apiTokens">The OAuth Access token.</param>
        /// <param name="virtualPathArgs"></param>
        /// <returns></returns>
        public static ApiMetaData GetReturnMeta(string virtualPath, string queryString, RequestOptions options, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, params object[] virtualPathArgs)
        {
            JObject resultData = Get(virtualPath, queryString, options, urlParts, apiTokens, clientSecrets, virtualPathArgs);

            return ConvertRestResponseToApiMetaData(resultData);
        }

        /// <summary>
        ///     GET a List of T back. Use when you are expecting and array of results.
        /// </summary>
        /// <param name="virtualPath">Path you want to access based on the base url of the token. Start it with '~/'</param>
        /// <param name="queryString">The query string, already URL encoded</param>
        /// <param name="options"> </param>
        /// <param name="urlParts"> </param>
        /// <param name="clientSecrets"> </param>
        /// <param name="apiTokens">The OAuth Access token.</param>
        /// <param name="virtualPathArgs">The arguments to replace the tokens ({0},{1}, etc.) in the virtual path</param>
        /// <returns></returns>
        public static Page<T> GetPagedResult<T>(string virtualPath, string queryString, RequestOptions options, UrlParts urlParts, IClientSecrets clientSecrets, Tokens apiTokens, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JToken resultData = Get(virtualPath, queryString, options, urlParts, apiTokens, clientSecrets, virtualPathArgs);

            Page<T> result = ConvertToRestTokenObjectPage<T>(clientSecrets, apiTokens, resultData);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="clientSecrets"> </param>
        /// <param name="apiTokens"> </param>
        /// <param name="virtualPathArgs"></param>
        /// <param name="urlParts"> </param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetListResult<T>(string virtualPath, string queryString, RequestOptions options, UrlParts urlParts, IClientSecrets clientSecrets, Tokens apiTokens, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Get(virtualPath, queryString, options, urlParts, apiTokens, clientSecrets, virtualPathArgs);

            return ConvertToRestTokenObjectList<T>(clientSecrets, apiTokens, resultData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="writeAllHeaders"></param>
        public static void OutputCurlCommand(HttpClient client, HttpMethod method, string url, StringContent content, bool writeAllHeaders = false)
        {
            if (GlobalEvents.IsListening(LogLevelType.Debug))
            {
                var sbCurl = new StringBuilder("CURL command: " + Environment.NewLine);
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
                    sbCurl.AppendFormat("\t-H \"{0}: {1}\" \\{2}", header.Key, header.Value.Aggregate((i, j) => i + " " + j), Environment.NewLine);
                }

                if (content != null)
                {
                    string jsonData = string.Empty;
                    Task task = content.ReadAsStringAsync().ContinueWith(f => { jsonData = f.Result; });

                    task.Wait();

                    if (!string.IsNullOrWhiteSpace(jsonData))
                    {
                        sbCurl.AppendFormat("\t-d \'{0}\' \\ {1}", jsonData.Replace(Environment.NewLine, Environment.NewLine + "\t\t"), Environment.NewLine);
                    }
                }

                sbCurl.AppendLine(url);

                Debug.WriteLine(sbCurl.ToString());

                LogEventManager.Debug(sbCurl.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="urlParts"> </param>
        /// <param name="clientSecrets"> </param>
        /// <param name="apiTokens"> </param>
        /// <param name="postData"></param>
        /// <param name="virtualPathArgs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Post<T>(string virtualPath, string queryString, UrlParts urlParts, IClientSecrets clientSecrets, Tokens apiTokens, object postData, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Post(virtualPath, queryString, urlParts, apiTokens, clientSecrets, postData, virtualPathArgs);
            var result = ConvertToRestTokenObject<T>(clientSecrets, apiTokens, resultData);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="urlParts"> </param>
        /// <param name="apiTokens"> </param>
        /// <param name="postData"></param>
        /// <param name="virtualPathArgs"></param>
        /// <returns></returns>
        public static ApiMetaData PostReturnMeta(string virtualPath, string queryString, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, object postData, params object[] virtualPathArgs)
        {
            JObject resultData = Post(virtualPath, queryString, urlParts, apiTokens, clientSecrets, postData, virtualPathArgs);
            return ConvertRestResponseToApiMetaData(resultData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="urlParts"> </param>
        /// <param name="clientSecrets"> </param>
        /// <param name="apiTokens"> </param>
        /// <param name="postData"></param>
        /// <param name="virtualPathArgs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> PostListResult<T>(string virtualPath, string queryString, UrlParts urlParts, IClientSecrets clientSecrets, Tokens apiTokens, object postData, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Post(virtualPath, queryString, urlParts, apiTokens, clientSecrets, postData, virtualPathArgs);
            return ConvertToRestTokenObjectList<T>(clientSecrets, apiTokens, resultData);
        }

        /// <summary>
        ///     Processes the resultData and extracts the status code
        /// </summary>
        /// <param name="resultData"></param>
        /// <param name="targetUrl"></param>
        /// <param name="method"> </param>
        /// <returns></returns>
        public static JObject ProcessResultData(JObject resultData, string targetUrl, HttpMethod method)
        {
            if (resultData == null)
            {
                LogEventManager.Warn(GenerateMessage("Http response returned null.", null, targetUrl, method));
                return null;
            }

            JObject jData = resultData;

            try
            {
                if (jData["meta"] != null)
                {
                    if (jData["meta"]["status"] != null)
                    {
                        HttpStatusCode statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), ((int)jData["meta"]["status"]).ToString(CultureInfo.InvariantCulture));

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
            catch (Exception ex)
            {
                LogEventManager.Error(GenerateMessage("Error processing the JSON result.", resultData, targetUrl, method), ex);
            }

            return jData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="urlParts"> </param>
        /// <param name="clientSecrets"> </param>
        /// <param name="apiTokens"></param>
        /// <param name="postData"></param>
        /// <param name="virtualPathArgs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Put<T>(string virtualPath, string queryString, UrlParts urlParts, IClientSecrets clientSecrets, Tokens apiTokens, object postData, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Put(virtualPath, queryString, urlParts, apiTokens, clientSecrets, postData, virtualPathArgs);
            var result = ConvertToRestTokenObject<T>(clientSecrets, apiTokens, resultData);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="urlParts"> </param>
        /// <param name="clientSecrets"> </param>
        /// <param name="apiTokens"></param>
        /// <param name="postData"></param>
        /// <param name="virtualPathArgs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Put<T>(string virtualPath, string queryString, UrlParts urlParts, IClientSecrets clientSecrets, Tokens apiTokens, List<KeyValuePair<string, string>> postData, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Put(virtualPath, queryString, urlParts, apiTokens, clientSecrets, postData, virtualPathArgs);
            var result = ConvertToRestTokenObject<T>(clientSecrets, apiTokens, resultData);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="urlParts"> </param>
        /// <param name="apiTokens"> </param>
        /// <param name="postData"></param>
        /// <param name="virtualPathArgs"></param>
        /// <returns></returns>
        public static ApiMetaData PutReturnMeta(string virtualPath, string queryString, UrlParts urlParts, Tokens apiTokens, IClientSecrets clientSecrets, object postData, params object[] virtualPathArgs)
        {
            JObject resultData = Put(virtualPath, queryString, urlParts, apiTokens, clientSecrets, postData, virtualPathArgs);
            return ConvertRestResponseToApiMetaData(resultData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="urlParts"> </param>
        /// <param name="clientSecrets"> </param>
        /// <param name="apiTokens"> </param>
        /// <param name="postData"></param>
        /// <param name="virtualPathArgs"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> PutListResult<T>(string virtualPath, string queryString, UrlParts urlParts, IClientSecrets clientSecrets, Tokens apiTokens, object postData, params object[] virtualPathArgs) where T : RestObject, new()
        {
            JObject resultData = Put(virtualPath, queryString, urlParts, apiTokens, clientSecrets, postData, virtualPathArgs);
            return ConvertToRestTokenObjectList<T>(clientSecrets, apiTokens, resultData);
        }

        public static JObject PutResponse(string virtualPath, string queryString, UrlParts urlParts, IClientSecrets clientSecrets, Tokens apiTokens, object postData, params object[] virtualPathArgs)
        {
            JObject resultData = Put(virtualPath, queryString, urlParts, apiTokens, clientSecrets, postData, virtualPathArgs);
            return resultData;
        }

        #endregion

        #region Private

        public static T ConvertToRestTokenObject<T>(JObject resultData) where T : RestObject, new()
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
                                result.PopulateData(dataNode);
                            }
                        }
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<T>(dataNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                        if (result != null)
                        {
                            result.Meta = resultData["meta"].ToObject<ApiMetaData>();
                        }
                    }
                }
                else
                {
                    JToken metaNode = resultData["meta"];
                    if (metaNode != null)
                    {
                        LogEventManager.Error(string.Format("No data returned: {0}{1}", Environment.NewLine, metaNode));
                    }
                }
            }

            return result;
        }

        private static ApiMetaData ConvertRestResponseToApiMetaData(JObject resultData)
        {
            ApiMetaData result = null;

            if (resultData != null)
            {
                result = resultData["meta"].ToObject<ApiMetaData>();
            }

            return result;
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

        private static T ConvertToRestTokenObject<T>(IClientSecrets clientSecrets, Tokens apiTokens, JObject resultData) where T : RestObject, new()
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
                                result.PopulateAccessToken(clientSecrets, apiTokens);
                                result.Meta = resultData["meta"].ToObject<ApiMetaData>();
                                result.PopulateData(dataNode);
                            }
                        }
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<T>(dataNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                        if (result != null)
                        {
                            result.PopulateAccessToken(clientSecrets, apiTokens);
                            result.Meta = resultData["meta"].ToObject<ApiMetaData>();
                        }
                    }
                }
                else
                {
                    JToken metaNode = resultData["meta"];
                    if (metaNode != null)
                    {
                        LogEventManager.Error(string.Format("No data returned: {0}{1}", Environment.NewLine, metaNode));

                    }
                }
            }

            return result;
        }

        private static Page<T> ConvertToRestTokenObjectPage<T>(IClientSecrets clientSecrets, Tokens apiTokens, JToken resultData) where T : RestObject, new()
        {
            var result = new Page<T>(clientSecrets, apiTokens);
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
                                item.PopulateAccessToken(clientSecrets, apiTokens);
                                item.Meta = resultData["meta"].ToObject<ApiMetaData>();
                                item.PopulateData(dataNode);
                            }

                            resultSet.Add(item);
                        }
                    }
                    else
                    {
                        var item = JsonConvert.DeserializeObject<T>(dataNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                        if (item != null)
                        {
                            item.PopulateAccessToken(clientSecrets, apiTokens);
                            item.Meta = resultData["meta"].ToObject<ApiMetaData>();
                            item.PopulateData(dataNode);
                        }

                        resultSet.Add(item);
                    }
                }
            }

            result.Items = resultSet;

            return result;
        }

        private static List<T> ConvertToRestTokenObjectList<T>(IClientSecrets clientSecrets, Tokens apiTokens, JToken resultData) where T : RestObject, new()
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
                                item.PopulateAccessToken(clientSecrets, apiTokens);
                                item.Meta = resultData["meta"].ToObject<ApiMetaData>();
                                item.PopulateData(resultData["data"]);
                            }

                            result.Add(item);
                        }
                    }
                    else
                    {
                        var item = JsonConvert.DeserializeObject<T>(dataNode.ToString(), GlobalConfiguration.GetJsonSerializerSettings());

                        if (item != null)
                        {
                            item.PopulateAccessToken(clientSecrets, apiTokens);
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
                return string.Format("{0} Url: {1}{3}{2}{3}Result JSON data: null", method, targetUrl, message, Environment.NewLine);
            }

            return string.Format("{0}Result JSON data:{0}{1}", Environment.NewLine, resultData);
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
                        Task task = taskwithresponse.Result.Content.ReadAsStringAsync().ContinueWith(taskReader => LogEventManager.Error("Response could not be converted to JSON because it was of type: " + taskwithresponse.Result.Content.Headers.ContentType.MediaType + ". Response:" + taskReader.Result));

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

        private static string CreateUrl(UrlParts urlParts, string virtualPath, string queryString, string fields = "", bool expand = false, bool includeAliases = true)
        {
            string baseUrl = urlParts.BaseUrl;

            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }

            string customerDatabaseUrl = "";

            if (includeAliases)
            {
                customerDatabaseUrl = string.Format("{0}api/v{1}/{2}/{3}/", baseUrl, urlParts.ApiVersion, urlParts.CustomerAlias, urlParts.DatabaseAlias);
            }
            else
            {
                customerDatabaseUrl = string.Format("{0}api/v{1}/", baseUrl, urlParts.ApiVersion);
            }
            

            string url;

            virtualPath = virtualPath.Replace(@"\", "/");
            if (virtualPath.StartsWith("~/"))
            {
                url = customerDatabaseUrl + virtualPath.Substring(2);
            }
            else if (virtualPath.StartsWith("/"))
            {
                url = customerDatabaseUrl + customerDatabaseUrl.Substring(1);
            }
            else
            {
                url = customerDatabaseUrl + virtualPath;
            }


            if (queryString == null)
            {
                //Make sure that we aren't dealing with a null item
                queryString = string.Empty;
            }


            if (!String.IsNullOrWhiteSpace(fields))
            {
                if (queryString.Length > 0)
                {
                    queryString += "&";
                }


                queryString += "fields=" + fields;
            }


            if (expand)
            {
                if (queryString.Length > 0)
                {
                    queryString += "&";
                }
                
                queryString += "expand=true";
            }


            if (url.Contains("?"))
            {
                //the url has a query string in it, split out the query string
                var parts = url.Split("?".ToCharArray());
                if (parts.Length > 1)
                {
                    //Parts[0] = the base url
                    //Parts[1] = the query string
                    url = parts[0];


                    if (queryString.Length > 0)
                    {
                        //A query string already exists, append on to it
                        queryString += "&" + parts[1];
                    }
                    else
                    {
                        //no query string yet, replace it
                        queryString = parts[1];
                    }
                }


            }

            //Remove the leading question mark
            if (!String.IsNullOrWhiteSpace(queryString) && queryString.StartsWith("?"))
            {
                queryString = queryString.Substring(1);
            }

            StringBuilder sbQuery = new StringBuilder();

            if (GlobalConfiguration.SuppressResponseCodes)
            {
                sbQuery.Append("suppress_response_codes=true&");
            }

            if (!string.IsNullOrWhiteSpace(queryString))
            {
                sbQuery.Append(queryString);
            }

            if (sbQuery.Length > 0)
            {
                sbQuery.Insert(0, "?");
            }

            url = url + sbQuery;

            return url;
        }

        #endregion
    }
}