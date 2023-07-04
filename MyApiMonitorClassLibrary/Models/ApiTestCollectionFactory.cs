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


        public List<ApiTestCollection> GenerateAvailabilityTestCollections()
        {
            List<ApiTestCollection> output = new List<ApiTestCollection>();

            //TODO - remove this 'hardcoded' section if developed further for future apis managed through database etc.
            var testCollection = new TestCollectionSetup_WhaddonShowApi().GenerateAvailabilityTestCollection();

            output.Add(testCollection);

            return output;
        }


        public (bool wasSuccessfull, Exception? exception) ExecuteTestCollections(List<ApiTestCollection> testCollections)
        {
            try
            {
                testCollections.ForEach((testCollection) =>
                                {
                                    _runner.RunTestAndSave(testCollection);
                                });
            }
            catch (Exception ex)
            {
                return (false, ex);
            }

            return (true, null);
        }
    }
}
