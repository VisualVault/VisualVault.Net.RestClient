using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;

namespace VVRestApi.Common.Messaging
{
    public class RetryHandler : DelegatingHandler
    {
        private const int MaxRetries = 3;

        public RetryHandler() : base(new HttpClientHandler())
        {
        }

        public RetryHandler(HttpMessageHandler messageHandler) : base(messageHandler)
        {

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            for (int i = 0; i < MaxRetries; i++)
            {
                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

                if ((int)response.StatusCode == 429)
                {

                    var resultData = await response.Content.ReadAsAsync<JObject>().ConfigureAwait(false);
                    JObject jData = resultData;

                    if (jData["meta"] != null && jData["meta"]["status"] != null && jData["meta"]["status"].ToString() == "429")
                    {
                        if (jData["meta"]["retryTime"] != null && DateTime.TryParse(jData["meta"]["retryTime"].ToString(), out var retryTime))
                        {
                            var msDiff = (int)(retryTime - DateTime.UtcNow).TotalMilliseconds;
                            if (msDiff < 0)
                                msDiff *= -1;
                            Thread.Sleep(msDiff);
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                    response.Content = new StringContent(JsonConvert.SerializeObject(resultData), System.Text.Encoding.UTF8, "application/json");

                }
                else
                {
                    return response;
                }
            }

            return response;
        }
    }

}
