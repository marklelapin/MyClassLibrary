

namespace MyApiMonitorClassLibrary.Models
{
    /// <summary>
    /// Carries the information obtained from a test run of an APItest
    /// </summary>
    public class APITestResult
    {

        /// <summary>
        /// Whether or not the test was successfull.
        /// </summary>
        public bool WasSuccessful { get; set; }

        /// <summary>
        /// The time in milliseconds taken to complete the API request.
        /// </summary>
        public int TimeToComplete { get; set; }

        /// <summary>
        /// Message summarising briefly what failed if the test wasn't successful.
        /// </summary>
        public string? FailureMessage { get; set; }

        /// <summary>
        /// The result expected from the test.
        /// </summary>
        public string? ExpectedResult { get; set; }

        /// <summary>
        /// The actual result from the test.
        /// </summary>
        public string? ActualResult { get; set; }




    }
}
