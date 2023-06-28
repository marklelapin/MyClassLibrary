using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApiMonitorClassLibrary.Interfaces;
using MyApiMonitorClassLibrary.Models;
using MyClassLibrary.ChartJs;
using System.Drawing;

namespace MyApiMonitor.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IApiTestDataProcessor _dataProcessor;

        public DashboardModel(IApiTestDataProcessor dataProcessor)
        {
            _dataProcessor = dataProcessor;
        }

        public string ResultChartConfiguration { get; set; }
        public string SpeedChartConfiguration { get; set; }

        public List<TestDateTimeSuccessFailure> TestDateTimeSuccessFailures { get; set; }

        public List<TestDateTimeSpeed> TestDateTimeSpeeds { get; set; }


        public void OnGet([FromQuery] Guid collectionId, DateTime? startDate, DateTime? endDate)
        {
            TestDateTimeSuccessFailures = _dataProcessor.SuccessOrFailureByDateTime(collectionId, startDate, endDate);

            TestDateTimeSpeeds = _dataProcessor.SpeedsByDateTime(collectionId, startDate, endDate);

            ConfigureResultChart();

            ConfigureSpeedChart();

        }


        private void ConfigureResultChart()
        {
            var builder = new ChartBuilder("bar");

            builder.AddLabels(TestDateTimeSuccessFailures.Select(x => x.TestDateTime.ToString()).ToArray())
                   .AddDataset("Successes", options =>
                    {
                        options.AddValues(TestDateTimeSuccessFailures.Select(x => x.SuccessfulTests).ToList())
                                .AddFormating(Color.Transparent, Color.OliveDrab)
                                .AddOrder(1);
                    })

                    .AddDataset("Failures", options =>
                    {
                        options.AddValues(TestDateTimeSuccessFailures.Select(x => x.FailedTests).ToList())
                                .AddFormating(Color.Transparent, Color.OrangeRed)
                                .AddOrder(2);
                    });

            ResultChartConfiguration = builder.BuildJson();
        }


        private void ConfigureSpeedChart()
        {
            var builder = new ChartBuilder("bar");
            builder.AddLabels(TestDateTimeSpeeds.Select(x => x.TestDateTime.ToString()).ToArray())
                   .AddDataset("Average Speed", options =>
                    {
                        options.AddValues(TestDateTimeSpeeds.Select(x => x.AvgSpeed).ToList())
                                .AddFormating(Color.Transparent, Color.Black)
                                .AddOrder(1);
                    });

            SpeedChartConfiguration = builder.BuildJson();
        }

    }
}
