namespace VVRestApi.Common
{
    using System;

    using Newtonsoft.Json;

    public class TimeZone
    {
         /// <summary>
        ///     The Id of the customer
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; internal set; }

        /// <summary>
        ///     The Id of the customer
        /// </summary>
        [JsonProperty(PropertyName = "DisplayName")]
        public string DisplayName { get; internal set; }

        /// <summary>
        ///     The Id of the customer
        /// </summary>
        [JsonProperty(PropertyName = "StandardName")]
        public string StandardName { get; internal set; }

        /// <summary>
        ///     The Id of the customer
        /// </summary>
        [JsonProperty(PropertyName = "DaylightName")]
        public string DaylightName { get; internal set; }

        /// <summary>
        ///     The Id of the customer
        /// </summary>
        [JsonProperty(PropertyName = "BaseUtcOffset")]
        public string BaseUtcOffset { get; internal set; }

        /// <summary>
        ///     The Id of the customer
        /// </summary>
        [JsonProperty(PropertyName = "SupportsDaylightSavingTime")]
        public bool SupportsDaylightSavingTime { get; internal set; }


    }
}