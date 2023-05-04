using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using MyClassLibrary.Tests.LocalServerMethods.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class TestUpdateLocalDataAccessTests
    {
        private static IServiceConfiguration _serviceConfiguration = new TestServiceConfiguration();

        private static ILocalDataAccessTests<TestUpdate> _localDataAccessTests = new LocalDataAccessTestsService<TestUpdate>(_serviceConfiguration);

        //private static ITestContent<TestUpdate> _testContent = new TestContentService<TestUpdate>();

        public static object[][] SaveTestData = _localDataAccessTests.SaveTestData();
        [Theory, MemberData(nameof(SaveTestData))]
        public void SaveTest(List<TestUpdate> testUpdates)
        {
            _localDataAccessTests.SaveTest(testUpdates);
        }

        public static object[][] SaveAndGetTestData = _localDataAccessTests.SaveAndGetTestData();
        [Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<TestUpdate> testUpdates, List<Guid> idsToGet, List<TestUpdate> expected)
        {
            _localDataAccessTests.SaveAndGetTest(testUpdates, idsToGet, expected);
        }

        public static object[][] GetChangesTestData = _localDataAccessTests.GetChangesTestData();
        [Theory, MemberData(nameof(GetChangesTestData))]
        public void GetChangesTest(List<TestUpdate> testUpdates)
        {
            _localDataAccessTests.GetChangesTest(testUpdates);
        }


        public static object[][] SaveAndGetLocalLastSyncDateTestData = _localDataAccessTests.SaveAndGetLocalLastSyncDateTestData();
        [Theory, MemberData(nameof(SaveAndGetLocalLastSyncDateTestData))]
        public void SaveAndGetLocalLastSyncDateTest(DateTime expected)
        {
            _localDataAccessTests.SaveAndGetLocalLastSyncDateTest(expected);
        }


        public static object[][] SaveUpdatedOnServerTestData = _localDataAccessTests.SaveUpdatedOnServerTestData();
        [Theory, MemberData(nameof(SaveUpdatedOnServerTestData))]
        public void SaveUpdatedOnServerTest(List<TestUpdate> testUpdates)
        {
            _localDataAccessTests.SaveUpdatedOnServerTest(testUpdates);
        }


        public static object[][] SaveConflictIdTestData = _localDataAccessTests.SaveConflictIdTestData();
        [Theory, MemberData(nameof(SaveConflictIdTestData))]
        public void SaveConflictIdTest(List<TestUpdate> testUpdates, List<Conflict> conflicts, List<Conflict> expected)
        {
            _localDataAccessTests.SaveConflictIdTest(testUpdates, conflicts, expected);
        }


        public static object[][] DeleteTestData = _localDataAccessTests.DeleteTestData();
        [Theory, MemberData(nameof(DeleteTestData))]
        public void DeleteTest(List<TestUpdate> testUpdatesToDelete)
        {
            _localDataAccessTests.DeleteTest(testUpdatesToDelete);
        }
    }
}
