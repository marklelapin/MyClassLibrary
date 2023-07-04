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
        private readonly IChartDataProcessor _dataProcessor;

        public DashboardModel(IChartDataProcessor dataProcessor)
        {
            _dataProcessor = dataProcessor;
        }

        public string CollectionId { get; set; }
        public string TestChartConfiguration { get; set; }
        public string SpeedChartConfiguration { get; set; }

        public List<ChartData_TestDateTimeSuccessFailure> TestDateTimeSuccessFailures { get; set; }

        public List<ChartData_TestDateTimeSpeed> TestDateTimeSpeeds { get; set; }


        public void OnGet([FromQuery] Guid collectionId, DateTime? startDate, DateTime? endDate)
        {
            TestDateTimeSuccessFailures = _dataProcessor.SuccessOrFailureByDateTime(collectionId, startDate, endDate);

            TestDateTimeSpeeds = _dataProcessor.SpeedsByDateTime(collectionId, startDate, endDate);

            CollectionId = collectionId.ToString();

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

            TestChartConfiguration = builder.BuildJson();
        }


        private void ConfigureSpeedChart()
        {
            var builder = new ChartBuilder("line");
            builder.AddLabels(TestDateTimeSpeeds.Select(x => x.TestDateTime.ToString()).ToArray())
                   //.AddDefaultLineStyle(3, Color.Orange, Color.Transparent)
                   .AddDefaultPointStyle(options =>
                   {
                       options.AddStyleAndRadius("circle", 0);
                   })
                   .ConfigureYAxis(options =>
                   {
                       options.AddTitle("Time To Complete (ms)")
                                .AddAbsoluteScaleLimits(0, 500);
                   })
                   .AddDataset("Min Time To Complete", options =>
                   {
                       options.AddValues(TestDateTimeSpeeds.Select(x => x.MinSpeed).ToList())
                               .AddArea("+1", 0, Color.AliceBlue, Color.AliceBlue)
                               .AddOrder(1)
                               .SpecifyYAxisID("yAxis");
                   })
                   .AddDataset("Avg Time To Complete", options =>
                    {
                        options.AddValues(TestDateTimeSpeeds.Select(x => x.AvgSpeed).ToList())
                                .AddLine(3, Color.Black, Color.Transparent)
                                .AddOrder(2)
                                .SpecifyYAxisID("yAxis");
                    })
                   .AddDataset("Max Time To Complete", options =>
                   {
                       options.AddValues(TestDateTimeSpeeds.Select(x => x.MaxSpeed).ToList())
                               .AddArea("-1", 0, Color.OrangeRed, Color.Transparent)
                               .AddOrder(3)
                               .SpecifyYAxisID("yAxis");
                   });



            SpeedChartConfiguration = builder.BuildJson();
        }

    }
}
