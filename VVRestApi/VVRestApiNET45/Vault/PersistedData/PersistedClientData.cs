namespace VVRestApi.Vault.PersistedData
{
    using System;

    using VVRestApi.Common;

    /// <summary>
    /// Allows for the creation and updating of text data to a table, PersistedClientData, in the vault
    /// </summary>
    public class PersistedClientData : RestObject
    {
        public PersistedClientData()
        {
            this.Id = Guid.NewGuid();
        }

        #region Public Properties
       
        /// <summary>
        /// Gets or sets the create by us id.
        /// </summary>
        public Guid CreateByUsId { get; set; }

        /// <summary>
        /// Gets or sets the create date utc.
        /// </summary>
        public DateTime CreateDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public string PersistedData { get; set; }

        /// <summary>
        /// Gets or sets the data length.
        /// </summary>
        public long DataLength { get; set; }

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
        public Guid Id { get; set; }

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
        public Guid ModifiedByUsId { get; set; }

        /// <summary>
        /// Gets or sets the modified date utc.
        /// </summary>
        public DateTime ModifiedDateUtc { get; set; }

       
        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        public ScopeType Scope { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}