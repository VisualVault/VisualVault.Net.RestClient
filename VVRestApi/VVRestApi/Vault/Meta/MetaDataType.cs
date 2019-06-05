// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaDataType.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VVRestApi.Common;

namespace VVRestApi.Vault.Meta
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class MetaDataType : RestObject
    {
        #region Public Properties

        /// <summary>
        ///     The available fields for this data type and also the fields that are returned if you do a POST, PUT or query with "expand=data" on GETs in your querystring in the VaultApi.REST area
        /// </summary>
        [JsonProperty(PropertyName = "AvailableFields")]
        public List<string> AvailableFields { get; internal set; }

        /// <summary>
        ///     The default fields that are returned if you do not request any
        /// </summary>
        [JsonProperty(PropertyName = "DefaultFields")]
        public List<string> DefaultFields { get; internal set; }

        /// <summary>
        ///     The field that is considered the Id for most queries and REST calls
        /// </summary>
        [JsonProperty(PropertyName = "IdField")]
        public string IdField { get; internal set; }

        /// <summary>
        ///     Name of the DataType
        /// </summary>
        [JsonProperty(PropertyName = "DataType")]
        public string Name { get; internal set; }

        #endregion
    }
}