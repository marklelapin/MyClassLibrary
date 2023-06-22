
using Azure;
using Microsoft.Identity.Abstractions;
using MongoDB.Bson;
using MyApiMonitorService.Interfaces;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Timers;

namespace MyApiMonitorService.Models
{
    internal class ApiTestRunner : IApiTestRunner
    {
        private readonly IApiTestingDataAccess _dataAccess;
        private readonly IDownstreamApi _downstreamApi;

        public ApiTestRunner(IApiTestingDataAccess dataAccess, IDownstreamApi downstreamApi)
        {
            _dataAccess = dataAccess;
            _downstreamApi = downstreamApi;
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

                    GetAndTimeApiResponse(test, response);

                    PerformTests(test, response);

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



        private void GetAndTimeApiResponse(ApiTest test, HttpResponseMessage? response)
        {
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                //TODO - allo fow different API's to be called. Also fix this call.
                stopwatch.Start();

                HttpContent? content = (test.RequestBody==null) ? null : new StringContent(test.RequestBody,Encoding.UTF8,"application/json");

               var callTask = _downstreamApi.CallApiForAppAsync( "DownstreamApi"
                                                               , options =>
                                                               {
                                                                   options.HttpMethod = test.RequestMethod;
                                                                   options.RelativePath = test.RequestUrl;
                                                               }
                                                               , content
                                                                );
                callTask.Wait();
                stopwatch.Stop();
                response = callTask.Result;
            }
            catch (Exception ex)
            {
                test.TestResult.WasSuccessful = false;
                test.TestResult.FailureMessage = "Application Error.";
                test.TestResult.ActualResult = ex.Message;
            }


            test.TestResult.TimeToComplete = (int)stopwatch.Elapsed.TotalMilliseconds;
        }


        private void PerformTests(ApiTest test, HttpResponseMessage? response)
        {
            //set inital success result to true
            test.TestResult.WasSuccessful = true;

            if (response == null)
            {
                test.TestResult.WasSuccessful = false;
                test.TestResult.FailureMessage = "Application error. Response is null.";
            }
            else
            {
                //Run test to trun was successful into false if they fail.
                if (TestStatusCode(test, response) == true)
                {
                    if (TestResponseMessage(test, response) == true)
                    {
                        if (TestResponseTime(test, response) == true)
                        {
                            test.TestResult.WasSuccessful = true;
                        };
                    };
                }
            }
        }




        //Helper methods
        private bool TestStatusCode(ApiTest test, HttpResponseMessage response)
        {
            if (test.ExpectedStatusCode != response.StatusCode)
            {
                test.TestResult.WasSuccessful = false;
                test.TestResult.FailureMessage = "Incorred HttpStatusCode.";
                test.TestResult.ExpectedResult = test.ExpectedStatusCode.ToString();
                test.TestResult.ActualResult = response.StatusCode.ToString();
                return false;
            }

            return true;
        }


        private bool TestResponseMessage(ApiTest test, HttpResponseMessage response)
        {
            if (test.ExpectedResponseMessage != null && test.ExpectedResponseMessage != response.Content.ToString()) //TODO = check if this can realte to HTTPResponseMessage extensions in MyLibrary
            {
                test.TestResult.WasSuccessful = false;
                test.TestResult.FailureMessage = "Response Message didn't match expected.";
                test.TestResult.ExpectedResult = test.ExpectedResponseMessage;
                test.TestResult.ActualResult = response.Content.ToString();
                return false;
            }

            return true;
        }

        private bool TestResponseTime(ApiTest test, HttpResponseMessage response)
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
