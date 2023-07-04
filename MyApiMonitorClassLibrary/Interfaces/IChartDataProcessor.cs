using MyApiMonitorClassLibrary.Models;

namespace MyApiMonitorClassLibrary.Interfaces
{
    public interface IChartDataProcessor
    {
        public List<ChartData_TestDateTimeSuccessFailure> SuccessOrFailureByDateTime(Guid collectionID, DateTime? startDate, DateTime? endDate, int skip, int limit);

        public List<ChartData_TestDateTimeSpeed> SpeedsByDateTime(Guid collectionID, DateTime? startDate, DateTime? endDate, int skip, int limit);

    }
}
