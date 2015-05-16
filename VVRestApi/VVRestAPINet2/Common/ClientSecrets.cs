

namespace VVRestAPINet2.Common
{
    /// <summary>
    /// 
    /// </summary>
    public struct ClientSecrets
    {
        /// <summary>
        /// VisualVault instance base URL (URL segment preceding the version number "/v1"). Example: 'https://na2.visualvault.com/'
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// API Client Id issued by the VisualVault Server
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// API Client Secret issued by the VisualVault Server
        /// </summary>
        public string ApiSecret { get; set; }

        /// <summary>
        ///     The customer alias that this token is for
        /// </summary>
        public string CustomerAlias { get; set; }

        /// <summary>
        ///     The database alias that this token is for
        /// </summary>
        public string DatabaseAlias { get; set; }

        /// <summary>
        /// The version number used when building the API URL (e.g. /v1/)
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// URL of the OAuth Authorization server's token end point used for authentication
        /// </summary>
        public string OAuthTokenEndPoint { get; set; }

        /// <summary>
        /// Scope is used to determine what resource types will be available after authentication.  If unsure of the scope to provide use
        /// either 'vault' or no value.  'vault' scope is used to request access to a specific customer vault (aka customer database).
        /// </summary>
        public string Scope { get; set; }
    }
}


