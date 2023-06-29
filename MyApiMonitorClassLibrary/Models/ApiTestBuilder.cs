using System.Net;

namespace MyApiMonitorClassLibrary.Models
{
    public class ApiTestBuilder
    {
        ApiTest apiTest;

        public ApiTestBuilder(Guid id, string title)
        {
            apiTest = new ApiTest(id, title);
        }


        public ApiTestBuilder(string id, string title)
        {
            apiTest = new ApiTest(id, title);
        }

        public ApiTestBuilder AddRequest(HttpMethod method, string uri, string? body = null)
        {
            apiTest.RequestMethod = method;
            apiTest.RequestUri = uri;
            apiTest.RequestBody = body;

            return this;
        }

        public ApiTestBuilder RemoveAuthentication()
        {
            apiTest.RemoveAuthentication = true;
            return this;
        }

        public ApiTestBuilder UseAuthentication()
        {
            apiTest.RemoveAuthentication = false;
            return this;
        }

        public ApiTestBuilder ExpectedStatusCode(HttpStatusCode statusCode)
        {
            apiTest.ExpectedStatusCode = statusCode;
            return this;
        }

        public ApiTestBuilder ExpectedResponseMessage(string message)
        {
            apiTest.ExpectedResponseMessage = message;
            return this;
        }

        public ApiTestBuilder ExpectedResponseTime(int time)
        {
            apiTest.ExpectedResponseTime = time;
            return this;
        }

        public ApiTest Build()
        {
            return apiTest;
        }

    }
}

