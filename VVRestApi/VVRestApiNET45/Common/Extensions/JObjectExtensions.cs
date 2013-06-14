namespace VVRestApi.Common.Extensions
{
    using System;
    using System.Net;

    using Newtonsoft.Json.Linq;

    public static class JObjectExtensions
    {
        /// <summary>
        /// Returns true if the source.meta.status matches the passed in status
        /// </summary>
        /// <param name="source"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool IsHttpStatus(this JObject source, HttpStatusCode status)
        {
            var sourceStatus = GetHttpStatus(source);
            return sourceStatus == status;
        }

        /// <summary>
        /// Gets the status from the source.meta.status
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static HttpStatusCode GetHttpStatus(this JObject source)
        {
            HttpStatusCode result = HttpStatusCode.InternalServerError;
            if (source != null)
            {
                if (source["meta"] != null && source["meta"]["status"] != null)
                {
                    result = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), ((int)source["meta"]["status"]).ToString(), true);
                }
            }

            return result;
        }
    }
}