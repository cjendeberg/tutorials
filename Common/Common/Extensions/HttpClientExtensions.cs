using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Zero99Lotto.SRC.Common.Extensions
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Retries retrieving data x times after HttpRequestException occurred.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAsyncAndRetry(this HttpClient httpClient, string url, int retryCount, ILogger logger)
        {
            HttpResponseMessage httpResponseMessage = default(HttpResponseMessage);

            var retry = Policy.Handle<HttpRequestException>()
                               .WaitAndRetryAsync(retryCount, (counter) =>
                               {
                                   var waitTime = TimeSpan.FromSeconds(Math.Pow(2, counter));
                                   logger.LogInformation($"Attempt: {counter}. Wait time:{waitTime}s");
                                   return waitTime;
                               });

            await retry.ExecuteAsync(async () => httpResponseMessage=await httpClient.GetAsync(url));
            return httpResponseMessage;
        }
    }
}