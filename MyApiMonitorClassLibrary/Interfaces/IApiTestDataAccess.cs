
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
        public List<ApiTestData> GetAllByTestCollectionId(Guid testCollectionId);


        /// <summary>
        /// Returs all tests on the database with a given testId.
        /// </summary>
        public List<ApiTestData> GetAllByTestId(Guid testId);

        /// <summary>
        /// Returns all test run at a specific datetime.
        /// </summary>
        /// <param name="testDateTime"></param>
        public List<ApiTestData> GetAllByDateTime(Guid testCollectionId, DateTime testDateTime);



        /// <summary>
        /// Returns all tests run between specific dates.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public List<ApiTestData> GetAllBetweenDates(Guid testCollectionID, DateTime? startDate, DateTime? endDate);



    }
}
