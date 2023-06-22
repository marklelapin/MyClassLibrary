using MyApiMonitorClassLibrary.Interfaces;

namespace MyApiMonitorClassLibrary.Models
{
    public class ApiTestCollectionFactory : IApiTestCollectionFactory
    {

        public ApiTestCollectionFactory() { }

        public List<ApiTestCollection> GenerateTestCollections()
        {
            List<ApiTestCollection> output = new List<ApiTestCollection>();

            output.Add(new TestCollectionSetup_WhaddonShowApi().TestCollection);

            return output;
        }
    }
}
