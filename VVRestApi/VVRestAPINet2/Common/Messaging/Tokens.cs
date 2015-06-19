using System;

namespace VVRestAPINet2.Common.Messaging
{

    /// <summary>
    /// Holds the Access and Refresh Tokens
    /// </summary>
    public struct Tokens
    {
        /// <summary>
        /// An AccessToken is returned after authenticating with the OAuth/Token API end point.  The AccessToken must be included in the Authorization HTTP header
        /// with each API Request.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Expiration Date for the AccessToken
        /// </summary>
        public DateTime AccessTokenExpiration { get; set; }

        /// <summary>
        /// Returned along with the AccessToken when authenticating with the VisualVault REST API.
        /// The Refresh token has a longer expiration time than the AccessToken and may be used to request a new
        /// AccessToken without providing credentials. This provides a means to update data (Claims) stored in the AccessToken and 
        /// improves security with frequent token updates.
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
