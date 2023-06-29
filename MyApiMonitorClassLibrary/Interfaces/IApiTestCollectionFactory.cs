using MyApiMonitorClassLibrary.Models;

namespace MyApiMonitorClassLibrary.Interfaces
{
    public interface IApiTestCollectionFactory
    {
        /// <summary>
        /// Generates the test collections to be executed.
        /// </summary>
        public List<ApiTestCollection> GenerateTestCollections();

        /// <summary>
        /// Runs and save the result of each test collection.
        /// </summary>
        public Task ExecuteTestCollections(List<ApiTestCollection> testCollections);
    }
}
