using System;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Common
{
    /// <summary>
    /// structure used to hold the authentication properties
    /// </summary>
    public struct ClientSecrets : IClientSecrets
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

    /// <summary>
    /// structure used to hold the authentication properties
    /// </summary>
    public interface IClientEndPoints
    {
        /// <summary>
        /// VisualVault instance base URL (URL segment preceding the version number "/v1"). Example: 'https://na2.visualvault.com/'
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        /// The version number used when building the API URL (e.g. /v1/)
        /// </summary>
        string ApiVersion { get; }
    }

    /// <summary>
    /// structure used to hold the authentication properties
    /// </summary>
    public interface IClientSecrets : IClientEndPoints
    {
        /// <summary>
        /// API Client Id issued by the VisualVault Server
        /// </summary>
        string ApiKey { get; }

        /// <summary>
        /// API Client Secret issued by the VisualVault Server
        /// </summary>
        string ApiSecret { get; }

        /// <summary>
        ///     The customer alias that this token is for
        /// </summary>
        string CustomerAlias { get; }

        /// <summary>
        ///     The database alias that this token is for
        /// </summary>
        string DatabaseAlias { get; }

        /// <summary>
        /// URL of the OAuth Authorization server's token end point used for authentication
        /// </summary>
        string OAuthTokenEndPoint { get; }

        /// <summary>
        /// Scope is used to determine what resource types will be available after authentication.  If unsure of the scope to provide use
        /// either 'vault' or no value.  'vault' scope is used to request access to a specific customer vault (aka customer database).
        /// </summary>
        string Scope { get; }
    }


}
