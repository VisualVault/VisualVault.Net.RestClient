﻿namespace VVRestApi.Administration.Customers
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Common;

    /// <summary>
    /// 
    /// </summary>
    public class Customer : RestObject
    {
        /// <summary>
        /// 
        /// </summary>
        public Customer()
        {
            this.Name = string.Empty;
            this.Alias = string.Empty;
            this.Description = string.Empty;
            this.Notes = string.Empty;
            this.Databases = new List<CustomerDatabase>();
        }

        internal override void SessionPopulated()
        {
            base.SessionPopulated();

            foreach (var customerDatabase in Databases)
            {
                customerDatabase.PopulateAccessToken(this.ClientSecrets,this.ApiTokens);
            }
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
        ///     Notes for the customer
        /// </summary>
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }

        /// <summary>
        ///     The number of active users
        /// </summary>
        [JsonProperty(PropertyName = "activeUsers")]
        public int ActiveUsers { get; set; }


        /// <summary>
        ///     The number of inactive users
        /// </summary>
        [JsonProperty(PropertyName = "inactiveUsers")]
        public int InactiveUsers { get; set; }

        /// <summary>
        ///     The name of the site
        /// </summary>
        [JsonProperty(PropertyName = "timeZone")]
        public Common.TimeZone TimeZone { get; set; }


        /// <summary>
        /// A list of the customer databases. These are returned when queries are set to expand the returned customer.
        /// </summary>
        [JsonProperty(PropertyName = "databases")]
        public List<CustomerDatabase> Databases { get; set; }
    }
}