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
        public List<ChartData_TestDateTimeSpeed> SpeedsByDateTime(Guid collectionId, DateTime? startDate, DateTime? endDate)
        {

            List<ChartData_TestDateTimeSpeed> output;

            List<ApiTestData> rawData = _dataAccess.GetAllBetweenDates(collectionId, startDate, endDate);

            output = rawData.GroupBy(g => g.TestDateTime).Select(g => new ChartData_TestDateTimeSpeed(
                g.Key
                , (int?)g.Average(x => x.TimeToComplete) ?? 0
                , (int?)g.Max(x => x.TimeToComplete) ?? 0
                , (int?)g.Min(x => x.TimeToComplete) ?? 0
                )).ToList();

            return output;
        }

        public List<ChartData_TestDateTimeSuccessFailure> SuccessOrFailureByDateTime(Guid collectionId, DateTime? startDate, DateTime? endDate)
        {
            List<ChartData_TestDateTimeSuccessFailure> output;

            List<ApiTestData> rawData = _dataAccess.GetAllBetweenDates(collectionId, startDate, endDate);

            output = rawData.GroupBy(g => g.TestDateTime).Select(g => new ChartData_TestDateTimeSuccessFailure(
                g.Key
                , g.Sum(x => (x.WasSuccessful) ? 1 : 0)
                , g.Sum(x => (x.WasSuccessful) ? 0 : 1)
              )).ToList();

            return output;

        }

        public List<ChartData_ControllerTestResultSpeed> ResultAndSpeedByControllerTest(Guid collectionId, DateTime? startDate, DateTime? endDate)
        {
            List<ChartData_ControllerTestResultSpeed> output;

            List<ApiTestData> rawData = _dataAccess.GetAllBetweenDates(collectionId, startDate, endDate);

            output = rawData.GroupBy(x => x.TestTitle).Select(g => new
            {
                TestTitle = g.Key
               ,
                AverageResult = g.Average(x => x.WasSuccessful ? 100 : 0)
               ,
                AverageTimeToComplete = (int)(g.Average(x => x.TimeToComplete) ?? 0)
               ,
                LatestTestDateTime = g.Max(x => x.TestDateTime)

            }).Select(g => new
            {
                TestTitle = g.TestTitle
                ,
                AverageResult = g.AverageResult
                ,
                AverageTimeToComplete = g.AverageTimeToComplete
                ,
                LatestResult = rawData.Where(x => x.TestDateTime == g.LatestTestDateTime && x.TestTitle == g.TestTitle).Select(x => x.WasSuccessful).FirstOrDefault()
                ,
                LatestTimeToComplete = (int)(rawData.Where(x => x.TestDateTime == g.LatestTestDateTime && x.TestTitle == g.TestTitle).Select(x => x.TimeToComplete).FirstOrDefault() ?? 0)
            }).Select(g => new ChartData_ControllerTestResultSpeed(
                  g.TestTitle
                  , g.TestTitle
                  , g.AverageResult
                  , g.LatestResult
                  , g.AverageTimeToComplete
                  , g.LatestTimeToComplete
                )).ToList();

            return output;

        }

    }
}
