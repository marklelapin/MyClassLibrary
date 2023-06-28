using MyApiMonitorClassLibrary.Interfaces;
using MyClassLibrary.Extensions;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace MyApiMonitorClassLibrary.Models
{
    public class ApiTestRunner : IApiTestRunner
    {
        private readonly IApiTestDataAccess _dataAccess;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;

        public ApiTestRunner(IApiTestDataAccess dataAccess, IHttpClientFactory clientFactory)
        {
            _dataAccess = dataAccess;
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient("DownstreamApi");
        }


        public async Task RunTest(ApiTest test)
        {
            await RunTest(new List<ApiTest> { test });
        }

        public async Task RunTest(List<ApiTest> tests)
        {
            HttpResponseMessage? response = null;

            await Task.Run(() =>
            {


                tests.ForEach((test) =>
                {

                    (string responseMessage, HttpStatusCode statusCode) = GetAndTimeApiResponse(test);

                    PerformTests(test, responseMessage, statusCode);

                });
            });
        }


        public async Task Save(ApiTestCollection testCollection)
        {
            await _dataAccess.Save(testCollection);
        }

        public async Task RunTestAndSave(ApiTestCollection testCollection)
        {
            await RunTest(testCollection.Tests);
            await Save(testCollection);
        }



        private (string responseMessage, HttpStatusCode statusCode) GetAndTimeApiResponse(ApiTest test)
        {
            Stopwatch stopwatch = new Stopwatch();
            (string responseMessage, HttpStatusCode statusCode) output;

            try
            {
                //TODO - allo fow different API's to be called. Also fix this call.
                stopwatch.Start();

                HttpRequestMessage request = new HttpRequestMessage(test.RequestMethod, test.RequestUri);

                request.Content = new StringContent(test.RequestBody ?? "", Encoding.UTF8, "application/json");

                var taskCall = _client.SendAsync(request);



                taskCall.Wait();
                stopwatch.Stop();
                output = taskCall.GetResponseDataAsync().Result;

                return output;
            }
            catch (Exception ex)
            {
                test.TestResult.WasSuccessful = false;
                test.TestResult.FailureMessage = "Monitor Error.";
                test.TestResult.ActualResult = ex.Message;
                output = ("Monitor Error.", HttpStatusCode.InternalServerError);
            }


            test.TestResult.TimeToComplete = (int)stopwatch.Elapsed.TotalMilliseconds;

            return output;
        }


        private void PerformTests(ApiTest test, string response, HttpStatusCode statusCode)
        {
            //set inital success result to true
            test.TestResult.WasSuccessful = true;

            if (response == "Monitor Error.")
            {
                test.TestResult.WasSuccessful = false;
                test.TestResult.FailureMessage = "Monitor error. Response is null.";
            }
            else
            {
                //Run test to turn was successful into false if they fail.
                if (TestStatusCode(test, statusCode, response) == true)
                {
                    if (TestResponseMessage(test, response) == true)
                    {
                        if (TestResponseTime(test) == true)
                        {
                            test.TestResult.WasSuccessful = true;
                        };
                    };
                }
            }
        }




        //Helper methods
        private bool TestStatusCode(ApiTest test, HttpStatusCode actualstatusCode, string response)
        {
            if (test.ExpectedStatusCode != actualstatusCode)
            {
                test.TestResult.WasSuccessful = false;
                test.TestResult.FailureMessage = "Incorrect HttpStatusCode.";
                test.TestResult.ExpectedResult = $"{(int)test.ExpectedStatusCode}({test.ExpectedStatusCode})";
                test.TestResult.ActualResult = $"{(int)actualstatusCode}({actualstatusCode}):\n{response}";
                return false;
            }

            return true;
        }


        private bool TestResponseMessage(ApiTest test, string response)
        {
            if (test.ExpectedResponseMessage != null && test.ExpectedResponseMessage != response) //TODO = check if this can realte to HTTPResponseMessage extensions in MyLibrary
            {
                test.TestResult.WasSuccessful = false;
                test.TestResult.FailureMessage = "Response Message didn't match expected.";
                test.TestResult.ExpectedResult = test.ExpectedResponseMessage;
                test.TestResult.ActualResult = response;
                return false;
            }

            return true;
        }

        private bool TestResponseTime(ApiTest test)
        {
            if (test.ExpectedResponseTime != null && test.ExpectedResponseTime < test.TestResult.TimeToComplete)
            {
                test.TestResult.WasSuccessful = false;
                test.TestResult.FailureMessage = "Response was too slow.";
                test.TestResult.ExpectedResult = $"Less than {test.ExpectedResponseTime}ms";
                test.TestResult.ActualResult = $"{test.TestResult.TimeToComplete}ms";
                return false;
            }
            return true;
        }

    }
}
