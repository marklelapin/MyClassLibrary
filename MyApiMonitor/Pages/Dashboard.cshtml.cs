using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApiMonitorClassLibrary.Interfaces;
using MyApiMonitorClassLibrary.Models;
using MyClassLibrary.ChartJs;
using MyClassLibrary.Colors;
using MyClassLibrary.Pagination;
using System.Drawing;
using System.Text.Json;

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

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public string PaginationHtml { get; set; }

        private List<ChartData_ResultByDateTime> ResultByDateTime { get; set; }

        private List<ChartData_SpeedsByDateTime> SpeedByDateTime { get; set; }

        private List<ChartData_ResultAndSpeedByTest> ResultAndSpeedByTest { get; set; }

        private List<ChartData_SpeedsByDateTime> AvailabilityByDateTime { get; set; }

        private MyColors MyColors { get; set; } = new MyColors();

        private Guid AvailabilityCollectionId = Guid.Parse("c8ecdb94-36a9-4dbb-a5db-e6e036bbba0f"); //TODO If developing this further this should be stored in database along with CollectionID for a particular API



        public async Task OnGet(Guid collectionId, int pagination = 1)
        {
            CollectionId = collectionId.ToString();
            DateTime dateFrom = DateTime.UtcNow.AddMonths(-3);
            DateTime dateTo = DateTime.UtcNow;
            int limit = 1014;
            int skip = (pagination - 1) * limit;


            var speedAndTestData = await _dataAccess.GetAllByCollectionId(collectionId, dateFrom, dateTo, (int)skip, (int)limit);

            var availabilityData = await _dataAccess.GetAllByCollectionId(AvailabilityCollectionId, DateTime.UtcNow.AddMinutes(-15), DateTime.UtcNow, 0, (int)limit);

            int totalPages = (int)Math.Ceiling((double)speedAndTestData.total / (double)limit);

            DateFrom = speedAndTestData.records.Select(x => x.TestDateTime).Min();
            DateTo = speedAndTestData.records.Select(x => x.TestDateTime).Max();

            var builder = new PaginationBuilder($"/Dashboard/{collectionId.ToString()}/<page>");
            builder.AddFirst("Earliest");
            builder.AddPrevious("Previous");
            builder.SetMiddleTotal(2);
            builder.AddNext("Next");
            builder.AddLast("Latest");

            PaginationHtml = builder.BuildHtml(pagination, totalPages);

            ResultByDateTime = _dataProcessor.ResultByDateTime(speedAndTestData.records);

            SpeedByDateTime = _dataProcessor.SpeedsByDateTime(speedAndTestData.records);

            ResultAndSpeedByTest = _dataProcessor.ResultAndSpeedByTest(speedAndTestData.records);

            AvailabilityByDateTime = _dataProcessor.AvailabilityByDateTime(availabilityData.records);




            ConfigureResultChart();

            ConfigureAvailabilityChart();

            ConfigureSpeedChart();

            ConfigureResultAndSpeedChart();

        }


        public async Task<IActionResult> OnGetNewAvailabilityDatapoints()
        {
            var newAvailabilityData = await _dataAccess.GetAllByCollectionId(AvailabilityCollectionId, DateTime.UtcNow.AddSeconds(-12), DateTime.UtcNow);

            List<Coordinate> newCoordinates = newAvailabilityData.records.Select(x => new Coordinate(x.TestDateTime, (double)x.TimeToComplete!)).ToList();

            return Content(JsonSerializer.Serialize(newCoordinates));
        }


        private void ConfigureResultChart()
        {
            var builder = new ChartBuilder("bar");

            builder.AddLabels(ResultByDateTime.Select(x => x.TestDateTime.ToString("o")).ToArray())
                   .ConfigureYAxis(options =>
                   {
                       options.Stacked("true")
                       .AddTitle("No of Tests");
                   })
                   .ConfigureXAxis(options =>
                   {
                       options.Stacked("true")
                       .ConvertLabelToDateTime("MMM-DD HH:mm");
                   })
                   .HideLegend()
                   .AddClickEventHandler("resultChartClickHandler")
                   .AddDataset("Successes", options =>
                    {
                        options.AddValues(ResultByDateTime.Select(x => x.SuccessfulTests).ToList())
                                .AddColors(new ColorSet(MyColors.TrafficGreen(), MyColors.TrafficGreen(0.6)))
                                .AddOrder(1)
                                .SpecifyAxes(null, "y")
                                .AddHoverFormat(new ColorSet(MyColors.TrafficGreen(), MyColors.TrafficGreen()));

                    })

                    .AddDataset("Failures", options =>
                    {
                        options.AddValues(ResultByDateTime.Select(x => x.FailedTests).ToList())
                                .AddColors(new ColorSet(MyColors.TrafficOrangeRed(), MyColors.TrafficOrangeRed(0.6)))
                                .AddOrder(2)
                                .SpecifyAxes(null, "y")
                                .AddHoverFormat(new ColorSet(MyColors.TrafficOrangeRed(), MyColors.TrafficOrangeRed()));
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
                //.AddAbsoluteScaleLimits(null, 500);
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
            builder.AddLabels(SpeedByDateTime.Select(x => x.TestDateTime.ToString("o")).ToArray())

                   .AddDefaultPointStyle(options =>
                   {
                       options.AddStyleAndRadius("circle", 0)
                       .AddColors(new ColorSet(MyColors.TrafficGreen(), MyColors.TrafficGreen()))
                       .AddHover(6, 6)
                       .AddHit(15);
                   })
                   .AddClickEventHandler("speedChartClickHandler")
                   .ConfigureXAxis(options =>
                   {
                       options.ConvertLabelToDateTime("MMM-DD HH:mm");
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
                            , ResultAndSpeedByTest.Where(x => x.AverageResult == 100 && x.LatestResult == true).Select(x => new CategoryCoordinate(x.Controller, x.Test, x.AverageTimeToComplete, $"{x.Controller}-{x.Test}", x.TestId.ToString())).ToList());
            chartSeries.Add("Latest Successfull"
                            , ResultAndSpeedByTest.Where(x => x.AverageResult != 100 && x.LatestResult == true).Select(x => new CategoryCoordinate(x.Controller, x.Test, x.AverageTimeToComplete, $"{x.Controller}-{x.Test}", x.TestId.ToString())).ToList());
            chartSeries.Add("Currently Failing"
                            , ResultAndSpeedByTest.Where(x => x.LatestResult == false).Select(x => new CategoryCoordinate(x.Controller, x.Test, x.AverageTimeToComplete, $"{x.Controller}-{x.Test}", x.TestId.ToString())).ToList());


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
                    options.AddTitle("Test")
                    .AddAbsoluteScaleLimits(0, bubbleData.YLabels.Count + 1)
                    .AddTickCategoryLabels(bubbleData.YLabels);

                })
                .ConfigureXAxis(options =>
                {
                    options.AddTitle("Controller")
                    .AddAbsoluteScaleLimits(0, bubbleData.XLabels.Count + 1)
                    .AddTickCategoryLabels(bubbleData.XLabels)
                    .SetAutoSkip(false)
                    .SetTickRotation(90);
                })
                .HideLegend()
                .AddClickEventHandler("resultAndSpeedChartClickHandler");



            ResultAndSpeedChartConfiguration = builder.BuildJson();

        }




    }
}
