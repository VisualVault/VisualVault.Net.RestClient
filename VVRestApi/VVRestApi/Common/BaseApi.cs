
using VVRestApi.Common.Messaging;

namespace VVRestApi.Common
{
    using System.Net;

    /// <summary>
    /// 
    /// </summary>
    public class BaseApi
    {
        /// <summary>
        /// base url for email links to use
        /// </summary>
        public static string BaseUrl = "http://aws.visualvault.com/client/";
        public static string ShareUrl = "http://aws.example.com/sample/view/";

        
        protected BaseApi()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public IClientSecrets ClientSecrets { get; set; }

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
            return WebUtility.UrlEncode(value);
        }

        /// <summary>
        /// Populates the token
        /// </summary>
        /// <param name="clientSecrets"></param>
        /// <param name="apiTokens"> </param>
        internal void Populate(IClientSecrets clientSecrets, Tokens apiTokens)
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

        protected static UrlParts GetUrlParts(IClientSecrets clientSecrets)
        {
            UrlParts urlParts = new UrlParts
            {
                ApiVersion = clientSecrets.ApiVersion,
                BaseUrl = clientSecrets.BaseUrl,
                CustomerAlias = clientSecrets.CustomerAlias,
                DatabaseAlias = clientSecrets.DatabaseAlias,
                OAuthTokenEndPoint = clientSecrets.OAuthTokenEndPoint
            };

            return urlParts;
        }

        protected static UrlParts GetUrlParts(IClientEndPoints clientEndPoints)
        {
            UrlParts urlParts = new UrlParts
            {
                ApiVersion = clientEndPoints.ApiVersion,
                BaseUrl = clientEndPoints.BaseUrl
            };

            return urlParts;
        }
    }
}