using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VVRestApiTests.Core.Tests.RestApiTests.Mocks
{
    public class RetryHttpMessageHandlerMock : HttpMessageHandler
    {
        private DateTime OkayAfter;
        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
        {
            dynamic response;
            if (OkayAfter == null || OkayAfter == DateTime.MinValue)
            {
                response = new
                {
                    meta = new
                    {
                        retryTime = OkayAfter = DateTime.UtcNow.AddSeconds(5),
                        status = 429,
                        statusMsg = "TooManyRequests"
                    }
                };
            }
            else if (OkayAfter > DateTime.UtcNow)
            {
                response = new
                {
                    meta = new
                    {
                        retryTime = OkayAfter,
                        status = 429,
                        statusMsg = "TooManyRequests"
                    }
                };
            }
            else
            {
                response = new
                {
                    meta = new
                    {
                        status = 200
                    }
                };
            }

            var resp = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), System.Text.Encoding.UTF8, "application/json")
            };

            return Task.FromResult(resp);

        }
    }
}
