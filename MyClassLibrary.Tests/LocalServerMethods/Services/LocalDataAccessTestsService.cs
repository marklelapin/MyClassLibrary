using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace MyClassLibrary.Tests.LocalServerMethods.Services
{
    public class LocalDataAccessTestsService<T> : ILocalDataAccessTests<T> where T : LocalServerIdentityUpdate
    {

        private ITestContent<T> _testContent;
        private IServiceConfiguration _serviceConfiguration;
        private readonly ILocalDataAccess _localDataAccess;

        public LocalDataAccessTestsService(IServiceConfiguration serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;
            _localDataAccess = _serviceConfiguration.LocalDataAccess();
            _testContent = _serviceConfiguration.TestContent<T>();
        }


        public object[][] SaveTestData() { return new object[][] { new object[] { _testContent.Generate(1, "Default")[0] } }; }
        //[Theory, MemberData(nameof(SaveTestData))]
        public void SaveTest(List<T> updates)
        {
            _localDataAccess.SaveToLocal(updates);
            Assert.True(true);
        }



        public object[][] SaveAndGetTestData()
        {
            List<List<T>> saveAndGetTestContents = _testContent.Generate(3, "Default");

            return new object[][] {
                            new object[] {
                                            saveAndGetTestContents[0]
                                            ,_testContent.ListIds(saveAndGetTestContents[0])
                                            ,saveAndGetTestContents[0]
                                           },
                            new object[]
                                        {
                                            saveAndGetTestContents[1]
                                            ,_testContent.ListIds(saveAndGetTestContents[1])
                                            ,saveAndGetTestContents[1]
                                           },
                            new object[]
                                        {
                                            saveAndGetTestContents[2]
                                            ,new List<Guid> {_testContent.ListIds(saveAndGetTestContents[2])[2] }
                                            ,saveAndGetTestContents[2].Where(x => x.Id == _testContent.ListIds(saveAndGetTestContents[2])[2]).ToList()
                                           }
            };
        }
        // [Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<T> testUpdates, List<Guid> idsToGet, List<T> expected)
        {
            _localDataAccess.SaveToLocal(testUpdates);

            List<T> actual = _localDataAccess.GetFromLocal<T>(idsToGet);

            actual.Sort((x, y) => x.Id.CompareTo(y.Id));
            expected.Sort((x, y) => x.Id.CompareTo(y.Id));

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }



        public object[][] GetChangesTestData() { return new object[][] { new object[] { _testContent.Generate(1, "Default")[0] } }; }
        //[Theory, MemberData(nameof(GetChangesTestData))]
        public void GetChangesTest(List<T> updates) //also tests functionality of passing null into GetFromLocal
        {

            _localDataAccess.SaveToLocal(updates);
            List<T> allUpdates = _localDataAccess.GetFromLocal<T>(null);

            List<T> expected = allUpdates.Where(x => x.UpdatedOnServer == null).ToList();

            List<T> actual = _localDataAccess.GetChangesFromLocal<T>();

            actual.Sort((x, y) => x.Id.CompareTo(y.Id));
            expected.Sort((x, y) => x.Id.CompareTo(y.Id));

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }


        public object[][] SaveAndGetLocalLastSyncDateTestData() { return new object[][] { new object[] { DateTime.Now } }; }
        // [Theory, MemberData(nameof(SaveandGetLocalLastSyncDateTestData))]
        public void SaveAndGetLocalLastSyncDateTest(DateTime expected)
        {
            _localDataAccess.SaveLocalLastSyncDate<T>(expected);

            DateTime actual = _localDataAccess.GetLocalLastSyncDate<T>();

            Assert.Equal(expected, actual);
        }



        public object[][] SaveUpdatedOnServerTestData() { return new object[][] { new object[] { _testContent.Generate(1, "Default")[0] } }; }
        //[Theory, MemberData(nameof(SaveUpdatedOnServerTestData))]
        public void SaveUpdatedOnServerTest(List<T> updates)
        {
            DateTime updatedOnServer = DateTime.Now;

            updates = _testContent.Generate(1, "Default")[0];

            _localDataAccess.SaveToLocal(updates);

            _localDataAccess.SaveUpdatedOnServerToLocal(updates, updatedOnServer);

            List<T> actualUpdated = _localDataAccess.GetFromLocal<T>(_testContent.ListIds(updates));

            List<T> actualNotUpdated = _localDataAccess.GetFromLocal<T>().Where(x => !actualUpdated.Any(y => y.Id == x.Id)).ToList();

            Assert.True(actualNotUpdated.Where(x => x.UpdatedOnServer == updatedOnServer).Count() == 0, "No Other Objects Have UpdatedOnServerDate");
            Assert.True(actualUpdated.Where(x => x.UpdatedOnServer == updatedOnServer).Count() == updates.Count());
        }



        
        private List<Conflict> Conflicts(List<List<T>> saveConflictIdTestContents)
        {
            List<Conflict> output = new List<Conflict>();
            List<T> conflictedObjects = new List<T>();
            conflictedObjects.Add(saveConflictIdTestContents[0][1]);
            conflictedObjects.Add(saveConflictIdTestContents[0][5]);

            output.Add(new Conflict(conflictedObjects[0].Id, conflictedObjects[0].Created, Guid.NewGuid()));
            output.Add(new Conflict(conflictedObjects[1].Id, conflictedObjects[1].Created, Guid.NewGuid()));

            return output;
        }
        public object[][] SaveConflictIdTestData()
        {
            List<List<T>> saveConflictIdTestContents = _testContent.Generate(2, "Default");

            List<Conflict> conflicts = Conflicts(saveConflictIdTestContents);

            return new object[][] {
                    new object[]{saveConflictIdTestContents[0],conflicts,conflicts},
                    new object[]{saveConflictIdTestContents[1],new List<Conflict>(),new List<Conflict>()}
            };
        }
        //[Theory, MemberData(nameof(saveConflictIdTestData))]
        public void SaveConflictIdTest(List<T> testUpdates, List<Conflict> conflicts, List<Conflict> expected)
        {
            _localDataAccess.SaveToLocal(testUpdates);
            _localDataAccess.SaveConflictIdsToLocal<T>(conflicts);

            List<Conflict> actual = new List<Conflict>();

            actual = _localDataAccess.GetFromLocal<T>(_testContent.ListIds(testUpdates))
                                                    .Where(x => x.ConflictId != null)
                                                    .Select(x => new Conflict(x.Id, x.Created, x.ConflictId))
                                                    .ToList();

            actual.Sort((x, y) => x.UpdateCreated.CompareTo(y.UpdateCreated));
            expected.Sort((x, y) => x.UpdateCreated.CompareTo(y.UpdateCreated));

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
        }



        //public object[][] DeleteTestData() { return new object[][] { new object[] { _testContent.Generate(1, "Default")[0] } };}
        ////[Theory, MemberData(nameof(DeleteTestContent))]
        //public void DeleteTest(List<T> testUpdatesToDelete)
        //{
        //    throw new NotImplementedException(testUpdatesToDelete.ToString());
        //}


        async private void InsertDelay(int milliSeconds)
        {
            await Task.Delay(milliSeconds);
        }

    }
}
