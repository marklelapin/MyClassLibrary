using MyApiMonitorService.Interfaces;

namespace MyApiMonitorService.Models
{
    public class ApiTestCollectionFactory : IApiTestCollectionFactory
    {

        public ApiTestCollectionFactory() { }

        public List<ApiTestCollection> GenerateTestCollections()
        {
            List<ApiTestCollection> output = new List<ApiTestCollection>();

            var testCollection = new TestCollectionSetup_WhaddonShowApi().GetTestCollection();

            output.Add(testCollection);

            return output;
        }
    }
}
