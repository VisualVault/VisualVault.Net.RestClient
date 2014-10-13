namespace VVRestApi.Vault.PersistedData
{
    using System;

    using Newtonsoft.Json;

    using VVRestApi.Common;

    /// <summary>
    /// Allows for the creation and updating of text data to a table, PersistedClientData, in the vault
    /// </summary>
    public class PersistedClientData : RestObject
    {
        public PersistedClientData()
        {
        }

        #region Public Properties
       
        /// <summary>
        /// Gets or sets the create by us id.
        /// </summary>
        [JsonProperty(PropertyName = "createByUsId")]
        public Guid CreateByUsId { get; private set; }

        /// <summary>
        /// Gets or sets the create date utc.
        /// </summary>
        [JsonProperty(PropertyName = "createDateUtc")]
        public DateTime CreateDateUtc { get; private set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public string PersistedData { get; set; }

        /// <summary>
        /// Gets or sets the data length.
        /// </summary>
        [JsonProperty(PropertyName = "dataLength")]
        public long DataLength { get; private set; }

        /// <summary>
        /// Gets or sets the data mime type.
        /// </summary>
        public string DataMimeType { get; set; }

        /// <summary>
        /// Gets or sets the expiration date utc.
        /// </summary>
        public DateTime? ExpirationDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the linked object id.
        /// </summary>
        public string LinkedObjectId { get; set; }

        /// <summary>
        /// Gets or sets the linked object type.
        /// </summary>
        public LinkedObjectType LinkedObjectType { get; set; }

        /// <summary>
        /// Gets or sets the modified by us id.
        /// </summary>
        [JsonProperty(PropertyName = "modifiedByUsId")]
        public Guid ModifiedByUsId { get; private set; }

        /// <summary>
        /// Gets or sets the modified date utc.
        /// </summary>
        [JsonProperty(PropertyName = "modifiedDateUtc")]
        public DateTime ModifiedDateUtc { get; set; }
        
        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        public ScopeType Scope { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }


        public bool Delete()
        {
            bool wasDeleted = false;
            var result = HttpHelper.DeleteReturnMeta(GlobalConfiguration.Routes.PersistedDataId, string.Empty, this.CurrentToken, this.Id);
            
            if (result != null)
            {
                if (result.IsAffirmativeStatus())
                {
                    wasDeleted = true;
                    this.Id = Guid.Empty;
                    
                }
            }

            return wasDeleted;
        }

        /// <summary>
        /// Updates the mime type, persisted data, data length (based on the data) and linked object details, but will not update the scope or name
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            bool updated = false;
            
            if (this.Id != Guid.Empty)
            {
                var result = HttpHelper.Put<PersistedClientData>(GlobalConfiguration.Routes.PersistedDataId, string.Empty, this.CurrentToken, this, this.Id);

                if (result != null)
                {
                    if (result.Meta.IsAffirmativeStatus())
                    {
                        this.ModifiedByUsId = result.ModifiedByUsId;
                        this.ModifiedDateUtc = result.ModifiedDateUtc;
                        this.DataLength = result.DataLength;
                        updated = true;
                    }
                }  
            }
            

            return updated;
        }


        #endregion
    }
}