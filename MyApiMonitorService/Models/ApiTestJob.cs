using MyApiMonitorService.Interfaces;
using Quartz;

namespace MyApiMonitorService.Models
{
    public class ApiTestJob : IJob
    {
        private readonly IApiTestRunner _testRunner;
        private readonly IApiTestCollectionFactory _testCollectionFactory;

        public ApiTestJob(IApiTestRunner testRunner, IApiTestCollectionFactory testCollectionFactory)
        {
            _testRunner = testRunner;
            _testCollectionFactory = testCollectionFactory;
        }


        public async Task Execute(IJobExecutionContext context)
        {

            List<ApiTestCollection> testCollections = _testCollectionFactory.GenerateTestCollections();

            await Task.Run(() =>
            {

                testCollections.ForEach(async (testCollection) =>
                    {
                        await _testRunner.RunTestAndSave(testCollection);
                    });
            });
        }

    }
}
