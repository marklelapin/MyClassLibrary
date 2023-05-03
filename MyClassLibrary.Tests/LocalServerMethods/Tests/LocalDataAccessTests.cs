using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using MyClassLibrary.Tests.LocalServerMethods.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class LocalDataAccessTests
    {
        private static IServiceConfiguration _serviceConfiguration = new TestServiceConfiguration();

        private static ILocalDataAccessTests<TestUpdate> _localDataAccessTests = new LocalDataAccessTestsService<TestUpdate>(_serviceConfiguration);

        //private static ITestContent<TestUpdate> _testContent = new TestContentService<TestUpdate>();

        public static object[][] SaveTestData = _localDataAccessTests.SaveTestData();
        [Theory, MemberData(nameof(SaveTestData))]
        public void TestUpdateSaveTest(List<TestUpdate> testUpdates)
        {
            _localDataAccessTests.SaveTest(testUpdates);
        }
            
            

    }
}
