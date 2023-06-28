namespace MyApiMonitorClassLibrary.Models
{
    public class TestDateTimeSuccessFailure
    {
        /// <summary>
        /// The time the set of tests were run.
        /// </summary>
        public DateTime TestDateTime { get; set; }


        /// <summary>
        /// Total No of successful tests.
        /// </summary>   
        public int SuccessfulTests { get; set; }


        /// <summary>
        /// Total No of failed tests.
        /// </summary>
        public int FailedTests { get; set; }

        public TestDateTimeSuccessFailure(DateTime testDateTime, int successfulTests, int failedTests)
        {
            TestDateTime = testDateTime;
            SuccessfulTests = successfulTests;
            FailedTests = failedTests;
        }
    }
}
