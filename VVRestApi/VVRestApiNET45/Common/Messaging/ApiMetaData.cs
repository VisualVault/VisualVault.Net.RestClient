// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiMetaData.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApi.Common
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Serialization;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ApiMetaData
    {
        #region Constructors and Destructors

        public ApiMetaData()
        {

        }
        
        #endregion

        #region Public Properties

        /// <summary>
        ///     Contains a list of all of the error messages
        /// </summary>
        [JsonProperty(PropertyName = "errors")]
        public List<ApiErrorMessage> ErrorMessages { get; set; }

        /// <summary>
        ///     Stores extra data to place in the meta tag
        /// </summary>
        public List<Tuple<string, string>> ExtraMetaData { get; set; }

        /// <summary>
        ///     Method used to get the
        /// </summary>
        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }

        /// <summary>
        ///     Method used to get the
        /// </summary>
        [JsonProperty(PropertyName = "statusMsg")]
        public string StatusMsg { get; set; }

        [JsonProperty(PropertyName = "status")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

       

        #endregion

        public bool IsAffirmativeStatus()
        {
            bool result = this.StatusCode == HttpStatusCode.OK || this.StatusCode == HttpStatusCode.Accepted || this.StatusCode == HttpStatusCode.Created || this.StatusCode == HttpStatusCode.PartialContent || this.StatusCode == HttpStatusCode.NoContent;
            return result;
        }
    }
}