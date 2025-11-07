// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestOptions.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace VVRestApi.Common
{
    using System;
    using System.Collections.Specialized;
    using System.Web;

    /// <summary>
    /// 
    /// </summary>
    public class RequestOptions : IRequestOptions
    {
        #region Constructors and Destructors

        /// <summary>
        /// Options for every request
        /// </summary>
        public RequestOptions()
        {
            this.Query = string.Empty;
            this.Take = GlobalConfiguration.DefaultPageSize;
            this.Skip = 0;
            this.Expand = false;
            this.Fields = string.Empty;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public bool Expand { get; set; }

        /// <summary>
        /// A comma-delimited list of fields to return. If none are supplied, the server will return the default fields.
        /// </summary>
        public string Fields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Take { get; set; }

        #endregion

        public void PrepForRequest()
        {
            if (!string.IsNullOrWhiteSpace(this.Fields))
            {
                this.Expand = false;
            }

            if (this.Take <= 0)
            {
                this.Take = 200;
            }

            if (!string.IsNullOrWhiteSpace(this.Query))
            {
                if (!this.Query.StartsWith("q=", StringComparison.OrdinalIgnoreCase))
                {
                    this.Query = string.Format("q={0}", this.Query);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalQueryString"></param>
        /// <returns></returns>
        //public string GetQueryString(string originalQueryString)
        //{
        //    string result = string.Empty;
        //    bool hasLimit = false;
        //    bool hasOffset = false;

        //    this.PrepForRequest();

        //    if (!String.IsNullOrWhiteSpace(originalQueryString) && !String.IsNullOrWhiteSpace(this.Query))
        //    {
        //        NameValueCollection nvc = HttpUtility.ParseQueryString(originalQueryString);
        //        if (nvc.HasKeys())
        //        {
        //            var existingQuery = this.Query;
        //            if (existingQuery.StartsWith("q="))
        //            {
        //                existingQuery = existingQuery.Substring(2);
        //            }

        //            foreach (string key in nvc.Keys)
        //            {
        //                if (key.Equals("q"))
        //                {
        //                    this.Query = string.Format("q=({0}) AND ({1})", (string)nvc[key], existingQuery);
        //                }
        //                else if (key.Equals("limit"))
        //                {
        //                    //Don't add these in
        //                    result += string.Format("{0}={1}&", key, (string)nvc[key]);
        //                    hasLimit = true;
        //                }
        //                else if (key.Equals("offset"))
        //                {
        //                    //Don't add these in
        //                    result += string.Format("{0}={1}&", key, (string)nvc[key]);
        //                    hasOffset = true;
        //                }
        //                else
        //                {
        //                    result += string.Format("{0}={1}&", key, (string)nvc[key]);
        //                }
        //            }
        //        }

        //        result += this.Query + "&";

        //    }
        //    else
        //    {
        //        var stringToParse = originalQueryString + this.Query;
        //        NameValueCollection nvc = HttpUtility.ParseQueryString(stringToParse);
        //        foreach (string key in nvc.Keys)
        //        {
        //           if (key.Equals("limit"))
        //            {
        //                //Don't add these in
        //                result += string.Format("{0}={1}&", key, (string)nvc[key]);
        //                hasLimit = true;
        //            }
        //            else if (key.Equals("offset"))
        //            {
        //                //Don't add these in
        //                result += string.Format("{0}={1}&", key, (string)nvc[key]);
        //                hasOffset = true;
        //            }
        //            else
        //            {
        //                result += string.Format("{0}={1}&", key, (string)nvc[key]);
        //            }
        //        }
        //    }

        //    if (!hasLimit && this.Take > 0)
        //    {
        //        result += string.Format("limit={0}&", this.Take);

        //        //Offset can be 0
        //        if (!hasOffset)
        //        {
        //            if (this.Skip < 0)
        //            {
        //                this.Skip = 0;
        //            }

        //            result += string.Format("offset={0}&", this.Skip);
        //        }
        //    }

        //    if (result.EndsWith("&"))
        //    {
        //        result = result.Substring(0, result.Length - 1);
        //    }

        //    return result;

        //}

        public string GetQueryString(string originalQueryString)
        {
            string result = string.Empty;
            bool hasLimit = false;
            bool hasOffset = false;

            this.PrepForRequest();

            if (!String.IsNullOrWhiteSpace(originalQueryString) && !String.IsNullOrWhiteSpace(this.Query))
            {
                var queryString = ParseQueryString(originalQueryString);
                if (queryString.Count > 0)
                {
                    var existingQuery = this.Query;
                    if (existingQuery.StartsWith("q="))
                    {
                        existingQuery = existingQuery.Substring(2);
                    }

                    foreach (string key in queryString.Keys)
                    {
                        if (key.Equals("q"))
                        {
                            this.Query = string.Format("q=({0}) AND ({1})", (string) queryString[key], existingQuery);
                        }
                        else if (key.Equals("limit"))
                        {
                            //Don't add these in
                            result += string.Format("{0}={1}&", key, (string) queryString[key]);
                            hasLimit = true;
                        }
                        else if (key.Equals("offset"))
                        {
                            //Don't add these in
                            result += string.Format("{0}={1}&", key, (string) queryString[key]);
                            hasOffset = true;
                        }
                        else
                        {
                            result += string.Format("{0}={1}&", key, (string) queryString[key]);
                        }
                    }
                }

                result += this.Query + "&";

            }
            else
            {
                var stringToParse = originalQueryString + this.Query;

                var queryString = ParseQueryString(stringToParse);
                foreach (string key in queryString.Keys)
                {
                    if (key.Equals("limit"))
                    {
                        //Don't add these in
                        result += string.Format("{0}={1}&", key, (string) queryString[key]);
                        hasLimit = true;
                    }
                    else if (key.Equals("offset"))
                    {
                        //Don't add these in
                        result += string.Format("{0}={1}&", key, (string) queryString[key]);
                        hasOffset = true;
                    }
                    else
                    {
                        result += string.Format("{0}={1}&", key, (string) queryString[key]);
                    }
                }
            }

            if (!hasLimit && this.Take > 0)
            {
                result += string.Format("limit={0}&", this.Take);

                //Offset can be 0
                if (!hasOffset)
                {
                    if (this.Skip < 0)
                    {
                        this.Skip = 0;
                    }

                    result += string.Format("offset={0}&", this.Skip);
                }
            }

            if (result.EndsWith("&"))
            {
                result = result.Substring(0, result.Length - 1);
            }

            return result;

        }

        private static Dictionary<string, string> ParseQueryString(String query)
        {
            Dictionary<String, String> queryDict = new Dictionary<string, string>();

            foreach (String token in query.TrimStart(new char[]
            {
                '?'
            }).Split(new char[]
            {
                '&'
            }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = token.Split(new char[]
                {
                    '='
                }, StringSplitOptions.RemoveEmptyEntries);

                var key = parts[0].Trim();
                if (parts.Length > 1)
                {
                    //var value = HttpUtility.UrlDecode(parts[1]);
                    var value = parts[1].Trim();
                    // if there are other '=' characters in the set, just include them
                    for (var i = 2; i < parts.Length; i++) 
                    {
                        value += $" = {parts[i].Trim()}";
                    }

                    queryDict[key] = (value ?? "").Trim();
                }
                else
                {
                    queryDict[parts[0].Trim()] = "";
                }
            }
            return queryDict;


        }
    }
}