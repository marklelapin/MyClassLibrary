using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApiMonitorClassLibrary.Interfaces;
using MyApiMonitorClassLibrary.Models;

namespace MyApiMonitor.Pages.Tests;

public class ResultsModel : PageModel
{
    private readonly IApiTestDataAccess _dataAccess;

    public ResultsModel(IApiTestDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }


    [BindProperty]
    public List<ApiTestData> TestResults { get; set; } = new List<ApiTestData>();

    [BindProperty]
    public string DateFrom { get; set; }

    [BindProperty]
    public string DateTo { get; set; }

    [BindProperty]
    public string CollectionTitle { get; set; }

    [BindProperty]
    public string TestDateTime { get; set; }

    public void OnGet([FromQuery] Guid collectionId, DateTime? dateFrom = null, DateTime? dateTo = null, int skip = 0, int limit = 1000)
    {

        if (dateFrom == null)
        {
            (TestResults, int totalRecords) = _dataAccess.GetAllByTestCollectionId(collectionId, skip, limit);
        }
        else if (dateTo == null)
        {
            (TestResults, int totalRecords) = _dataAccess.GetAllByDateTime(collectionId, (DateTime)dateFrom);
        }
        else
        {
            (TestResults, int totalRecords) = _dataAccess.GetAllBetweenDates(collectionId, (DateTime)dateFrom, (DateTime)dateTo);
        }

        if (TestResults.Count == 0) { TestResults = new List<ApiTestData>(); };

        TestResults = TestResults.OrderByDescending(x => x.TestDateTime).ToList();

        DateFrom = dateFrom.ToString() ?? string.Empty;
        DateTo = dateTo.ToString() ?? string.Empty;
        CollectionTitle = (TestResults.Count == 0) ? "" : TestResults.First().CollectionTitle;
        TestDateTime = (TestResults.Count == 0) ? "" : TestResults.First().TestDateTime.ToString();

    }
}


