using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyApiMonitorServiceFunction
{
    public class RunTestCollections
    {
        [FunctionName("RunTestCollections")]
        public async Task Run([TimerTrigger("0 */127 * * * *")] TimerInfo myTimer, ILogger log)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://myapimonitorserviceapi.azurewebsites.net/");

            HttpResponseMessage response = await client.GetAsync("runtestcollections");

            log.LogInformation($"MyApiMonitorServiceApi ran at {DateTime.Now} with response: {response.StatusCode}");

        }
    }
}

