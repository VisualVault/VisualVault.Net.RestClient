// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormTemplateMeta.cs" company="Auersoft">
//   Copyright (c) Auersoft. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VVRestApi.Vault.Forms
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using VVRestApi.Common;

    public class FormTemplateMeta : RestObject
    {
        #region Constructors and Destructors

        public FormTemplateMeta()
        {
            this.Fields = new Dictionary<string, string>();
            this.BaseFields = new List<string>();
        }

        #endregion

        internal override void PopulateData(Newtonsoft.Json.Linq.JToken data)
        {
            base.PopulateData(data);
        }

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