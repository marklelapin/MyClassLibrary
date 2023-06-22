using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyApiMonitorService.Models
{
    /// <summary>
    /// Contains the necessary information to save and retrieve lists of APITets to database.
    /// </summary>
    public class ApiTestCollection
    {
        /// <summary>
        /// A Guid identifying the collection.
        /// </summary>
        /// <remarks>
        public Guid Id { get; set; }

        /// <summary>
        /// Current title for the Id
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// The list of tests in the order they are to be executed.
        /// </summary>
        public List<ApiTest> Tests { get; set; } = new List<ApiTest>();

        /// <summary>
        /// The DateTime the Tests in the collection were run.
        /// </summary>
        public DateTime TestDateTime { get; set; }


        public ApiTestCollection(Guid id, string title, DateTime? testDateTime = null, List<ApiTest>? tests = null)
        {
            Id = id;
            Title = title;
            TestDateTime = (testDateTime == null) ? DateTime.Now : (DateTime)testDateTime;
            if (tests != null) { Tests = tests; }
        }


        public List<ApiTestData> CreateApiTestData()
        {
            List<ApiTestData> output = new List<ApiTestData>();

            this.Tests.ForEach(test =>
            {
                var data = new ApiTestData(
                                                Id
                                                , Title
                                                , test.Id
                                                , test.Title
                                                , TestDateTime
                                                , test.TestResult?.WasSuccessful ?? throw new ArgumentNullException("WasSuccessul", "Can't convert to APITestData until tests have been ran.")
                                                , test.TestResult.FailureMessage
                                                , test.TestResult.ExpectedResult
                                                , test.TestResult.ActualResult
                                                );

                output.Add(data);
            });

            return output;
        }


    }
}
