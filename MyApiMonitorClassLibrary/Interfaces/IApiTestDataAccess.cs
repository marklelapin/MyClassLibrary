
using MyApiMonitorClassLibrary.Models;

namespace MyApiMonitorClassLibrary.Interfaces
{
    /// <summary>
    /// Provides the method for saving API Tests to a database.
    /// </summary>
    public interface IApiTestDataAccess
    {
        /// <summary>
        /// Saves a testCollection to database.
        /// </summary>
        public void Save(ApiTestCollection testCollection);

        /// <summary>
        /// Returns all tests on the database with a given testCollectionId.
        /// </summary>
        public (List<ApiTestData> records, int total) GetAllByTestCollectionId(Guid testCollectionId, int skip = 0, int limit = 10000);


        /// <summary>
        /// Returs all tests on the database with a given testId.
        /// </summary>
        public (List<ApiTestData> records, int total) GetAllByTestId(Guid testId, int skip = 0, int limit = 10000);

        /// <summary>
        /// Returns all test run at a specific datetime.
        /// </summary>
        public (List<ApiTestData> records, int total) GetAllByDateTime(Guid testCollectionId, DateTime testDateTime, int skip = 0, int limit = 10000);



        /// <summary>
        /// Returns all tests run between specific dates.
        /// </summary>
        public (List<ApiTestData> records, int total) GetAllBetweenDates(Guid testCollectionID, DateTime? startDate, DateTime? endDate, int skip = 0, int limit = 10000);



    }
}
