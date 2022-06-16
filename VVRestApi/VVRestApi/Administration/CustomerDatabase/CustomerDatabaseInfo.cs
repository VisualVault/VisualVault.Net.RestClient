using System;
using VVRestApi.Common;

namespace VVRestApi.Administration.Customers
{

    /// <summary>
    /// provides information about the customer database in use
    /// </summary>
    public class CustomerDatabaseInfo : RestObject
    {
        /// <summary>
        /// Gets or sets the customer alias.
        /// </summary>
        public string CustomerAlias { get; set; }

        /// <summary>
        /// Gets or sets the customer database alias.
        /// </summary>
        public string CustomerDatabaseAlias { get; set; }

        /// <summary>
        /// Gets or sets the customer database description.
        /// </summary>
        public string CustomerDatabaseDescription { get; set; }

        /// <summary>
        /// Gets or sets the customer database id.
        /// </summary>
        public Guid CustomerDatabaseId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customer database is active.
        /// </summary>
        public bool CustomerDatabaseIsActive { get; set; }

        /// <summary>
        /// Gets or sets the customer database name.
        /// </summary>
        public string CustomerDatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the customer database visual vault name.
        /// </summary>
        public string CustomerDatabaseVisualVaultName { get; set; }

        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is billable.
        /// </summary>
        public bool IsBillable { get; set; }

        /// <summary>
        /// Gets or sets the time zone.
        /// </summary>
        public TimeZoneInfo CustomerTimeZone { get; set; }

        /// <summary>
        /// Gets or sets the vault database version.
        /// </summary>
        public string VaultDatabaseVersion { get; set; }
    }
}