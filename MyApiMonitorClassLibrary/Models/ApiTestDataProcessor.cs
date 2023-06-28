using MyApiMonitorClassLibrary.Interfaces;

namespace MyApiMonitorClassLibrary.Models
{
    public class ApiTestDataProcessor : IApiTestDataProcessor
    {
        private readonly IApiTestDataAccess _dataAccess;

        public ApiTestDataProcessor(IApiTestDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        public List<TestDateTimeSpeed> SpeedsByDateTime(Guid collectionId, DateTime? startDate, DateTime? endDate)
        {

            List<TestDateTimeSpeed> output;

            List<ApiTestData> rawData = _dataAccess.GetAllBetweenDates(collectionId, startDate, endDate);

            output = rawData.GroupBy(g => g.TestDateTime).Select(g => new TestDateTimeSpeed(
                g.Key
                , (int?)g.Average(x => x.TimeToComplete) ?? 0
                , (int?)g.Max(x => x.TimeToComplete) ?? 0
                , (int?)g.Min(x => x.TimeToComplete) ?? 0
                )).ToList();

            return output;
        }

        public List<TestDateTimeSuccessFailure> SuccessOrFailureByDateTime(Guid collectionId, DateTime? startDate, DateTime? endDate)
        {
            List<TestDateTimeSuccessFailure> output;

            List<ApiTestData> rawData = _dataAccess.GetAllBetweenDates(collectionId, startDate, endDate);

            output = rawData.GroupBy(g => g.TestDateTime).Select(g => new TestDateTimeSuccessFailure(
                g.Key
                , g.Sum(x => (x.WasSuccessful) ? 1 : 0)
                , g.Sum(x => (x.WasSuccessful) ? 0 : 1)
              )).ToList();

            return output;

        }

    }
}
