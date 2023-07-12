using MyApiMonitorClassLibrary.Interfaces;

namespace MyApiMonitorClassLibrary.Models
{
    public class ChartDataProcessor : IChartDataProcessor
    {
        private readonly IApiTestDataAccess _dataAccess;

        public ChartDataProcessor(IApiTestDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        public List<ChartData_SpeedsByDateTime> SpeedsByDateTime(List<ApiTestData> testData)
        {

            List<ChartData_SpeedsByDateTime> output;

            output = testData.GroupBy(g => g.TestDateTime).Select(g => new ChartData_SpeedsByDateTime(
                g.Key
                , (int?)g.Average(x => x.TimeToComplete) ?? 0
                , (int?)g.Max(x => x.TimeToComplete) ?? 0
                , (int?)g.Min(x => x.TimeToComplete) ?? 0
                )).ToList();

            return output;
        }

        public List<ChartData_ResultByDateTime> ResultByDateTime(List<ApiTestData> testData)
        {
            List<ChartData_ResultByDateTime> output;

            output = testData.GroupBy(g => g.TestDateTime).Select(g => new ChartData_ResultByDateTime(
                g.Key
                , g.Sum(x => (x.WasSuccessful) ? 1 : 0)
                , g.Sum(x => (x.WasSuccessful) ? 0 : 1)
              )).ToList();

            return output;

        }

        public List<ChartData_ResultAndSpeedByTest> ResultAndSpeedByTest(List<ApiTestData> testData)
        {
            List<ChartData_ResultAndSpeedByTest> output;


            var grouped = testData.GroupBy(x => x.TestId).Select(g => new
            {
                TestId = g.Key
               ,
                TestTitle = g.First().TestTitle
               ,
                AverageResult = g.Average(x => x.WasSuccessful ? 100 : 0)
               ,
                AverageTimeToComplete = (int)(g.Average(x => x.TimeToComplete) ?? 0)
               ,
                LatestTestDateTime = g.Max(x => x.TestDateTime)

            }).ToList();

            var latest = grouped.Select(g => new
            {
                TestId = g.TestId,

                TestTitle = g.TestTitle,

                AverageResult = g.AverageResult,

                AverageTimeToComplete = g.AverageTimeToComplete,

                LatestResult = testData.Where(x => x.TestDateTime == g.LatestTestDateTime && x.TestTitle == g.TestTitle).Select(x => x.WasSuccessful).FirstOrDefault(),

                LatestTimeToComplete = (int)(testData.Where(x => x.TestDateTime == g.LatestTestDateTime && x.TestTitle == g.TestTitle).Select(x => x.TimeToComplete).FirstOrDefault() ?? 0)
            }).ToList();





            //var grouped = testData.GroupBy(x => x.TestTitle).Select(g => new
            //{
            //    TestTitle = g.Key
            //   ,
            //    AverageResult = g.Average(x => x.WasSuccessful ? 100 : 0)
            //   ,
            //    AverageTimeToComplete = (int)(g.Average(x => x.TimeToComplete) ?? 0)
            //   ,
            //    LatestTestDateTime = g.Max(x => x.TestDateTime)

            //}).ToList();

            //var latest = grouped.Select(g => new
            //{
            //    TestTitle = g.TestTitle
            //                            ,
            //    AverageResult = g.AverageResult
            //                            ,
            //    AverageTimeToComplete = g.AverageTimeToComplete
            //                            ,
            //    LatestResult = testData.Where(x => x.TestDateTime == g.LatestTestDateTime && x.TestTitle == g.TestTitle).Select(x => x.WasSuccessful).FirstOrDefault()
            //                            ,
            //    LatestTimeToComplete = (int)(testData.Where(x => x.TestDateTime == g.LatestTestDateTime && x.TestTitle == g.TestTitle).Select(x => x.TimeToComplete).FirstOrDefault() ?? 0)
            //}).ToList();

            output = latest.Select(g => new ChartData_ResultAndSpeedByTest(
                                  g.TestTitle.Split('-')[0].Trim()
                                  , g.TestTitle.Split("-")[1].Trim()
                                  , g.AverageResult
                                  , g.LatestResult
                                  , g.AverageTimeToComplete
                                  , g.LatestTimeToComplete
                                  , g.TestId
                                )).ToList();

            return output;

        }

        public List<ChartData_SpeedsByDateTime> AvailabilityByDateTime(List<ApiTestData> testData)
        {
            List<ChartData_SpeedsByDateTime> output;

            output = testData.Select(x => new ChartData_SpeedsByDateTime(x.TestDateTime, x.TimeToComplete, null, null)).ToList();

            return output;
        }


    }
}
