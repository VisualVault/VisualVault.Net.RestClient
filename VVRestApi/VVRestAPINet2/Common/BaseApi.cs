using System;
using VVRestAPINet2.Common.Messaging;

namespace VVRestAPINet2.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseApi
    {
        protected BaseApi()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public ClientSecrets ClientSecrets { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Tokens ApiTokens { get; set; }

        /// <summary>
        /// Encode the passed in value for web requests
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string UrlEncode(string value)
        {
            return Uri.EscapeDataString(value);
        }

        /// <summary>
        /// Populates the token
        /// </summary>
        /// <param name="clientSecrets"></param>
        /// <param name="apiTokens"> </param>
        internal void Populate(ClientSecrets clientSecrets, Tokens apiTokens)
        {
            this.ClientSecrets = clientSecrets;
            this.ApiTokens = apiTokens;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal UrlParts GetUrlParts()
        {
            UrlParts urlParts = new UrlParts
            {
                ApiVersion = ClientSecrets.ApiVersion,
                BaseUrl = ClientSecrets.BaseUrl,
                CustomerAlias = ClientSecrets.CustomerAlias,
                DatabaseAlias = ClientSecrets.DatabaseAlias,
                OAuthTokenEndPoint = ClientSecrets.OAuthTokenEndPoint
            };

            return urlParts;
        }
    }
}
