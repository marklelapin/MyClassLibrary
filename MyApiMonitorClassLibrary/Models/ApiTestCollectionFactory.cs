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


        public (bool wasSuccessfull, Exception? exception, int testsPassed, int testsRun) ExecuteTestCollections(List<ApiTestCollection> testCollections)
        {
            int totalTestsPassed = 0;
            int totalTestsRun = 0;

            try
            {
                int collectionTestsPassed = 0;
                int collectionTestsRun = 0;

                testCollections.ForEach((testCollection) =>
                                {
                                    (collectionTestsPassed, collectionTestsRun) = _runner.RunTestAndSave(testCollection);
                                    totalTestsPassed += collectionTestsPassed;
                                    totalTestsRun += collectionTestsRun;
                                });
            }
            catch (Exception ex)
            {
                return (false, ex, totalTestsPassed, totalTestsRun);
            }

            return (true, null, totalTestsPassed, totalTestsRun);
        }
    }
}
