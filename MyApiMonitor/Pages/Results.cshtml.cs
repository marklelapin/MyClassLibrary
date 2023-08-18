using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApiMonitorClassLibrary.Interfaces;
using MyApiMonitorClassLibrary.Models;

namespace MyApiMonitor.Pages.Results;

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

	public async Task OnGet(Guid testOrCollectionId, DateTime? dateFrom = null, DateTime? dateTo = null, int skip = 0, int limit = 1000)
	{
		dateFrom = dateFrom ?? DateTime.MinValue; //DateTime dtFrom = (dateFrom == null) ? DateTime.MinValue : DateTime.Parse(dateFrom);
		dateTo = dateTo ?? DateTime.MaxValue; //DateTime dtTo = (dateTo == null) ? DateTime.MaxValue : DateTime.Parse(dateTo);

		(TestResults, int totalRecords) = await _dataAccess.GetAllByCollectionId(testOrCollectionId, dateFrom, dateTo, skip, limit);

		if (totalRecords == 0)
		{ //if it can't find any using the guid passed in as collection Id then try using it as test Id.

			(TestResults, totalRecords) = await _dataAccess.GetAllByTestId(testOrCollectionId, dateFrom, dateTo, skip, limit);

		}

		TestResults = TestResults.OrderByDescending(x => x.TestDateTime).ToList();

		DateFrom = dateFrom.ToString() ?? string.Empty;
		DateTo = dateTo.ToString() ?? string.Empty;
		CollectionTitle = (TestResults.Count == 0) ? "" : TestResults.First().CollectionTitle;
		TestDateTime = (TestResults.Count == 0) ? "" : TestResults.First().TestDateTime.ToString();

	}
}


