using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace MyApiMonitorFunctions
{
    public class RunTestCollections
    {
        [FunctionName("RunTestCollections")]
        public void RunCollections([TimerTrigger("0 15 */4 * * *")] TimerInfo myRunCollectionsTimer, ILogger log)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("https://myapimonitorserviceapi.azurewebsites.net/");

            var getTask = client.GetAsync("runtestcollections");
            getTask.Wait();
            HttpResponseMessage response = getTask.Result;

            log.LogInformation($"MyApiMonitorServiceApi ran at {DateTime.Now} with response: {response.StatusCode}");


            // log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        [FunctionName("RunAvailabilityTests")]
        public void RunAvailabilityTests([TimerTrigger("*/5 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://myapimonitorserviceapi.azurewebsites.net/");
            var getTask = client.GetAsync("runavailabilitytests");
            getTask.Wait();
            HttpResponseMessage response = getTask.Result;

            //log.LogInformation($"AvailabilityTest ran at {DateTime.Now} with response: {response.StatusCode}");
        }

    }
}
