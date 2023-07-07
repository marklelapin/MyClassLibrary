using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApiMonitorClassLibrary.Interfaces;
using MyApiMonitorClassLibrary.Models;
using MyClassLibrary.ChartJs;
using MyClassLibrary.Colors;
using System.Drawing;

namespace MyApiMonitor.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IApiTestDataAccess _dataAccess;
        private readonly IChartDataProcessor _dataProcessor;

        public DashboardModel(IApiTestDataAccess dataAccess, IChartDataProcessor dataProcessor)
        {
            _dataAccess = dataAccess;
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

        private MyColors MyColors { get; set; } = new MyColors();

        public async Task OnGet([FromQuery] Guid collectionId, DateTime? startDate, DateTime? endDate, int? skip, int? limit)
        {
            skip = skip ?? 0;
            limit = limit ?? 1014;

            Guid availabilityCollectionId = Guid.Parse("c8ecdb94-36a9-4dbb-a5db-e6e036bbba0f");

            var speedAndTestData = await _dataAccess.GetAllBetweenDates(collectionId, startDate, endDate, (int)skip, (int)limit);

            var availabilityData = await _dataAccess.GetAllBetweenDates(availabilityCollectionId, DateTime.UtcNow.AddMinutes(-15), DateTime.UtcNow, (int)skip, (int)limit);

            ResultByDateTime = _dataProcessor.ResultByDateTime(speedAndTestData.records);

            SpeedByDateTime = _dataProcessor.SpeedsByDateTime(speedAndTestData.records);

            ResultAndSpeedByTest = _dataProcessor.ResultAndSpeedByTest(speedAndTestData.records);

            AvailabilityByDateTime = _dataProcessor.AvailabilityByDateTime(availabilityData.records);

            CollectionId = collectionId.ToString();



            ConfigureResultChart();

            ConfigureAvailabilityChart();

            ConfigureSpeedChart();

            ConfigureResultAndSpeedChart();

        }


        private void ConfigureResultChart()
        {
            var builder = new ChartBuilder("bar");

            builder.AddLabels(ResultByDateTime.Select(x => x.TestDateTime.ToString("MMM-dd HH:mm:ss")).ToArray())
                   .ConfigureYAxis(options =>
                   {
                       options.Stacked("true")
                       .AddTitle("No of Tests");
                   })
                   .ConfigureXAxis(options =>
                   {
                       options.Stacked("true");
                   })
                   .HideLegend()
                   .AddDataset("Successes", options =>
                    {
                        options.AddValues(ResultByDateTime.Select(x => x.SuccessfulTests).ToList())
                                .AddColors(new ColorSet(MyColors.Transparent(), MyColors.TrafficGreen(0.75), "sdfsd"))
                                .AddOrder(1)
                                .SpecifyAxes(null, "y");

                    })

                    .AddDataset("Failures", options =>
                    {
                        options.AddValues(ResultByDateTime.Select(x => x.FailedTests).ToList())
                                .AddColors(new ColorSet(MyColors.Transparent(), MyColors.TrafficOrangeRed(0.755)))
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
            .AddDefaultLineStyle(options =>
            {
                options.AddLineStyle(3, null)
                .AddColors(new ColorSet(MyColors.TrafficGreen(), MyColors.TrafficGreen(0.5)));
            })
            .ConfigureYAxis(options =>
            {
                options.AddTitle("Time to Complete Basic Get Request (ms)");
            })
            .ConfigureXAxis(options =>
            {
                options.AddTitle("Time")
                .ConvertTickToDateTime("hh:mm:ss");

            })
            .HideLegend()
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
            builder.AddLabels(SpeedByDateTime.Select(x => x.TestDateTime.ToString("MMM-dd HH:mm:ss")).ToArray())

                   .AddDefaultPointStyle(options =>
                   {
                       options.AddStyleAndRadius("circle", 0);
                   })
                   .ConfigureYAxis(options =>
                   {
                       options.AddTitle("Time To Complete (ms)");

                   })
                   .HideLegend()
                   .AddDataset("Min Time To Complete", options =>
                   {
                       options.AddValues(SpeedByDateTime.Select(x => x.MinSpeed).ToList())
                               .AddArea("+1", 1)
                               .AddColors(new ColorSet(MyColors.TrafficGreen(), MyColors.TrafficGreen(0.5)))
                               .AddOrder(1)
                               .SpecifyAxes(null, "y");
                   })
                   .AddDataset("Avg Time To Complete", options =>
                    {
                        options.AddValues(SpeedByDateTime.Select(x => x.AvgSpeed).ToList())
                                .AddLine(3)
                                .AddColors(new ColorSet(Color.Black, Color.Transparent))
                                .AddOrder(2)
                                .SpecifyAxes("x", "y");

                    })
                   .AddDataset("Max Time To Complete", options =>
                   {
                       options.AddValues(SpeedByDateTime.Select(x => x.MaxSpeed).ToList())
                               .AddArea("-1", 1)
                               .AddColors(new ColorSet(MyColors.TrafficGreen(), MyColors.TrafficGreen(0.5)))
                               .AddOrder(3)
                                .SpecifyAxes(null, "y");
                   });


            SpeedChartConfiguration = builder.BuildJson();
        }



        private void ConfigureResultAndSpeedChart()
        {
            Dictionary<string, List<CategoryCoordinate>> chartSeries = new Dictionary<string, List<CategoryCoordinate>>();

            chartSeries.Add("Always Successfull"
                            , ResultAndSpeedByTest.Where(x => x.AverageResult == 100 && x.LatestResult == true).Select(x => new CategoryCoordinate(x.Test, x.Controller, x.AverageTimeToComplete)).ToList());
            chartSeries.Add("Latest Successfull"
                            , ResultAndSpeedByTest.Where(x => x.AverageResult != 100 && x.LatestResult == true).Select(x => new CategoryCoordinate(x.Test, x.Controller, x.AverageTimeToComplete)).ToList());
            chartSeries.Add("Currently Failing"
                            , ResultAndSpeedByTest.Where(x => x.LatestResult == false).Select(x => new CategoryCoordinate(x.Test, x.Controller, x.AverageTimeToComplete)).ToList());


            var bubbleData = new CategoryBubbleChartData(chartSeries, 30);


            var builder = new ChartBuilder("bubble");

            builder.AddDataset("Always Successful", options =>
                {
                    options.AddCoordinates(bubbleData.Coordinates["Always Successfull"]!)
                    .AddColors(new ColorSet(MyColors.TrafficGreen(), MyColors.TrafficGreen(0.5)));
                })
                .AddDataset("Latest Successful", options =>
                {
                    options.AddCoordinates(bubbleData.Coordinates["Latest Successfull"])
                    .AddColors(new ColorSet(MyColors.TrafficOrange(), MyColors.TrafficOrange(0.5)));
                })
                .AddDataset("Currently Failing", options =>
                {
                    options.AddCoordinates(bubbleData.Coordinates["Currently Failing"])
                    .AddColors(new ColorSet(MyColors.TrafficOrangeRed(), MyColors.TrafficOrangeRed(0.5)));
                })
                .ConfigureYAxis(options =>
                {
                    options.AddTitle("Controller")
                    .AddAbsoluteScaleLimits(0, bubbleData.YLabels.Count + 1)
                    .AddTickCategoryLabels(bubbleData.YLabels);

                })
                .ConfigureXAxis(options =>
                {
                    options.AddTitle("Test")
                    .AddAbsoluteScaleLimits(0, bubbleData.XLabels.Count + 1)
                    .AddTickCategoryLabels(bubbleData.XLabels);
                })
                .HideLegend();



            ResultAndSpeedChartConfiguration = builder.BuildJson();

        }




    }
}
