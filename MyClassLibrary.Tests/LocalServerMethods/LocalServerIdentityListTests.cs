using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using MyClassLibrary.Extensions;
using MyClassLibrary.LocalServerMethods;
using NuGet.Frameworks;
using Xunit.Sdk;

namespace MyClassLibrary.Tests.LocalServerMethods
{
    public class LocalServerIdentityListTests
    {
        DefaultServiceConfiguration dataService = new DefaultServiceConfiguration();

        public static readonly List<TestContent> SaveAndGetTestContents = new List<TestContent>().GenerateTestContents(3);


        public static readonly object[][] SaveAndGetTestData =
        {
                            new object[]{ SaveAndGetTestContents[0].TestObjects
                                           ,SaveAndGetTestContents[0].TestIds()
                                           ,SaveAndGetTestContents [0].TestObjects
                                        }
                            ,new object[]{ SaveAndGetTestContents[1].TestObjects
                                           ,new List<Guid> {SaveAndGetTestContents[1].TestIds()[1] }
                                           ,SaveAndGetTestContents [1].TestObjects.Where(x=>x.Id == SaveAndGetTestContents[1].TestIds()[1]).ToList()
                                        }
                             ,new object[]{ SaveAndGetTestContents[2].TestObjects
                                           ,new List<Guid> {SaveAndGetTestContents[2].TestIds()[2] }
                                           ,SaveAndGetTestContents [2].TestObjects.Where(x=>x.Id == SaveAndGetTestContents[2].TestIds()[2]).ToList()
                                        }
        };
        [Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<TestObject> objects, List<Guid> ids, List<TestObject> expected)
        {
            LocalServerEngine<TestObject> testList = new LocalServerIdentityUpdates<TestObject>(
                objects
                , dataService.serverDataAccess
                , dataService.localDataAccess
                );

            testList.Save();

            LocalServerEngine<TestObject> actualList = new LocalServerIdentity<TestObject>(
                    null
                    , dataService.serverDataAccess
                    , dataService.localDataAccess
                );

            actualList.PopulateObjects(ids);

            List<TestObject> actual = actualList.Objects;

            expected.Sort((x, y) => (x.Id.CompareTo(y.Id)));
            actual.Sort((x, y) => (x.Id.CompareTo(y.Id)));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }


        public static readonly List<TestContent> TrySyncTestContents = new List<TestContent>().GenerateTestContents(14,"SyncTesting");

