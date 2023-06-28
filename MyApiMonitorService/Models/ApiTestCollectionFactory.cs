using MyApiMonitorService.Interfaces;

namespace MyApiMonitorService.Models
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
