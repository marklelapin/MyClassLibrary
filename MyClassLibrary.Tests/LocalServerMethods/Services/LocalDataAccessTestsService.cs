using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Services
{
    public class LocalDataAccessTestsService<T> : ILocalDataAccessTests<T> where T : LocalServerIdentityUpdate
    {
        
        private static IServiceConfiguration _serviceConfiguration;

        private static IServiceConfiguration ServiceConfiguration
        {
            get { return _serviceConfiguration; }
            set { _serviceConfiguration = value; }
        }

        private static ILocalDataAccess localDataAccess = ServiceConfiguration.LocalDataAccess();

        private static ITestContent<T> testContent = ServiceConfiguration.TestContent<T>();



        public static readonly object[][] SaveTestUpdates = { new object[] { testContent.Generate(1, "Default")[0] } };
        [Theory, MemberData(nameof(SaveTestUpdates))]
        public void SaveTest(List<T> updates)
        {
            localDataAccess.SaveToLocal(updates);
            Assert.True(true);
        }



        private static readonly List<List<T>> SaveAndGetTestContents = testContent.Generate(3, "Default");
        public static readonly object[][] SaveAndGetTestData =
        {
            new object[] {
                            SaveAndGetTestContents[0]
                            ,testContent.ListIds(SaveAndGetTestContents[0])
                            ,SaveAndGetTestContents[0]
                           },
            new object[]
                        {
                            SaveAndGetTestContents[1]
                            ,testContent.ListIds(SaveAndGetTestContents[1])
                            ,SaveAndGetTestContents[1]
                           },
            new object[]
                        {
                            SaveAndGetTestContents[2]
                            ,testContent.ListIds(SaveAndGetTestContents[2])[2]
                            ,SaveAndGetTestContents[2].Where(x => x.Id == testContent.ListIds(SaveAndGetTestContents[2])[2]).ToList()
                           }
        };
        [Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<T> testUpdates, List<Guid> idsToGet, List<T> expected)
        {
            localDataAccess.SaveToLocal(testUpdates);

            List<T> actual = localDataAccess.GetFromLocal<T>(idsToGet);

            actual.Sort((x, y) => x.Id.CompareTo(y.Id));
            expected.Sort((x, y) => x.Id.CompareTo(y.Id));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }



        public static readonly object[][] GetChangesTestContent = { new object[] { testContent.Generate(1, "Default")[0] } };
        [Theory, MemberData(nameof(GetChangesTestContent))]
        public void GetChangesTest(List<T> updates) //also tests functionality of passing null into GetFromLocal
        {

            localDataAccess.SaveToLocal(updates);
            List<T> allUpdates = localDataAccess.GetFromLocal<T>(null);

            List<T> expected = allUpdates.Where(x => x.UpdatedOnServer == null).ToList();

            List<T> actual = localDataAccess.GetChangesFromLocal<T>();

            actual.Sort((x, y) => x.Id.CompareTo(y.Id));
            expected.Sort((x, y) => x.Id.CompareTo(y.Id));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }



        [Fact]
        public void SaveAndGetLocalLastSyncDate()
        {
            DateTime expected = DateTime.Now;
            localDataAccess.SaveLocalLastSyncDate<T>(expected);

            DateTime actual = localDataAccess.GetLocalLastSyncDate<T>();

            Assert.Equal(expected, actual);
        }



        public static readonly object[][] SaveUpdatedOnServerTestContent = { new object[] { testContent.Generate(1, "Default")[0] } };
        [Theory, MemberData(nameof(SaveUpdatedOnServerTestContent))]
        public void SaveUpdatedOnServerTest(List<T> updates)
        {
            DateTime updatedOnServer = DateTime.Now;

            updates = testContent.Generate(1, "default")[0];

            localDataAccess.SaveToLocal(updates);

            localDataAccess.SaveUpdatedOnServerToLocal(updates, updatedOnServer);

            List<T> actualUpdated = localDataAccess.GetFromLocal<T>(testContent.ListIds(updates));

            List<T> actualNotUpdated = localDataAccess.GetFromLocal<T>().Where(x => !actualUpdated.Any(y => y.Id == x.Id)).ToList();

            Assert.True(actualNotUpdated.Where(x => x.UpdatedOnServer == updatedOnServer).Count() == 0, "No Other Objects Have UpdatedOnServerDate");
            Assert.True(actualUpdated.Where(x => x.UpdatedOnServer == updatedOnServer).Count() == updates.Count());
        }


        public static readonly List<List<T>> saveConflictIDTestContents = testContent.Generate(2,"Default");

        public static List<Conflict> conflicts { get
                 {
                List<Conflict> output = new List<Conflict>();
                List<T> conflictedObjects = new List<T>();
                conflictedObjects.Add(saveConflictIDTestContents[0][1]);
                conflictedObjects.Add(saveConflictIDTestContents[0][5]);
                
                conflicts.Add(new Conflict(conflictedObjects[0].Id, conflictedObjects[0].Created, Guid.NewGuid()));
                conflicts.Add(new Conflict(conflictedObjects[1].Id, conflictedObjects[1].Created, Guid.NewGuid()));

                return output;
                }
         } 


        public static readonly object[][] saveConflictIDTestData =
        {
            new object[]{saveConflictIDTestContents[0],conflicts,conflicts},
            new object[]{saveConflictIDTestContents[1],new List<Conflict>(),new List<Conflict>()}
        };

        [Theory, MemberData(nameof(saveConflictIDTestData))]
        public void SaveConflictIdTest(List<T> testUpdates, List<Conflict> conflicts, List<Conflict> expected)
        {
            localDataAccess.SaveToLocal(testUpdates);
            localDataAccess.SaveConflictIdsToLocal<T>(conflicts);

            List<Conflict> actual = new List<Conflict>();

            actual =localDataAccess.GetFromLocal<T>(testContent.ListIds(testUpdates))
                                                    .Where(x => x.ConflictId != null)
                                                    .Select(x => new Conflict(x.Id, x.Created, x.ConflictId))
                                                    .ToList();

            actual.Sort((x, y) => x.ObjectCreated.CompareTo(y.ObjectCreated));
            expected.Sort((x, y) => x.ObjectCreated.CompareTo(y.ObjectCreated));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }



        public static readonly object[][] DeleteTestContent = { new object[] { testContent.Generate(1, "Default")[0] } };
        [Theory, MemberData(nameof(DeleteTestContent))]
        public void DeleteTest(List<T> testUpdates)
        {
            throw new NotImplementedException();
        }


        //async static private void InsertDelay(int milliSeconds)
        //{
        //    await Task.Delay(milliSeconds);
        //}

    }
}