        public static readonly object[][] TrySyncTestData =
        {
            new object[] {TrySyncTestContents[0].TestObjects,TrySyncTestContents[7].TestObjects,true,true,true},
            new object[] {TrySyncTestContents[1].TestObjects,TrySyncTestContents[8].TestObjects,true,true,true},
            new object[] {TrySyncTestContents[2].TestObjects,TrySyncTestContents[9].TestObjects,true,true,true},
            new object[] {TrySyncTestContents[3].TestObjects,TrySyncTestContents[10].TestObjects,true,true,true},
            new object[] {TrySyncTestContents[4].TestObjects,TrySyncTestContents[11].TestObjects,false,true,false},
            new object[] {TrySyncTestContents[5].TestObjects,TrySyncTestContents[12].TestObjects,true,false,false},
            new object[] {TrySyncTestContents[6].TestObjects,TrySyncTestContents[13].TestObjects,false,false,false}
        };

    
        [Theory, MemberData(nameof(TrySyncTestData))]
        public void TrySyncTest(List<TestObject> serverObjects,List<TestObject> localObjects,bool serverStatus,bool localStatus,bool expectedWasSuccessfull)
        {

            DateTime localLastSyncDate = DateTime.Now;

           CreateDelay(1000);//This delay allows for that for discrepancies between time on server and local
            
            dataService.localDataAccess.SaveLocalLastSyncDate<TestObject>(localLastSyncDate);
                      

            ILocalDataAccess trySyncLocalDataAccess = localStatus ? dataService.localDataAccess : new LocalSQLConnector("Error");
            IServerDataAccess trySyncServerDataAccess = serverStatus ? dataService.serverDataAccess : new ServerSQLConnector("Error");

            List<Guid> combinedIds = serverObjects.Select(x=>x.Id).ToList();
            combinedIds.AddRange(localObjects.Select(x=>x.Id).ToList());

            combinedIds.Distinct();
              
            dataService.serverDataAccess.SaveToServer(serverObjects);

            dataService.localDataAccess.SaveToLocal(localObjects);

            
            LocalServerEngine<TestObject> testList = new LocalServerIdentity<TestObject>(null,trySyncServerDataAccess,trySyncLocalDataAccess);

            bool actualWasSuccessfull = testList.TrySync();

            List<TestObject> actualServerResult = dataService.serverDataAccess.GetFromServer<TestObject>(combinedIds);

            List<TestObject> actualLocalResult = dataService.localDataAccess.GetFromLocal<TestObject>(combinedIds);

            List<TestObject> expectedServerResult = serverObjects;

            List<TestObject> expectedLocalResult = localObjects;

            if (expectedWasSuccessfull)
            {
                expectedServerResult.AddRange(localObjects.Where(x => x.UpdatedOnServer == null));
                expectedLocalResult.AddRange(serverObjects.Where(x=>x.UpdatedOnServer > localLastSyncDate));      
            } 

            Assert.True(expectedWasSuccessfull==actualWasSuccessfull,$"TrySync Was Successfull Test: expected={expectedWasSuccessfull}, actual={actualWasSuccessfull}.");


            actualServerResult.Sort((x,y)=> $"{x.Id.ToString()}.{x.Created}".CompareTo($"{y.Id.ToString()}.{y.Created}"));
            actualLocalResult.Sort((x, y) => $"{x.Id.ToString()}.{x.Created}".CompareTo($"{y.Id.ToString()}.{y.Created}")); 
            expectedServerResult.Sort((x, y) => $"{x.Id.ToString()}.{x.Created}".CompareTo($"{y.Id.ToString()}.{y.Created}"));
            expectedLocalResult.Sort((x, y) => $"{x.Id.ToString()}.{x.Created}".CompareTo($"{y.Id.ToString()}.{y.Created}"));

            //Not perfect but inserts actual result for UpdatedOnServer into expected result as difficult to get this within test.
            //The final test comparing if server data = local data provides an additional test that this figure is correct.
            for (int i = 0;i<expectedLocalResult.Count();i++)
            {
                if (expectedLocalResult[i].UpdatedOnServer == null && expectedWasSuccessfull == true)
                {
                    expectedLocalResult[i].UpdatedOnServer = actualLocalResult[i].UpdatedOnServer;
                }
            }


            Assert.Equal(JsonSerializer.Serialize(expectedServerResult), JsonSerializer.Serialize(actualServerResult));

            Assert.Equal(JsonSerializer.Serialize(expectedLocalResult), JsonSerializer.Serialize(actualLocalResult));

            if (expectedWasSuccessfull) { Assert.Equal(JsonSerializer.Serialize(actualLocalResult), JsonSerializer.Serialize(actualServerResult));}

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

            LocalServerEngine<TestObject> actualList = new LocalServerIdentityUpdates<TestObject>(actualTestContent.TestObjects);
            
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


            LocalServerEngine<TestObject> actualList = new LocalServerIdentityUpdates<TestObject>(actualTestContent.TestObjects);

            actualList.SortByCreated();

            List<TestObject> actual = actualList.Objects;

            List<TestObject> expected = expectedTestContent.TestObjects;

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        }


        [Fact]
        public void SortByIdAndCreatedTest()
        {
            List<Guid> overrideIds = new List<Guid>();

            for (int i = 0; i < 10; i++)
            {
                overrideIds.Add(Guid.NewGuid());
            }

            TestContent actualTestContent = new List<TestContent>().GenerateTestContents(1, "Unsorted", overrideIds, DateTime.Now)[0];
            TestContent expectedTestContent = new List<TestContent>().GenerateTestContents(1, "History", overrideIds, DateTime.Now)[0];

            LocalServerEngine<TestObject> actualList = new LocalServerIdentityUpdates<TestObject>(actualTestContent.TestObjects);
            actualList.SortByIdAndCreated();
            List<TestObject> actual = actualList.Objects;
                
           

            List<TestObject> expected = expectedTestContent.TestObjects;

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        }




