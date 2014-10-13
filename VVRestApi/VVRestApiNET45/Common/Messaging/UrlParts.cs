using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVRestApi.Common.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public struct UrlParts
    {
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
        /// 
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OAuthTokenEndPoint { get; set; }
    }
}
