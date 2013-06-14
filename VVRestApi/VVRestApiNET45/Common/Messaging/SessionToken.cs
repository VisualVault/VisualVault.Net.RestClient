// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticatedToken.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApi.Common.Messaging
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;

    /// <summary>
    /// The authenticated token.
    /// </summary>
    internal class SessionToken
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionToken"/> class. 
        /// Creates a new authentication token
        /// </summary>
        /// <param name="baseUrl">
        /// </param>
        /// <param name="customerAlias">
        /// </param>
        /// <param name="databaseAlias">
        /// </param>
        /// <param name="tokenBase64">
        /// </param>
        /// <param name="expirationDate">
        /// </param>
        public SessionToken(string baseUrl, string customerAlias, string databaseAlias, string tokenBase64, DateTime? expirationDateUtc, string developerKey, string developerSecret)
        {
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }

            this.BaseUrl = baseUrl;
            this.CustomerAlias = customerAlias;
            this.DatabaseAlias = databaseAlias;
            this.TokenBase64 = tokenBase64;
            this.ExpirationDateUtc = expirationDateUtc;
            this.TokenType = TokenType.Vault;
            this.DeveloperKey = developerKey;
            this.DeveloperSecret = developerSecret;
            if (customerAlias.Equals("config", StringComparison.OrdinalIgnoreCase))
            {
                if (databaseAlias.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    this.TokenType = TokenType.Administration;
                }
            }
        }

       
        #endregion

        #region Public Properties

        /// <summary>
        ///     The URL to VisualVault that will always end in a forward slash. Ends with 'api/v1/'
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        ///     The customer alias that this token is for
        /// </summary>
        public string CustomerAlias { get; set; }

        /// <summary>
        ///     The database alias that this token is for
        /// </summary>
        public string DatabaseAlias { get; set; }

        /// <summary>
        /// Gets or sets the expiration date utc.
        /// </summary>
        public DateTime? ExpirationDateUtc { get; set; }

        /// <summary>
        ///     The string version of the passed out token
        /// </summary>
        public string TokenBase64 { get; set; }

        public TokenType TokenType { get; set; }

        public string DeveloperKey { get; set; }
        
        public string DeveloperSecret { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns true if the token is expired
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsExpired()
        {
            bool expired = false;
            if (this.ExpirationDateUtc.HasValue)
            {
                if (this.ExpirationDateUtc.Value < DateTime.UtcNow)
                {
                    expired = true;
                }
            }

            return expired;
        }

        /// <summary>
        /// Returns true if the authenticated token is valid
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsValid()
        {
            bool result = false;
            if (!this.IsExpired())
            {
                // Make sure the token has a value
                result = !string.IsNullOrWhiteSpace(this.TokenBase64);
            }

            return result;
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="queryString"></param>
        /// <param name="method"></param>
       /// <param name="fields">A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.</param>
        /// <param name="expand">If set to true, the request will return all available fields.</param>
        /// <returns></returns>
        public string CreateUrl(string virtualPath, string queryString, HttpMethod method, string fields="", bool expand = false)
        {
            string customerDatabaseUrl = string.Format("{0}{1}/{2}/", BaseUrl, CustomerAlias, DatabaseAlias);
            string url = string.Empty;

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
            if (!String.IsNullOrWhiteSpace(queryString) && queryString.StartsWith("?"))
            {
                queryString = queryString.Substring(1);
            }

            //append the question mark to the front of the query string along with the token
            StringBuilder sbQuery = new StringBuilder();
            if (method == HttpMethod.Get || method == HttpMethod.Delete)
            {
                sbQuery.AppendFormat("token={0}&", System.Net.WebUtility.UrlEncode(this.TokenBase64));
            }

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

            url = url + sbQuery.ToString();

            return url;
        }

        public void PrepPostPutHeaders(HttpClient client)
        {
            //Add headers to the client
            client.DefaultRequestHeaders.Add("X-VV-Token", this.TokenBase64);
            
        }
    }
}