namespace VVRestApi.Common
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

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
            this.Meta = new ApiMetaData(meta);

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