namespace VVRestApi.Common
{
    using System;
    using System.Net;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using VVRestApi.Common.Logging;
    using VVRestApi.Common.Messaging;

    public abstract class RestObject : BaseApi
    {
        public RestObject()
        {
            Href = string.Empty;
            Meta = new ApiMetaData();
        }

        internal void PopulateSessionToken(SessionToken token)
        {
            base.Populate(token);

            this.SessionPopulated();
        }

        internal virtual void SessionPopulated()
        {

        }

        internal void Populate(JToken meta, JToken data, SessionToken sessionToken)
        {
            this.Populate(sessionToken);

            //Now populate the meta
            try
            {
                this.Meta = meta.ToObject<ApiMetaData>();
            }
            catch (Exception ex)
            {
                LogEventManager.Error("Error deserializing the Meta node.", ex);
                this.Meta = new ApiMetaData();
                this.Meta.ErrorMessages.Add(new ApiErrorMessage() { DeveloperMessage = ex.Message });
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

        public ApiMetaData Meta { get; set; }
    }
}