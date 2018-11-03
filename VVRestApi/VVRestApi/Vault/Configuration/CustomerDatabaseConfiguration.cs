// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Group.cs" company="GRM">
//   Copyright (c) GRM Information Management 2017. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Newtonsoft.Json.Linq;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Vault.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Net;
    using Newtonsoft.Json;
    using VVRestApi.Common;
    using VVRestApi.Common.Extensions;
    using VVRestApi.Common.Logging;
    using VVRestApi.Vault.Users;

    /// <summary>
    /// 
    /// </summary>
    public class CustomerDatabaseConfiguration : RestObject
    {
        #region Constructors and Destructors

        /// <summary>
        /// 
        /// </summary>
        public CustomerDatabaseConfiguration()
        {
            
        }

        #endregion

        #region Public Properties

        [JsonProperty(PropertyName = "baseUrl")]
        public new string BaseUrl { get; set; }

        [JsonProperty(PropertyName = "customerId")]
        public Guid CustomerId { get; set; }

        [JsonProperty(PropertyName = "customerDatabaseId")]
        public Guid CustomerDatabaseId { get; set; }

        [JsonProperty(PropertyName = "customerAlias")]
        public string CustomerAlias { get; set; }

        [JsonProperty(PropertyName = "customerDatabaseAlias")]
        public string CustomerDatabaseAlias { get; set; }

        [JsonProperty(PropertyName = "utcOffset")]
        public int UtcOffset { get; set; }

        [JsonProperty(PropertyName = "vaultDbConnections")]
        public VaultDbConnections VaultDbConnections { get; set; }

        [JsonProperty(PropertyName = "contentProviders")]
        public ContentProvider[] ContentProviders { get; set; }

        #endregion
    }
}