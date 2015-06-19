using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace VVRestAPINet2.Common.Messaging
{
    public static class HttpHelper
    {
        #region OAuth Access/Refresh Tokens

        /// <summary>
        /// Authenticates with OAuth Authorization Server token end point using OAuth2 Client Credentials Grant Type
        /// </summary>
        /// <param name="oauthTokenEndPoint">OAuth2 Authorization Server Access/Refresh Token URL</param>
        /// <param name="apiKey">Issued by VisualVault, used to identify the client application/developer</param>
        /// <param name="apiSecret">Issued by VisualVault, used to authenticate the client application/developer</param>
        /// <returns></returns>
        public static Tokens GetAccessToken(string oauthTokenEndPoint, string apiKey, string apiSecret)
        {
            Tokens apiTokens = new Tokens();

            var post = new Dictionary<string, string>
                           {
                               {"client_id", apiKey},
                               {"client_secret", apiSecret},
                               {"username", apiKey},
                               {"password", apiSecret},
                               {"grant_type", "client_credentials"}
                           };

            //string formData = post.Keys.Aggregate("", (current, key) => current + string.Format("&{0}={1}", key, post[key]));

            string formData = "";

            foreach (string key in post.Keys)
            {
                formData += string.Format("&{0}={1}", key, post[key]);
            }

            WebRequest httpWebRequest = WebRequest.Create(oauthTokenEndPoint);
            httpWebRequest.Method = "Post";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(formData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            var stream = httpResponse.GetResponseStream();

            if (stream != null)
            {
                string content;

                using (var streamReader = new StreamReader(stream))
                {
                    content = streamReader.ReadToEnd();
                }

                var json = JObject.Parse(content);

                apiTokens = new Tokens
                                   {
                                       AccessToken = json["access_token"].ToString(),
                                       RefreshToken = json["refresh_token"].ToString()
                                   };

                if (!string.IsNullOrEmpty(json["expires_in"].ToString()))
                {
                    double expiration;
                    if (double.TryParse(json["expires_in"].ToString(), out expiration))
                    {
                        apiTokens.AccessTokenExpiration = DateTime.UtcNow.AddSeconds(expiration);
                    }
                }
            }

            return apiTokens;
        }

        /// <summary>
        /// Authenticates with OAuth Authorization Server token end point using OAuth2 Client Credentials Grant Type
        /// </summary>
        /// <param name="oauthTokenEndPoint">OAuth2 Authorization Server Access/Refresh Token URL</param>
        /// <param name="apiKey">Issued by VisualVault, used to identify the client application/developer</param>
        /// <param name="apiSecret">Issued by VisualVault, used to authenticate the client application/developer</param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Tokens GetAccessToken(string oauthTokenEndPoint, string apiKey, string apiSecret, string userName, string password)
        {
            Tokens apiTokens = new Tokens();

            var post = new Dictionary<string, string>
                           {
                               {"client_id", apiKey},
                               {"client_secret", apiSecret},
                               {"username", userName},
                               {"password", password},
                               {"grant_type", "client_credentials"}
                           };

            string formData = "";

            foreach (string key in post.Keys)
            {
                formData += string.Format("&{0}={1}", key, post[key]);
            }

            WebRequest httpWebRequest = WebRequest.Create(oauthTokenEndPoint);
            httpWebRequest.Method = "Post";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(formData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            var stream = httpResponse.GetResponseStream();

            if (stream != null)
            {
                string content;

                using (var streamReader = new StreamReader(stream))
                {
                    content = streamReader.ReadToEnd();
                }

                var json = JObject.Parse(content);

                apiTokens = new Tokens
                {
                    AccessToken = json["access_token"].ToString(),
                    RefreshToken = json["refresh_token"].ToString()
                };

                if (!string.IsNullOrEmpty(json["expires_in"].ToString()))
                {
                    double expiration;
                    if (double.TryParse(json["expires_in"].ToString(), out expiration))
                    {
                        apiTokens.AccessTokenExpiration = DateTime.UtcNow.AddSeconds(expiration);
                    }
                }
            }

            return apiTokens;
        }

        /// <summary>
        /// Authenticates with OAuth Authorization Server token end point using OAuth2 Client Credentials Grant Type
        /// </summary>
        /// <param name="oauthTokenEndPoint">OAuth2 Authorization Server Access/Refresh Token URL</param>
        /// <param name="apiKey">Issued by VisualVault, used to identify the client application/developer</param>
        /// <param name="apiSecret">Issued by VisualVault, used to authenticate the client application/developer</param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public static Tokens GetAccessToken(string oauthTokenEndPoint, string apiKey, string apiSecret, string refreshToken)
        {
            Tokens apiTokens = new Tokens();

            var post = new Dictionary<string, string>
                           {
                               {"client_id", apiKey},
                               {"client_secret", apiSecret},
                               {"refresh_token", refreshToken},
                               {"grant_type", "refresh_token"}
                           };

            string formData = "";

            foreach (string key in post.Keys)
            {
                formData += string.Format("&{0}={1}", key, post[key]);
            }

            WebRequest httpWebRequest = WebRequest.Create(oauthTokenEndPoint);
            httpWebRequest.Method = "Post";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(formData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            var stream = httpResponse.GetResponseStream();

            if (stream != null)
            {
                string content;

                using (var streamReader = new StreamReader(stream))
                {
                    content = streamReader.ReadToEnd();
                }

                var json = JObject.Parse(content);

                apiTokens = new Tokens
                {
                    AccessToken = json["access_token"].ToString(),
                    RefreshToken = json["refresh_token"].ToString()
                };

                if (!string.IsNullOrEmpty(json["expires_in"].ToString()))
                {
                    double expiration;
                    if (double.TryParse(json["expires_in"].ToString(), out expiration))
                    {
                        apiTokens.AccessTokenExpiration = DateTime.UtcNow.AddSeconds(expiration);
                    }
                }
            }

            return apiTokens;
        }

        #endregion

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
        public static Stream GetStream(string virtualPath, string queryString, RequestOptions options, UrlParts urlParts, Tokens apiTokens, ClientSecrets clientSecrets, params object[] virtualPathArgs)
        {
            if (options == null)
            {
                options = new RequestOptions();
            }

            options.PrepForRequest();

            CleanupVirtualPathArgs(virtualPathArgs);

            string url = CreateUrl(urlParts, string.Format(virtualPath, virtualPathArgs), options.GetQueryString(queryString), options.Fields, options.Expand);

            WebRequest httpWebRequest = WebRequest.Create(url);
            httpWebRequest.Method = "Get";

            httpWebRequest.Headers.Add("Authorization", "Bearer " + apiTokens.AccessToken);

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            Stream stream = httpResponse.GetResponseStream();

            return stream;
        }

        private static void CleanupVirtualPathArgs(object[] virtualPathArgs)
        {
            if (virtualPathArgs != null && virtualPathArgs.Length > 0)
            {
                for (int i = 0; i < virtualPathArgs.Length; i++)
                {
                    if (virtualPathArgs[i] is string)
                    {
                        virtualPathArgs[i] = Uri.EscapeDataString((string)virtualPathArgs[i]);
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

        private static string CreateUrl(UrlParts urlParts, string virtualPath, string queryString, string fields = "", bool expand = false)
        {
            string baseUrl = urlParts.BaseUrl;

            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }

            string customerDatabaseUrl = string.Format("{0}api/v{1}/{2}/{3}/", baseUrl, urlParts.ApiVersion, urlParts.CustomerAlias, urlParts.DatabaseAlias);

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


            if (!String.IsNullOrEmpty(fields))
            {
                if (queryString.Length > 0)
                {
                    queryString += "&";
                }


                queryString += "f=" + fields;
            }


            if (expand)
            {


                if (queryString.Length > 0)
                {
                    queryString += "&";
                }


                queryString += "expand=data";
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
            if (!String.IsNullOrEmpty(queryString) && queryString.StartsWith("?"))
            {
                queryString = queryString.Substring(1);
            }

            StringBuilder sbQuery = new StringBuilder();

            if (GlobalConfiguration.SuppressResponseCodes)
            {
                sbQuery.Append("suppress_response_codes=true&");
            }

            if (!string.IsNullOrEmpty(queryString))
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

    }
}
