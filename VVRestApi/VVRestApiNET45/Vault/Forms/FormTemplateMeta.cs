// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormTemplateMeta.cs" company="Auersoft">
//   Copyright (c) Auersoft 2014. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApi.Vault.Forms
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using VVRestApi.Common;

    /// <summary>
    /// 
    /// </summary>
    public class FormTemplateMeta : RestObject
    {
        #region Constructors and Destructors

        /// <summary>
        /// 
        /// </summary>
        public FormTemplateMeta()
        {
            this.Fields = new Dictionary<string, string>();
            this.BaseFields = new List<string>();
        }

        #endregion
        
        #region Public Properties

        /// <summary>
        ///     A list of all of the fields. The key is the display name of the field, Value is the database name of the field
        /// </summary>
        [JsonProperty(PropertyName = "Fields")]
        public Dictionary<string, string> Fields { get; set; }

        /// <summary>
        ///     The basic fields to be
        /// </summary>
        [JsonProperty(PropertyName = "baseFields")]
        public List<string> BaseFields { get; set; }

        #endregion
    }
}