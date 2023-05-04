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
    public class TestUpdateServerDataAccessTests 
    {

        private static IServiceConfiguration _serviceConfiguration = new TestServiceConfiguration();

        private static IServerDataAccessTests<TestUpdate> _serverDataAccessTests = new ServerDataAccessTestsService<TestUpdate>(_serviceConfiguration);


        public static readonly object[][] SaveTestData = _serverDataAccessTests.SaveTestData();
        [Theory, MemberData(nameof(SaveTestData))]
        public void SaveTest(List<TestUpdate> testUpdates)
        {
            _serverDataAccessTests.SaveTest(testUpdates);
        }


        public static readonly object[][] SaveAndGetTestData = _serverDataAccessTests.SaveAndGetTestData();
        [Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<TestUpdate> testUpdates, List<Guid> getIds, List<TestUpdate> expected)
        {
            _serverDataAccessTests.SaveAndGetTest(testUpdates, getIds, expected);
        }


        public static readonly object[][] GetChangesTestData = _serverDataAccessTests.GetChangesTestData();
        [Theory, MemberData(nameof(GetChangesTestData))]
        public void GetChangesTest(List<TestUpdate> testUpdates, int lastSyncDateAdjustment, List<TestUpdate> expected)
        {
            _serverDataAccessTests.GetChangesTest(testUpdates,lastSyncDateAdjustment, expected);
        }


        public static readonly object[][] SaveConflictIdTestData = _serverDataAccessTests.SaveConflictIdTestData();
        [Theory, MemberData(nameof(SaveConflictIdTestData))]
        public void SaveConflictIdTest(List<TestUpdate> testUpdate, List<Conflict> conflicts, List<Conflict> expected)
        {
            _serverDataAccessTests.SaveConflictIdTest(testUpdate, conflicts, expected);
        }
        


        //TODO add in Delete Test when funcionality is setup
        //public static readonly object[][] DeleteTestData = _serverDataAccessTests.DeleteTestData();
        //[Theory,MemberData(nameof(DeleteTestData))]
        //public void DeleteTest(List<TestUpdate> testUpdatesToDelete)
        //{
        //   _serverDataAccessTests.DeleteTest(testUpdatesToDelete);
        //}

       

    }
}
