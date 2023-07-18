using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApiMonitorClassLibrary.Interfaces;
using MyApiMonitorClassLibrary.Models;
using MyClassLibrary.ChartJs;
using MyClassLibrary.Colors;
using MyClassLibrary.Extensions;
using MyClassLibrary.Pagination;
using MyExtensions;
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
        public int Reliability { get; set; }
        public int AverageSpeed { get; set; }


        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public string PaginationHtml { get; set; }

        [BindProperty(SupportsGet = true)]
        public int LatestTestDateTime { get; set; }

        private List<ChartData_ResultByDateTime> ResultByDateTime { get; set; }

        private List<ChartData_SpeedsByDateTime> SpeedByDateTime { get; set; }

        private List<ChartData_ResultAndSpeedByTest> ResultAndSpeedByTest { get; set; }

        private List<ChartData_SpeedsByDateTime> AvailabilityByDateTime { get; set; }

        //Colors...
        string chartWhite = MyColors.OffWhite();



        private Guid AvailabilityCollectionId = Guid.Parse("c8ecdb94-36a9-4dbb-a5db-e6e036bbba0f"); //TODO If developing this further this should be stored in database along with CollectionID for a particular API



        public async Task OnGet(Guid collectionId, int pagination = 1)
        {
            CollectionId = collectionId.ToString();
            DateTime dateFrom = DateTime.UtcNow.AddMonths(-3);
            DateTime dateTo = DateTime.UtcNow;
            int limit = 1014;
            int skip = (pagination - 1) * limit;


            var speedAndTestData = await _dataAccess.GetAllByCollectionId(collectionId, dateFrom, dateTo, (int)skip, (int)limit);

            var availabilityData = await _dataAccess.GetAllByCollectionId(AvailabilityCollectionId, DateTime.UtcNow.AddMinutes(-2), DateTime.UtcNow, 0, (int)limit);

            int totalPages = (int)Math.Ceiling((double)speedAndTestData.total / (double)limit);

            DateFrom = speedAndTestData.records.Select(x => x.TestDateTime).Min();
            DateTo = speedAndTestData.records.Select(x => x.TestDateTime).Max();

            var builder = new PaginationBuilder($"/Dashboard?collectionId={collectionId.ToString()}&page=<page>");
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

            var totalSuccesses = ResultByDateTime.Sum(x => x.SuccessfulTests);
            var totalTests = ResultByDateTime.Sum(x => x.SuccessfulTests + x.FailedTests);
            Reliability = (int)(100 * ((double)totalSuccesses / (double)totalTests));

            AverageSpeed = (int)(SpeedByDateTime.Average(x => x.AvgSpeed) ?? 0);


            ConfigureResultChart();

            ConfigureAvailabilityChart();

            ConfigureSpeedChart();

            ConfigureResultAndSpeedChart();

        }


        public async Task<IActionResult> OnGetNewAvailabilityDatapoints(double timestamp)
        {
            DateTime timeFrom = timestamp.JavascriptTicksToDate();

            var newAvailabilityData = await _dataAccess.GetAllByCollectionId(AvailabilityCollectionId, timeFrom, DateTime.UtcNow);

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
                       .AddTitle("No of Tests", chartWhite)
                       .AddAbsoluteScaleLimits(0, 50);
                   })
                   .ConfigureXAxis(options =>
                   {
                       options.AddTitle("DateTime", chartWhite)
                       .Stacked("true")
                       .ConvertLabelToDateTime("MMM-DD")
                       .SetTickRotation(0);


                   })
                   .HideLegend()
                   .MaintainAspectRatio(false)
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
                options.AddTitle("Time to Complete (ms)", chartWhite)
                .AddAbsoluteScaleLimits(100, 500);
            })
            .ConfigureXAxis(options =>
            {
                options.AddTitle("Time", chartWhite)
                .ConvertTickToDateTime("HH:mm:ss")
                .AddAbsoluteScaleLimits(AvailabilityByDateTime.Select(x => x.TestDateTime).Min().ToJavascriptTimeStamp()
                                        , AvailabilityByDateTime.Select(x => x.TestDateTime).Max().ToJavascriptTimeStamp());

            })
            .HideLegend()
            .MaintainAspectRatio(false)
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
                       options.AddTitle("DateTime", chartWhite)
                       .ConvertLabelToDateTime("MMM-DD")
                       .SetTickRotation(0)
                       .AutoSkipTicks(true, 5);
                   })
                   .ConfigureYAxis(options =>
                   {
                       options.AddTitle("Time To Complete (ms)", MyColors.OffWhite());
                   })
                   .HideLegend()
                   .MaintainAspectRatio(false)
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
                    .AddColors(new ColorSet(MyColors.TrafficRed(), MyColors.TrafficRed(0.5)));
                })
                .ConfigureYAxis(options =>
                {
                    options.AddTitle("Test", MyColors.OffWhite())
                    .AddAbsoluteScaleLimits(0, bubbleData.YLabels.Count + 1)
                    .OverrideTickValues(bubbleData.YLabels.Values.Select(x => (double)x).ToList())
                    .AddTickCategoryLabels(bubbleData.YLabels)
                    .TickColor(Color.Gainsboro);
                })
                .ConfigureXAxis(options =>
                {
                    options.AddTitle("Controller", MyColors.OffWhite())
                    .AddAbsoluteScaleLimits(0, bubbleData.XLabels.Count + 1)
                    .AddTickCategoryLabels(bubbleData.XLabels)
                    .AutoSkipTicks(false)
                    .SetTickRotation(90)
                    .TickColor(Color.Gainsboro);
                })
                //.HideLegend()
                .MaintainAspectRatio(false)
                .AddClickEventHandler("resultAndSpeedChartClickHandler");



            ResultAndSpeedChartConfiguration = builder.BuildJson();

        }




    }
}
