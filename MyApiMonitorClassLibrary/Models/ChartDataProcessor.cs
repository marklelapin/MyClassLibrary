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


        public List<ChartData_SpeedsByDateTime> SpeedsByDateTime(Guid collectionId, DateTime? startDate, DateTime? endDate, int skip, int limit)
        {

            List<ChartData_SpeedsByDateTime> output;

            (List<ApiTestData> records, int totalRecords) = _dataAccess.GetAllBetweenDates(collectionId, startDate, endDate, skip, limit);

            output = records.GroupBy(g => g.TestDateTime).Select(g => new ChartData_SpeedsByDateTime(
                g.Key
                , (int?)g.Average(x => x.TimeToComplete) ?? 0
                , (int?)g.Max(x => x.TimeToComplete) ?? 0
                , (int?)g.Min(x => x.TimeToComplete) ?? 0
                )).ToList();

            return output;
        }

        public List<ChartData_ResultByDateTime> ResultByDateTime(Guid collectionId, DateTime? startDate, DateTime? endDate, int skip, int limit)
        {
            List<ChartData_ResultByDateTime> output;

            (List<ApiTestData> records, int totalRecords) = _dataAccess.GetAllBetweenDates(collectionId, startDate, endDate, skip, limit);

            output = records.GroupBy(g => g.TestDateTime).Select(g => new ChartData_ResultByDateTime(
                g.Key
                , g.Sum(x => (x.WasSuccessful) ? 1 : 0)
                , g.Sum(x => (x.WasSuccessful) ? 0 : 1)
              )).ToList();

            return output;

        }

        public List<ChartData_ResultAndSpeedByTest> ResultAndSpeedByTest(Guid collectionId, DateTime? startDate, DateTime? endDate, int skip, int limit)
        {
            List<ChartData_ResultAndSpeedByTest> output;

            (List<ApiTestData> records, int totalRecords) = _dataAccess.GetAllBetweenDates(collectionId, startDate, endDate, skip, limit);


            output = records.GroupBy(x => x.TestTitle).Select(g => new
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
                LatestResult = records.Where(x => x.TestDateTime == g.LatestTestDateTime && x.TestTitle == g.TestTitle).Select(x => x.WasSuccessful).FirstOrDefault()
                ,
                LatestTimeToComplete = (int)(records.Where(x => x.TestDateTime == g.LatestTestDateTime && x.TestTitle == g.TestTitle).Select(x => x.TimeToComplete).FirstOrDefault() ?? 0)
            }).Select(g => new ChartData_ResultAndSpeedByTest(
                  g.TestTitle
                  , g.TestTitle
                  , g.AverageResult
                  , g.LatestResult
                  , g.AverageTimeToComplete
                  , g.LatestTimeToComplete
                )).ToList();

            return output;

        }

        public List<ChartData_SpeedsByDateTime> AvailabilityByDateTime(Guid collectionId, DateTime? startDate, DateTime? endDate, int skip, int limit)
        {
            List<ChartData_SpeedsByDateTime> output;
            (List<ApiTestData> records, int totalRecords) = _dataAccess.GetAllBetweenDates(collectionId, startDate, endDate, skip, limit);

            output = records.Select(x => new ChartData_SpeedsByDateTime(x.TestDateTime, x.TimeToComplete, null, null)).ToList();

            return output;
        }


    }
}
