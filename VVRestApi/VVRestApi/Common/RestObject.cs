using VVRestApi.Common.Logging;
using VVRestApi.Common.Messaging;

namespace VVRestApi.Common
{
    using System;
    using System.Net;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using VVRestApi.Common.Logging;
    using VVRestApi.Common.Messaging;

    /// <summary>
    /// 
    /// </summary>
    public abstract class RestObject : BaseApi
    {
        /// <summary>
        /// 
        /// </summary>
        protected RestObject()
        {
            Href = string.Empty;
            Meta = new ApiMetaData();
        }

        internal void PopulateAccessToken(IClientSecrets clientSecrets, Tokens apiTokens)
        {
            base.Populate(clientSecrets, apiTokens);

            this.SessionPopulated();
        }

        internal virtual void SessionPopulated()
        {

        }

        internal void Populate(JToken meta, JToken data, ClientSecrets clientSecrets, Tokens apiTokens)
        {
            this.Populate(clientSecrets, apiTokens);

            //Now populate the meta
            try
            {
                this.Meta = meta.ToObject<ApiMetaData>();
            }
            catch (Exception ex)
            {
                LogEventManager.Error("Error deserializing the Meta node.", ex);
                this.Meta = new ApiMetaData();
                this.Meta.ErrorMessages.Add(new ApiErrorMessage { DeveloperMessage = ex.Message });
                this.Meta.StatusCode = HttpStatusCode.BadRequest;
            }


            if (data != null)
            {
                this.PopulateData(data);
            }
        }

        internal virtual void PopulateData(JToken data)
        {

        }

        /// <summary>
        /// Location of the object for REST access
        /// </summary>
        [JsonProperty(PropertyName = "href")]
        public string Href { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ApiMetaData Meta { get; set; }
    }
}