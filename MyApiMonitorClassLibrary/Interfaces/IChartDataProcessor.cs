using MyApiMonitorClassLibrary.Models;

namespace MyApiMonitorClassLibrary.Interfaces
{
    public interface IChartDataProcessor
    {
        public List<ChartData_ResultByDateTime> ResultByDateTime(List<ApiTestData> testData);

        public List<ChartData_SpeedsByDateTime> SpeedsByDateTime(List<ApiTestData> testData);

        public List<ChartData_SpeedsByDateTime> AvailabilityByDateTime(List<ApiTestData> testData);

        public List<ChartData_ResultAndSpeedByTest> ResultAndSpeedByTest(List<ApiTestData> testData);

    }
}
