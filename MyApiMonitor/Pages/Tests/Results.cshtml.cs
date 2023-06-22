using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApiMonitorClassLibrary.Interfaces;
using MyApiMonitorClassLibrary.Models;

namespace MyApiMonitor.Pages.Tests;

public class ResultsModel : PageModel
{
    private readonly IApiTestingDataAccess _dataAccess;

    public ResultsModel(IApiTestingDataAccess dataAccess)
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

    public void OnGet([FromQuery] Guid collectionId, [FromQuery] DateTime? dateFrom = null, [FromQuery] DateTime? dateTo = null)
    {

        if (dateFrom == null)
        {
            TestResults = _dataAccess.GetAllByTestCollectionId(collectionId);
        }
        else if (dateTo == null)
        {
            TestResults = _dataAccess.GetAllByDateTime(collectionId, (DateTime)dateFrom);
        }
        else
        {
            TestResults = _dataAccess.GetAllBetweenDates(collectionId, (DateTime)dateFrom, (DateTime)dateTo);
        }

        if (TestResults.Count == 0) { TestResults = new List<ApiTestData>(); };

        DateFrom = dateFrom.ToString() ?? string.Empty;
        DateTo = dateTo.ToString() ?? string.Empty;
        CollectionTitle = (TestResults.Count == 0) ? "" : TestResults.First().CollectionTitle;
        TestDateTime = (TestResults.Count == 0) ? "" : TestResults.First().TestDateTime.ToString();

    }
}


