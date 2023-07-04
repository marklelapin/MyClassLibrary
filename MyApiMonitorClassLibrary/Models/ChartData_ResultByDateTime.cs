namespace MyApiMonitorClassLibrary.Models
{
    public class ChartData_ResultByDateTime
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

        public ChartData_ResultByDateTime(DateTime testDateTime, int successfulTests, int failedTests)
        {
            TestDateTime = testDateTime;
            SuccessfulTests = successfulTests;
            FailedTests = failedTests;
        }
    }
}