        [Fact]
        public void FindConflictsTest ()
        {
            List<Guid> draftIds = Guid.NewGuid().GenerateList(10);
            DateTime ServerCreatedDate = DateTime.Now;
            List<TestObject> changesFromServer = new List<TestObject>()
                        {
                            new TestObject(draftIds[1], ServerCreatedDate, "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                            new TestObject(draftIds[1], ServerCreatedDate.AddSeconds(1), "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                            new TestObject(draftIds[1], ServerCreatedDate.AddSeconds(2), "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                            new TestObject(draftIds[2], ServerCreatedDate.AddSeconds(3), "mcarter", null, true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestObject(draftIds[2], ServerCreatedDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestObject(draftIds[3], ServerCreatedDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" }),
                            new TestObject(draftIds[4], ServerCreatedDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" }),
                            new TestObject(draftIds[4], ServerCreatedDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" })
                        };

            CreateDelay(200);

            DateTime LocalCreatedDate = DateTime.Now;
            List<TestObject> changesFromLocal = new List<TestObject>()
                        {
                            new TestObject(draftIds[5], LocalCreatedDate, "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" }),
                            new TestObject(draftIds[5], LocalCreatedDate.AddSeconds(1), "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" }),
                            new TestObject(draftIds[5], LocalCreatedDate.AddSeconds(2), "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" }),
                            new TestObject(draftIds[2], LocalCreatedDate.AddSeconds(3), "mcarter", null, true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestObject(draftIds[2], LocalCreatedDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" }),
                            new TestObject(draftIds[3], LocalCreatedDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas", "Carrots" }),
                            new TestObject(draftIds[6], LocalCreatedDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger", "Chicken", "Chocolate" }),
                            new TestObject(draftIds[6], LocalCreatedDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger", "Chicken", "Chocolate", "Lindor Balls" })
                        };


            List<Conflict> expected = new List<Conflict>()
                        {
                            new Conflict(draftIds[2], LocalCreatedDate.AddSeconds(3)),
                            new Conflict(draftIds[2], LocalCreatedDate.AddSeconds(4)),
                            new Conflict(draftIds[3], LocalCreatedDate.AddSeconds(5)),
                            new Conflict(draftIds[2], ServerCreatedDate.AddSeconds(3)),
                            new Conflict(draftIds[2], ServerCreatedDate.AddSeconds(4)),
                            new Conflict(draftIds[3], ServerCreatedDate.AddSeconds(5))
                         };
            
            
            expected.Sort((x,y)=>x.ObjectCreated.CompareTo(y.ObjectCreated));   


            LocalServerEngine<TestObject> test = new LocalServerIdentity<TestObject>();

            List<Conflict> actual = test.FindConflicts(changesFromServer, changesFromLocal);
            actual.Sort((x, y) => x.ObjectCreated.CompareTo(y.ObjectCreated));

            List<Conflict> actualMinusConflictID = actual.Select(x => new Conflict(x.ObjectId, x.ObjectCreated)).ToList();

            List<string> actualIdAndConflictID = actual.Select(x => x.ObjectId.ToString() + x.ConflictId.ToString()).ToList();

            //expected to should match actual (without conflictID generated within function)
            Assert.Equal(JsonSerializer.Serialize(expected),JsonSerializer.Serialize(actualMinusConflictID));

            //the conflictIds generated within function should be distinct for each id.
           // Assert.True(actualIdAndConflictID.Count() == 3, "Error with ConflictIDs generated in function.");         
            }

        

        

        [Fact]
        public void SaveConflictIdsTest()
        {
           List<TestContent> testContents = new List<TestContent>().GenerateTestContents(1);

            dataService.serverDataAccess.SaveToServer(testContents[0].TestObjects);
            dataService.localDataAccess.SaveToLocal(testContents[0].TestObjects);

            List<Conflict> conflicts = new List<Conflict>();

            Guid conflictID = Guid.NewGuid();

            conflicts.Add(new Conflict(testContents[0].TestObjects[1].Id, testContents[0].TestObjects[1].Created, conflictID));

            testContents[0].TestObjects[1].ConflictId = conflictID;


            LocalServerEngine<TestObject> testList = new LocalServerIdentity<TestObject>();

           



            List<TestObject> expected = testContents[0].TestObjects;
            expected.Sort((x,y)=>x.Id.CompareTo(y.Id));
            expected.Select(x => (x.Id, x.Created, x.ConflictId)).ToList();





            List<TestObject> actualServer = dataService.serverDataAccess.GetFromServer<TestObject>(testContents[0].TestIds());
            actualServer.Sort((x, y) => x.Id.CompareTo(y.Id));
            actualServer.Select(x => (x.Id, x.Created, x.ConflictId)).ToList();

            List<TestObject> actualLocal = dataService.localDataAccess.GetFromLocal<TestObject>(testContents[0].TestIds());
            actualLocal.Sort((x, y) => x.Id.CompareTo(y.Id));
            actualLocal.Select(x => (x.Id, x.Created, x.ConflictId)).ToList();

            Assert.Equal(expected, actualServer);
            Assert.Equal(expected, actualLocal);

        }


        async void CreateDelay(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }

        
       

    }
}
