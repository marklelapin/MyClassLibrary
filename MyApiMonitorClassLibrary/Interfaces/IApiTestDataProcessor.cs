using MyApiMonitorClassLibrary.Models;

namespace MyApiMonitorClassLibrary.Interfaces
{
    public interface IApiTestDataProcessor
    {
        public List<TestDateTimeSuccessFailure> SuccessOrFailureByDateTime(Guid collectionID, DateTime? startDate, DateTime? endDate);

        public List<TestDateTimeSpeed> SpeedsByDateTime(Guid collectionID, DateTime? startDate, DateTime? endDate);

    }
}
