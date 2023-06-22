using MyApiMonitorClassLibrary.Models;

namespace MyApiMonitorClassLibrary.Interfaces
{
    public interface IApiTestCollectionFactory
    {
        /// <summary>
        /// Generates the test collections to be used by Api Test Monitor
        /// </summary>
        public List<ApiTestCollection> GenerateTestCollections();
    }
}
