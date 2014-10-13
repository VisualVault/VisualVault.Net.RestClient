// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiMetaData.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace VVRestApi.Common.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiMetaData
    {
        #region Constructors and Destructors

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

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

       

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsAffirmativeStatus()
        {
            bool result = this.StatusCode == HttpStatusCode.OK || this.StatusCode == HttpStatusCode.Accepted || this.StatusCode == HttpStatusCode.Created || this.StatusCode == HttpStatusCode.PartialContent || this.StatusCode == HttpStatusCode.NoContent;
            return result;
        }
    }
}