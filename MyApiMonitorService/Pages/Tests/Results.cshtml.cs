using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using MongoDB.Driver.Linq;
using MyApiMonitorService.Interfaces;
using MyApiMonitorService.Models;

namespace MyApiMonitorService.Pages.Tests
{
    public class ResultsModel : PageModel
    {
        private readonly IApiTestingDataAccess _dataAccess;

        public ResultsModel(IApiTestingDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        [BindProperty]
        public List<ApiTestData> TestResults { get; set; }

        [BindProperty]
        public string DateFrom { get; set; }

        [BindProperty]
        public string DateTo { get; set; }


        [BindProperty]
        public string CollectionTitle { get; set; }

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
            {

            }

            DateFrom = dateFrom.ToString() ?? string.Empty;
            DateTo = dateTo.ToString() ?? string.Empty;


        }


    }
}