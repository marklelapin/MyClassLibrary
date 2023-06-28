using MyApiMonitorClassLibrary.Interfaces;

namespace MyApiMonitorClassLibrary.Models
{
    public class ApiTestCollectionFactory : IApiTestCollectionFactory
    {

        public ApiTestCollectionFactory() { }

        public List<ApiTestCollection> GenerateTestCollections()
        {
            List<ApiTestCollection> output = new List<ApiTestCollection>();

            //TODO - remove this 'hardcoded' section if developed further for future apis managed through database etc.
            var testCollection = new TestCollectionSetup_WhaddonShowApi().GenerateTestCollection();

            output.Add(testCollection);

            return output;
        }
    }
}
