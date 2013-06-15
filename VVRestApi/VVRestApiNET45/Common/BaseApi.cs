namespace VVRestApi.Common
{
    using System;
    using System.Net;

    using VVRestApi.Common.Messaging;

    public class BaseApi
    {
        protected BaseApi()
        {
            
        }
     
        /// <summary>
        ///     The current token which the API is validated against.
        /// </summary>
        protected internal SessionToken CurrentToken { get; set; }

        /// <summary>
        /// Encode the passed in value for web requests
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string UrlEncode(string value)
        {
            return WebUtility.UrlEncode(value);
        }

        /// <summary>
        /// Populates the token
        /// </summary>
        /// <param name="token"></param>
        internal void Populate(SessionToken token)
        {
            this.CurrentToken = token;
        }
    }
}