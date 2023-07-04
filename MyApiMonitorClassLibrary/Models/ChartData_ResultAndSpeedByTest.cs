namespace MyApiMonitorClassLibrary.Models
{
    public class ChartData_ResultAndSpeedByTest

    {
        /// <summary>
        /// The controller being tested.
        /// </summary>
        public string Controller { get; set; } = string.Empty;

        /// <summary>
        /// The test being performed on the controller.
        /// </summary>
        public string Test { get; set; } = string.Empty;

        /// <summary>
        /// Percentage of tests that ran successfully.
        /// </summary>
        public double AverageResult { get; set; }

        /// <summary>
        /// The most recent test result.
        /// </summary>
        public bool LatestResult { get; set; }

        /// <summary>
        /// The average time to complete the test in ms.
        /// </summary>
        public int AverageTimeToComplete { get; set; }

        /// <summary>
        /// The most recent time to complete the test in ms.
        /// </summary>
        public int LatestTimeToComplete { get; set; }



        public ChartData_ResultAndSpeedByTest(string controller, string test, double averageResult, bool latestResult, int averageTimeToComplete, int latestTimeToComplete)
        {
            Controller = controller;
            Test = test;
            AverageResult = averageResult;
            LatestResult = LatestResult;
            AverageTimeToComplete = averageTimeToComplete;
            LatestTimeToComplete = latestTimeToComplete;
        }

    }
}
