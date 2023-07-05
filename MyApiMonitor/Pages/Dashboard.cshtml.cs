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
        public string ResultChartConfiguration { get; set; }
        public string SpeedChartConfiguration { get; set; }
        public string AvailabilityChartConfiguration { get; set; }
        public string ResultAndSpeedChartConfiguration { get; set; }

        private List<ChartData_ResultByDateTime> ResultByDateTime { get; set; }

        private List<ChartData_SpeedsByDateTime> SpeedByDateTime { get; set; }

        private List<ChartData_ResultAndSpeedByTest> ResultAndSpeedByTest { get; set; }

        private List<ChartData_SpeedsByDateTime> AvailabilityByDateTime { get; set; }

        public void OnGet([FromQuery] Guid collectionId, DateTime? startDate, DateTime? endDate, int? skip, int? limit)
        {
            skip = skip ?? 0;
            limit = limit ?? 1000;

            Guid availabilityCollectionId = Guid.Parse("c8ecdb94-36a9-4dbb-a5db-e6e036bbba0f");

            ResultByDateTime = _dataProcessor.ResultByDateTime(collectionId, startDate, endDate, (int)skip, (int)limit);

            SpeedByDateTime = _dataProcessor.SpeedsByDateTime(collectionId, startDate, endDate, (int)skip, (int)limit);

            ResultAndSpeedByTest = _dataProcessor.ResultAndSpeedByTest(collectionId, startDate, endDate, (int)skip, (int)limit);

            AvailabilityByDateTime = _dataProcessor.AvailabilityByDateTime(availabilityCollectionId, DateTime.UtcNow.AddHours(-4), DateTime.UtcNow, 0, 60);

            CollectionId = collectionId.ToString();

            ConfigureResultChart();

            ConfigureAvailabilityChart();

            ConfigureSpeedChart();

            ConfigureResultAndSpeedChart();

        }


        private void ConfigureResultChart()
        {
            var builder = new ChartBuilder("bar");

            builder.AddLabels(ResultByDateTime.Select(x => x.TestDateTime.ToString()).ToArray())
                   .ConfigureYAxis(options =>
                   {
                       options.Stacked("true");
                   })
                   .ConfigureXAxis(options =>
                   {
                       options.Stacked("true");
                   })
                   .AddDataset("Successes", options =>
                    {
                        options.AddValues(ResultByDateTime.Select(x => x.SuccessfulTests).ToList())
                                .AddFormating(Color.Transparent, Color.OliveDrab)
                                .AddOrder(1)
                                .SpecifyAxes(null, "y");

                    })

                    .AddDataset("Failures", options =>
                    {
                        options.AddValues(ResultByDateTime.Select(x => x.FailedTests).ToList())
                                .AddFormating(Color.Transparent, Color.OrangeRed)
                                .AddOrder(2)
                                .SpecifyAxes(null, "y");
                    });

            ResultChartConfiguration = builder.BuildJson();
        }


        private void ConfigureAvailabilityChart()
        {
            var builder = new ChartBuilder("scatter");
            builder.AddDefaultPointStyle(options =>
            {
                options.AddStyleAndRadius("circle", 0);
            })
            .AddDefaultLineStyle(3, Color.Orange, Color.Orange)
            .ConfigureYAxis(options =>
            {
                options.AddTitle("Time to Complete Basic Get Request (ms)");
            })
            .ConfigureXAxis(options =>
            {
                options.AddTitle("Time");
            })
            //    options.AddType("time");
            //})
            .AddDataset("Availability", options =>
            {
                options.AddCoordinates(AvailabilityByDateTime.Select(x => new Coordinate(x.TestDateTime, (double)x.AvgSpeed!)).ToList())
                .ShowLine()
                ;

            });

            AvailabilityChartConfiguration = builder.BuildJson();
        }

        private void ConfigureSpeedChart()
        {
            var builder = new ChartBuilder("line");
            builder.AddLabels(SpeedByDateTime.Select(x => x.TestDateTime.ToString()).ToArray())

                   .AddDefaultPointStyle(options =>
                   {
                       options.AddStyleAndRadius("circle", 0);
                   })
                   .ConfigureYAxis(options =>
                   {
                       options.AddTitle("Time To Complete (ms)")
                                .AddAbsoluteScaleLimits(0, null);

                   })
                   .AddDataset("Min Time To Complete", options =>
                   {
                       options.AddValues(SpeedByDateTime.Select(x => x.MinSpeed).ToList())
                               .AddArea("+1", 1, Color.AliceBlue, Color.Orange)
                               .AddOrder(1)
                               .SpecifyAxes(null, "yAxis");
                   })
                   .AddDataset("Avg Time To Complete", options =>
                    {
                        options.AddValues(SpeedByDateTime.Select(x => x.AvgSpeed).ToList())
                                .AddLine(3, Color.Black, Color.Transparent)
                                .AddOrder(2)
                                .SpecifyAxes(null, "yAxis");
                    })
                   .AddDataset("Max Time To Complete", options =>
                   {
                       options.AddValues(SpeedByDateTime.Select(x => x.MaxSpeed).ToList())
                               .AddArea("-1", 1, Color.OrangeRed, Color.Orange)
                               .AddOrder(3)
                               .SpecifyAxes(null, "yAxis");
                   });



            SpeedChartConfiguration = builder.BuildJson();
        }



        private void ConfigureResultAndSpeedChart()
        {

            var categoryCoordinates = ResultAndSpeedByTest.Select(x => new CategoryCoordinate(x.Test, x.Controller, x.AverageTimeToComplete)).ToList();

            var bubbleData = new CategoryBubbleChartData(categoryCoordinates, 30);


            var builder = new ChartBuilder("bubble");

            builder.AddDataset("ResultAndSpeed", options =>
                {
                    options.AddCoordinates(bubbleData.coordinates);
                })
                .ConfigureYAxis(options =>
                {
                    options.AddAbsoluteScaleLimits(0, 4);
                });

            ResultAndSpeedChartConfiguration = builder.BuildJson();

        }
    }
}
