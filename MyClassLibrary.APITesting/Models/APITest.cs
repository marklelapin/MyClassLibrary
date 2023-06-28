using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.APITesting.Models
{
    /// <summary>
    /// Carries the congfiguration information required for IAPITestRunner
    /// </summary>
    public class APITest
    {
        /// <summary>
        /// An integer identifying the test.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The title of the test.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The HttpMethod to be used by the test api call.
        /// </summary>
        public HttpMethod RequestMethod { get; set; }
        /// <summary>
        /// The url to be used by the test api call.
        /// </summary>
        public string RequestUrl { get; set; }
        /// <summary>
        /// The request body to be used by the test api call (if required).
        /// </summary>
        public string? RequestBody { get; set; } = null;

        /// <summary>
        /// The response code expected from running the api test.
        /// </summary>
        public HttpStatusCode ExpectedStatusCode { get; set; }

        /// <summary>
        /// The expected response message to test against from the api call.
        /// </summary>
        /// <remarks>
        /// If you do not want to test the response message leave as null.
        /// </remarks>
        public string? ExpectedResponseMessage { get; set; } = null;

        /// <summary>
        /// The maximum response time required. If the test takes longer than this time it will fail.
        /// </summary>
        /// If you do not want to test the response message leave as null.
        /// </remarks>
        public int? ExpectedResponseTime { get; set; } = null;


        /// <summary>
        /// The result of the latest test run.
        /// </summary>
        public APITestResult? TestResult { get; set; }

        public APITest(string title, HttpMethod reqeustMethod, string requestUri, string? requestBody = null, HttpStatusCode expectedStatusCode, string? expectedResponseMessage = null, int? expectedResponseTime = null)
        {
            Title = title;
            RequestMethod = reqeustMethod;
            RequestUrl = requestUri;
            RequestBody = requestBody;
            ExpectedStatusCode = expectedStatusCode;
            ExpectedResponseMessage = expectedResponseMessage;
            ExpectedResponseTime = expectedResponseTime;
        }

        public APITest(string title, HttpMethod requestMethod, string requestUri, HttpStatusCode expectedStatusCode)
        {
            Title = title;
            RequestMethod = requestMethod;
            RequestUrl = requestUri;
            ExpectedStatusCode = expectedStatusCode;
        }

        public APITest(string title, HttpMethod requestMethod, string requestUri, HttpStatusCode expectedStatusCode, int expectedResponseTime)
        {
            Title = title;
            RequestMethod = requestMethod;
            RequestUrl = requestUri;
            ExpectedStatusCode = expectedStatusCode;
            ExpectedResponseTime = expectedResponseTime;
        }

        public APITest(string title, HttpMethod requestMethod, string requestUri, HttpStatusCode expectedStatusCode, string expectedResponseMessage)
        {
            Title = title;
            RequestMethod = requestMethod;
            RequestUrl = requestUri;
            ExpectedStatusCode = expectedStatusCode;
            ExpectedResponseMessage = expectedResponseMessage;
        }

        public APITest(string title, HttpMethod requestMethod, string requestUri, HttpStatusCode expectedStatusCode, string expectedResponseMessage, int expectedResponseTime)
        {
            Title = title;
            RequestMethod = requestMethod;
            RequestUrl = requestUri;
            ExpectedStatusCode = expectedStatusCode;
            ExpectedResponseMessage = expectedResponseMessage;
            ExpectedResponseTime = expectedResponseTime;
        }

        public APITest(string title, HttpMethod requestMethod, string requestUri, string requestBody, HttpStatusCode expectedStatusCode)
        {
            Title = title;
            RequestMethod = requestMethod;
            RequestUrl = requestUri;
            RequestBody = requestBody;
            ExpectedStatusCode = expectedStatusCode;
        }

        public APITest(string title, HttpMethod requestMethod, string requestUri, string requestBody, HttpStatusCode expectedStatusCode, int expectedResponseTime)
        {
            Title = title;
            RequestMethod = requestMethod;
            RequestUrl = requestUri;
            RequestBody = requestBody;
            ExpectedStatusCode = expectedStatusCode;
            ExpectedResponseTime = expectedResponseTime;
        }

        public APITest(string title, HttpMethod requestMethod, string requestUri, string requestBody, HttpStatusCode expectedStatusCode, string expectedResponseMessage)
        {
            Title = title;
            RequestMethod = requestMethod;
            RequestUrl = requestUri;
            RequestBody = requestBody;
            ExpectedStatusCode = expectedStatusCode;
            ExpectedResponseMessage = expectedResponseMessage;
        }
    }

    public APITest(string title, HttpMethod requestMethod, string requestUri, string requestBody, HttpStatusCode expectedStatusCode, string expectedResponseMessage, int expectedResponseTime)
    {
        Title = title;
        RequestMethod = requestMethod;
        RequestUrl = requestUri;
        RequestBody = requestBody;
        ExpectedStatusCode = expectedStatusCode;
        ExpectedResponseMessage = expectedResponseMessage;
        ExpectedResponseTime = expectedResponseTime;
    }
}
}
