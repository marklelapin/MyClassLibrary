
using MyApiMonitorService.Models;

namespace MyApiMonitorService.Interfaces
{
    /// <summary>
    /// Provides the methods for executing,timing and saving the APITest class or lists of APITest 
    /// </summary>
    public interface IApiTestRunner
    {


        /// <summary>
        /// Runs a list of API Tests in the list order and populates the TestResult property of each test.
        /// </summary>
        /// <returns>
        /// True if all tests were successful.
        /// </returns>
        public Task RunTest(List<ApiTest> tests);


        /// <summary>
        /// Saves a Test Collection to database
        /// </summary>
        public Task Save(ApiTestCollection testCollection);


        /// <summary>
        /// Runs and saves a test Collection to database
        /// </summary>
        /// <param name="tests"></param>
        /// <returns></returns>
        public Task RunTestAndSave(ApiTestCollection testCollection);


    }
}
