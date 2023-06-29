using MyApiMonitorClassLibrary.Interfaces;

namespace MyApiMonitorClassLibrary.Models
{
    public class ApiTestCollectionFactory : IApiTestCollectionFactory
    {
        private IApiTestRunner _runner;

        public ApiTestCollectionFactory(IApiTestRunner testRunner)
        {
            _runner = testRunner;
        }



        public List<ApiTestCollection> GenerateTestCollections()
        {
            List<ApiTestCollection> output = new List<ApiTestCollection>();

            //TODO - remove this 'hardcoded' section if developed further for future apis managed through database etc.
            var testCollection = new TestCollectionSetup_WhaddonShowApi().GenerateTestCollection();

            output.Add(testCollection);

            return output;
        }



        public async Task ExecuteTestCollections(List<ApiTestCollection> testCollections)
        {

            await Task.Run(() =>
            {

                testCollections.ForEach(async (testCollection) =>
                {
                    await _runner.RunTestAndSave(testCollection);
                });
            });

        }
    }
}
