using MyApiMonitorClassLibrary.Models;

namespace MyApiMonitorClassLibrary.Interfaces
{
    public interface IChartDataProcessor
    {
        public List<ChartData_ResultByDateTime> ResultByDateTime(Guid collectionID, DateTime? startDate, DateTime? endDate, int skip, int limit);

        public List<ChartData_SpeedsByDateTime> SpeedsByDateTime(Guid collectionID, DateTime? startDate, DateTime? endDate, int skip, int limit);

        public List<ChartData_SpeedsByDateTime> AvailabilityByDateTime(Guid collectionId, DateTime? startDate, DateTime? endDate, int skip, int limit);

        public List<ChartData_ResultAndSpeedByTest> ResultAndSpeedByTest(Guid collectionId, DateTime? startDate, DateTime? endDate, int skip, int limit);

    }
}
