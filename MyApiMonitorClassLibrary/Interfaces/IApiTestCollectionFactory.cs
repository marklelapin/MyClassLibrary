using MyApiMonitorClassLibrary.Models;

namespace MyApiMonitorClassLibrary.Interfaces
{
    public interface IApiTestCollectionFactory
    {
        /// <summary>
        /// Generates larger test collections to be run periodically.
        /// </summary>
        public List<ApiTestCollection> GenerateTestCollections();

        /// <summary>
        /// Generates small test collections to be run every 10 seconds.
        /// </summary>
        public List<ApiTestCollection> GenerateAvailabilityTestCollections();


        /// <summary>
        /// Runs and save the result of each test collection.
        /// </summary>
        public (bool wasSuccessfull, Exception? exception) ExecuteTestCollections(List<ApiTestCollection> testCollections);
    }
}
