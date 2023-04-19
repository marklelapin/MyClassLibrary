using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyClassLibrary.LocalServerMethods;
using Xunit.Sdk;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class LocalServerIdentityListTests
    {
        DataService dataService = new DataService();

        public static readonly List<TestContent> SaveAndHistoryTestContents = new List<TestContent>().GenerateTestContents(3);


        public static readonly object[][] SaveAndGetTestData =
        {
                            new object[]{ SaveAndHistoryTestContents[0].TestObjects
                                           ,SaveAndHistoryTestContents[0].TestIds()
                                           ,SaveAndHistoryTestContents [0].TestObjects
                                        }
                            ,new object[]{ SaveAndHistoryTestContents[1].TestObjects
                                           ,new List<Guid> {SaveAndHistoryTestContents[1].TestIds()[1] }
                                           ,SaveAndHistoryTestContents [1].TestObjects.Where(x=>x.Id == SaveAndHistoryTestContents[1].TestIds()[1]).ToList()
                                        }
                             ,new object[]{ SaveAndHistoryTestContents[2].TestObjects
                                           ,new List<Guid> {SaveAndHistoryTestContents[2].TestIds()[2] }
                                           ,SaveAndHistoryTestContents [2].TestObjects.Where(x=>x.Id == SaveAndHistoryTestContents[2].TestIds()[2]).ToList()
                                        }
        };
        [Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<TestObject> objects, List<Guid> ids, List<TestObject> expected)
        {
            LocalServerIdentityList<TestObject> testList = new LocalServerIdentityList<TestObject>(
                objects
                , dataService.serverDataAccess
                , dataService.localDataAccess
                );

            testList.Save();

            LocalServerIdentityList<TestObject> actualList = new LocalServerIdentityList<TestObject>(
                    null
                    , dataService.serverDataAccess
                    , dataService.localDataAccess
                );

            actualList.GetObjects(ids);

            List<TestObject> actual = actualList.Objects;

            expected.Sort((x, y) => (x.Id.CompareTo(y.Id)));
            actual.Sort((x, y) => (x.Id.CompareTo(y.Id)));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }


        public static readonly List<TestContent> TrySyncTestContents = new List<TestContent>().GenerateTestContents(14);

        public static readonly List<TestContent> TrySyncExpectedResults = new List<TestContent>().GenerateTestContents(7);

        public static readonly object[][] TrySyncTestData =
        {
            new object[] {TrySyncTestContents[0].TestObjects,TrySyncTestContents[7].TestObjects,true,true,TrySyncExpectedResults[0].TestObjects,true},
            new object[] {TrySyncTestContents[1].TestObjects,TrySyncTestContents[8].TestObjects,true,true,TrySyncExpectedResults[1].TestObjects,true},
            new object[] {TrySyncTestContents[2].TestObjects,TrySyncTestContents[9].TestObjects,true,true,TrySyncExpectedResults[2].TestObjects,true},
            new object[] {TrySyncTestContents[3].TestObjects,TrySyncTestContents[10].TestObjects,true,true,TrySyncExpectedResults[3].TestObjects,true},
            new object[] {TrySyncTestContents[4].TestObjects,TrySyncTestContents[11].TestObjects,false,true,TrySyncExpectedResults[4].TestObjects,false},
            new object[] {TrySyncTestContents[5].TestObjects,TrySyncTestContents[12].TestObjects,true,false,TrySyncExpectedResults[5].TestObjects,false},
            new object[] {TrySyncTestContents[6].TestObjects,TrySyncTestContents[13].TestObjects,false,false,TrySyncExpectedResults[6].TestObjects,false}
        };


        [Theory, MemberData(nameof(TrySyncTestData))]
        public void TrySyncTest(List<TestObject> serverObjects,List<TestObject> localObjects,bool serverStatus,bool localStatus,List<TestObject> expectedTestObjects,bool expectedWasSuccessful)
        {

            DateTime localLastSyncDate = DateTime.Now;

            CreateDelay(2000);
            
            dataService.localDataAccess.SaveLocalLastSyncDate<TestObject>(localLastSyncDate);
                      

            ILocalDataAccess trySyncLocalDataAccess = localStatus ? dataService.localDataAccess : new LocalSQLConnector("Error");
            IServerDataAccess trySyncServerDataAccess = serverStatus ? dataService.serverDataAccess : new ServerSQLConnector("Error");

            List<Guid> combinedIds = serverObjects.Select(x=>x.Id).ToList();
            combinedIds.AddRange(localObjects.Select(x=>x.Id).ToList());

            combinedIds.Distinct();
              
            dataService.serverDataAccess.SaveToServer(serverObjects);

            dataService.localDataAccess.SaveToLocal(localObjects);

            
            LocalServerIdentityList<TestObject> testList = new LocalServerIdentityList<TestObject>(null,trySyncServerDataAccess,trySyncLocalDataAccess);

            bool actualBool = testList.TrySync();

            dataService.localDataAccess.GetFromLocal<TestObject>(combinedIds);

            dataService.localDataAccess.GetFromLocal<TestObject>(combinedIds);

            Assert.True(actualBool,"Return value of TrySync for Was Successful");

        }




        [Fact] public void SortByIdTest() 
        {
            List<Guid> overrideIds = new List<Guid>();

            for (int i = 0; i < 10; i++)
            {
                overrideIds.Add(Guid.NewGuid());
            }

            TestContent actualTestContent = new List<TestContent>().GenerateTestContents(1, "Unsorted",overrideIds, DateTime.Now)[0];
            TestContent expectedTestContent = new List<TestContent>().GenerateTestContents(1, "SortedById", overrideIds, DateTime.Now)[0];

            LocalServerIdentityList<TestObject> actualList = new LocalServerIdentityList<TestObject>(actualTestContent.TestObjects);
            
            actualList.SortById();

            List<TestObject> actual = actualList.Objects;

            List<TestObject> expected = expectedTestContent.TestObjects;

            Assert.Equal(JsonSerializer.Serialize(expected),JsonSerializer.Serialize(actual));

        }


        [Fact]
        public void SortByCreatedTest()
        {
            List<Guid> overrideIds = new List<Guid>();

            for (int i = 0; i < 10; i++)
            {
                overrideIds.Add(Guid.NewGuid());
            }

            TestContent actualTestContent = new List<TestContent>().GenerateTestContents(1, "Unsorted", overrideIds, DateTime.Now)[0];
            TestContent expectedTestContent = new List<TestContent>().GenerateTestContents(1, "SortedByCreated", overrideIds, DateTime.Now)[0];


            LocalServerIdentityList<TestObject> actualList = new LocalServerIdentityList<TestObject>(actualTestContent.TestObjects);

            actualList.SortByCreated();

            List<TestObject> actual = actualList.Objects;

            List<TestObject> expected = expectedTestContent.TestObjects;

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        }


        [Fact]
        public void HistoryTest()
        {
            List<Guid> overrideIds = new List<Guid>();

            for (int i = 0; i < 10; i++)
            {
                overrideIds.Add(Guid.NewGuid());
            }

            TestContent actualTestContent = new List<TestContent>().GenerateTestContents(1, "Unsorted", overrideIds, DateTime.Now)[0];
            TestContent expectedTestContent = new List<TestContent>().GenerateTestContents(1, "History", overrideIds, DateTime.Now)[0];

            LocalServerIdentityList<TestObject> actualList = new LocalServerIdentityList<TestObject>(actualTestContent.TestObjects);
            
            List<TestObject> actual = actualList.History();

            List<TestObject> expected = expectedTestContent.TestObjects;

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        }


        async void CreateDelay(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }


    }
}
