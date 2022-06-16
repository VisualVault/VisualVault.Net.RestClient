namespace VVRestApi.Administration.Customers
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Common;

    /// <summary>
    /// 
    /// </summary>
    public class CustomerDatabase : RestObject
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomerDatabase()
        {
            this.Name = string.Empty;
            this.Alias = string.Empty;
            this.Description = string.Empty;
            this.Version = string.Empty;
        }

        /// <summary>
        ///     The Id of the customer
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; internal set; }

        /// <summary>
        ///     The name of the site
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     The alias of the site
        /// </summary>
        [JsonProperty(PropertyName = "alias")]
        public string Alias { get; set; }

        /// <summary>
        ///     The description of the site
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        ///     True if the customer is enabled, false if the customer is disabled
        /// </summary>
        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        ///     The name of the site
        /// </summary>
        [JsonProperty(PropertyName = "timeZone")]
        public Common.TimeZone TimeZone { get; set; }

        /// <summary>
        ///     The description of the site
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        /// <summary>
        ///     The Id of the customer
        /// </summary>
        [JsonProperty(PropertyName = "customerId")]
        public Guid CustomerId { get; internal set; }


    }
}