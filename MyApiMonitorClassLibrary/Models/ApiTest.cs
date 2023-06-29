using System.Net;

namespace MyApiMonitorClassLibrary.Models
{

    /// <summary>
    /// Carries the congfiguration information required for IAPITestRunner
    /// </summary>
    public class ApiTest
    {
        /// <summary>
        /// An guid identifying the test.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The title of the test.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The HttpMethod to be used by the test api call.
        /// </summary>
        public HttpMethod? RequestMethod { get; set; }
        /// <summary>
        /// The url to be used by the test api call.strin
        /// </summary>
        public string? RequestUri { get; set; }
        /// <summary>
        /// The request body to be used by the test api call (if required).
        /// </summary>
        public string? RequestBody { get; set; } = null;

        /// <summary>
        /// If set to true will remove authentication from the HttpMessageRequest to test unauthorized access.
        /// </summary>
        public bool? RemoveAuthentication { get; set; } = false;


        /// <summary>
        /// The response code expected from running the api test.
        /// </summary>
        public HttpStatusCode? ExpectedStatusCode { get; set; }

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
        public APITestResult TestResult { get; set; } = new APITestResult();



        public ApiTest(Guid id, string title)
        {
            Id = id;
            Title = title;
        }

        public ApiTest(string id, string title)
        {
            Id = Guid.Parse(id);
            Title = title;
        }

    }


}
