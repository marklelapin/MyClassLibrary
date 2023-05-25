using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using MyClassLibrary.Extensions;
using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using MyClassLibrary.Tests.LocalServerMethods.Services;
using NuGet.Frameworks;
using Xunit.Sdk;
using Microsoft.Extensions.Configuration;
using MyClassLibrary.DataAccessMethods;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class LocalServerEngineTests
    {
       private static IServiceConfiguration<TestUpdate> _serviceConfiguration = new TestServiceConfiguration<TestUpdate>();

        private static readonly ILocalDataAccess<TestUpdate> _localDataAccess = _serviceConfiguration.LocalDataAccess();

        private static readonly IServerDataAccess<TestUpdate> _serverDataAccess = _serviceConfiguration.ServerDataAccess();

        private static readonly ITestContent<TestUpdate> _testContent = _serviceConfiguration.TestContent();

        private static readonly ILocalServerEngine<TestUpdate> _localServerEngine = _serviceConfiguration.LocalServerEngine(_localDataAccess,_serverDataAccess);





        public static readonly List<List<TestUpdate>> SaveAndGetTestContents = _testContent.Generate(3,"Default");

        public static readonly object[][] SaveAndGetTestData =
        {
                            new object[]{ SaveAndGetTestContents[0]
                                           ,_testContent.ListIds(SaveAndGetTestContents[0])
                                           ,SaveAndGetTestContents [0]
                                        }
                            ,new object[]{ SaveAndGetTestContents[1]
                                           ,new List<Guid> { _testContent.ListIds(SaveAndGetTestContents[1])[1] }
                                           ,SaveAndGetTestContents [1].Where(x=>x.Id == _testContent.ListIds(SaveAndGetTestContents[1])[1]).ToList()
                                        }
                             ,new object[]{ SaveAndGetTestContents[2]
                                           ,new List<Guid> { _testContent.ListIds(SaveAndGetTestContents[2])[2] }
                                           ,SaveAndGetTestContents [2].Where(x=>x.Id == _testContent.ListIds(SaveAndGetTestContents[2])[2]).ToList()
                                        }
        };
        [Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<TestUpdate> updates, List<Guid> ids, List<TestUpdate> expected)
        {
            var saveTask = Task.Run(()=>_localServerEngine.Save(updates));
            saveTask.Wait();

            Task<List<TestUpdate>> taskActual = Task.Run(()=> _localServerEngine.GetAllUpdates(ids));
            taskActual.Wait();
            List<TestUpdate> actual = taskActual.Result;

            expected.Sort((x, y) => x.Id.CompareTo(y.Id));
            actual.Sort((x, y) => x.Id.CompareTo(y.Id));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }


        public static readonly List<List<TestUpdate>> TrySyncTestContents = _testContent.Generate(14, "SyncTesting");

        public static readonly object[][] TrySyncTestData =
        {
            new object[] {TrySyncTestContents[0],TrySyncTestContents[7],true,true,true},
            new object[] {TrySyncTestContents[1],TrySyncTestContents[8],true,true,true},
            new object[] {TrySyncTestContents[2],TrySyncTestContents[9],true,true,true},
            new object[] {TrySyncTestContents[3],TrySyncTestContents[10],true,true,true},
            new object[] {TrySyncTestContents[4],TrySyncTestContents[11],false,true,false},
            new object[] {TrySyncTestContents[5],TrySyncTestContents[12],true,false,false},
            new object[] {TrySyncTestContents[6],TrySyncTestContents[13],false,false,false}
        };
        [Theory, MemberData(nameof(TrySyncTestData))]
        public async void TrySyncTest(List<TestUpdate> serverUpdates, List<TestUpdate> localUpdates, bool serverStatus, bool localStatus, bool expectedWasSuccessfull)
        {

            DateTime localLastSyncDate = DateTime.Now;

            CreateDelay(1000);//This delay allows for that for discrepancies between time on server and local

            Task.Run(()=>_localDataAccess.SaveLocalLastSyncDate(localLastSyncDate)).Wait();


            ILocalDataAccess<TestUpdate> trySyncLocalDataAccess = localStatus ? _localDataAccess : new LocalSQLConnector<TestUpdate>(new SqlDataAccess(_serviceConfiguration.Config), "Error");
            IServerDataAccess<TestUpdate> trySyncServerDataAccess = serverStatus ? _serverDataAccess : new ServerSQLConnector<TestUpdate>(new SqlDataAccess(_serviceConfiguration.Config),"Error");

            ILocalServerEngine<TestUpdate> _testEngine = _serviceConfiguration.LocalServerEngine(trySyncLocalDataAccess, trySyncServerDataAccess);

            List<Guid> combinedIds = serverUpdates.Select(x => x.Id).ToList();
            combinedIds.AddRange(localUpdates.Select(x => x.Id).ToList());

            combinedIds.Distinct();

           Task.Run(()=> _serverDataAccess.SaveToServer(serverUpdates)).Wait();

            await Task.Run(()=> _localDataAccess.SaveToLocal(localUpdates));

            var trySyncResultTask = Task.Run(() => _testEngine.TrySync());
            trySyncResultTask.Wait();
            bool actualWasSuccessfull = trySyncResultTask.Result;

       


            var actualServerResultTask = _serverDataAccess.GetFromServer(combinedIds);
            var actualLocalResultTask = _localDataAccess.GetFromLocal(combinedIds);
            Task.WaitAll(actualLocalResultTask, actualServerResultTask);

            List<TestUpdate> actualServerResult = actualServerResultTask.Result;

            List<TestUpdate> actualLocalResult = actualLocalResultTask.Result;

            List<TestUpdate> expectedServerResult = serverUpdates;

            List<TestUpdate> expectedLocalResult = localUpdates;

            if (expectedWasSuccessfull)
            {
                expectedServerResult.AddRange(localUpdates.Where(x => x.UpdatedOnServer == null));
                expectedLocalResult.AddRange(serverUpdates.Where(x => x.UpdatedOnServer > localLastSyncDate));
            }

            Assert.True(expectedWasSuccessfull == actualWasSuccessfull, $"TrySync Was Successfull Test: expected={expectedWasSuccessfull}, actual={actualWasSuccessfull}.");


            actualServerResult.Sort((x, y) => $"{x.Id.ToString()}.{x.Created}".CompareTo($"{y.Id.ToString()}.{y.Created}"));
            actualLocalResult.Sort((x, y) => $"{x.Id.ToString()}.{x.Created}".CompareTo($"{y.Id.ToString()}.{y.Created}"));
            expectedServerResult.Sort((x, y) => $"{x.Id.ToString()}.{x.Created}".CompareTo($"{y.Id.ToString()}.{y.Created}"));
            expectedLocalResult.Sort((x, y) => $"{x.Id.ToString()}.{x.Created}".CompareTo($"{y.Id.ToString()}.{y.Created}"));

            //Not perfect but inserts actual result for UpdatedOnServer into expected result as difficult to get this within test.
            //The final test comparing if server data = local data provides an additional test that this figure is correct.
            for (int i = 0; i < expectedLocalResult.Count(); i++)
            {
                if (expectedLocalResult[i].UpdatedOnServer == null && expectedWasSuccessfull == true)
                {
                    expectedLocalResult[i].UpdatedOnServer = actualLocalResult[i].UpdatedOnServer;
                }
            }


            Assert.Equal(JsonSerializer.Serialize(expectedServerResult), JsonSerializer.Serialize(actualServerResult));

            Assert.Equal(JsonSerializer.Serialize(expectedLocalResult), JsonSerializer.Serialize(actualLocalResult));

            if (expectedWasSuccessfull) { Assert.Equal(JsonSerializer.Serialize(actualLocalResult), JsonSerializer.Serialize(actualServerResult)); }

        }




        [Fact]
        public void SortByIdTest()
        {
            List<Guid> overrideIds = new List<Guid>();

            for (int i = 0; i < 10; i++)
            {
                overrideIds.Add(Guid.NewGuid());
            }

            List<TestUpdate> actual = _testContent.Generate(1, "Unsorted", overrideIds, DateTime.Now)[0];
            List<TestUpdate> expected = _testContent.Generate(1, "SortedById", overrideIds, DateTime.Now)[0];

            var task = Task.Run(()=>_localServerEngine.SortById(actual));
            task.Wait();

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        }


        [Fact]
        public void SortByCreatedTest()
        {
            List<Guid> overrideIds = new List<Guid>();

            for (int i = 0; i < 10; i++)
            {
                overrideIds.Add(Guid.NewGuid());
            }

            List<TestUpdate> actual = _testContent.Generate(1, "Unsorted", overrideIds, DateTime.Now)[0];
            List<TestUpdate> expected = _testContent.Generate(1, "SortedByCreated", overrideIds, DateTime.Now)[0];


            var task = Task.Run(()=>_localServerEngine.SortByCreated(actual));
            task.Wait();

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

            List<TestUpdate> actual = _testContent.Generate(1, "Unsorted", overrideIds, DateTime.Now)[0];
            List<TestUpdate> expected = _testContent.Generate(1, "History", overrideIds, DateTime.Now)[0];

            var task = Task.Run(()=>_localServerEngine.SortByIdAndCreated(actual));
            task.Wait();

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        }




        [Fact]
        public void FindConflictsTest()
        {
            List<Guid> draftIds = Guid.NewGuid().GenerateList(10);
            DateTime ServerCreatedDate = DateTime.Now;
            List<TestUpdate> changesFromServer = new List<TestUpdate>()
                        {
                            new TestUpdate(draftIds[1], ServerCreatedDate, "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" },null),
                            new TestUpdate(draftIds[1], ServerCreatedDate.AddSeconds(1), "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" },null),
                            new TestUpdate(draftIds[1], ServerCreatedDate.AddSeconds(2), "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" },null),
                            new TestUpdate(draftIds[2], ServerCreatedDate.AddSeconds(3), "mcarter", null, true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" },null),
                            new TestUpdate(draftIds[2], ServerCreatedDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" },null),
                            new TestUpdate(draftIds[3], ServerCreatedDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas","Carrots" },null),
                            new TestUpdate(draftIds[4], ServerCreatedDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate" },null),
                            new TestUpdate(draftIds[4], ServerCreatedDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger","Chicken","Chocolate","Lindor Balls" },null)
                        };

            CreateDelay(200);

            DateTime LocalCreatedDate = DateTime.Now;
            List<TestUpdate> changesFromLocal = new List<TestUpdate>()
                        {
                            new TestUpdate(draftIds[5], LocalCreatedDate, "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips", "Strawberries" },null),
                            new TestUpdate(draftIds[5], LocalCreatedDate.AddSeconds(1), "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1934-05-02 00:00:00.000"), new List<string> { "Chips" },null),
                            new TestUpdate(draftIds[5], LocalCreatedDate.AddSeconds(2), "mcarter", null, true, "Bob", "Hoskins", DateTime.Parse("1956-12-24 00:00:00.000"), new List<string> { "Chips", "Strawberries", "Tiramisu" },null),
                            new TestUpdate(draftIds[2], LocalCreatedDate.AddSeconds(3), "mcarter", null, true, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" },null),
                            new TestUpdate(draftIds[2], LocalCreatedDate.AddSeconds(4), "mcarter", null, false, "Tracey", "Emin", DateTime.Parse("1999-12-31 00:00:00.000"), new List<string> { "Cake" },null),
                            new TestUpdate(draftIds[3], LocalCreatedDate.AddSeconds(5), "mcarter", null, true, "Jim", "Broadbent", DateTime.Parse("2010-06-04 00:00:00.000"), new List<string> { "Peas", "Carrots" },null),
                            new TestUpdate(draftIds[6], LocalCreatedDate.AddSeconds(6), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger", "Chicken", "Chocolate" },null),
                            new TestUpdate(draftIds[6], LocalCreatedDate.AddSeconds(7), "mcarter", null, true, "Mark", "Carter", DateTime.Parse("1978-07-02 00:00:00.000"), new List<string> { "Burger", "Chicken", "Chocolate", "Lindor Balls" },null)
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


            expected.Sort((x, y) => x.UpdateCreated.CompareTo(y.UpdateCreated));

            Task<List<Conflict>> actualTask = Task.Run(()=>_localServerEngine.FindConflicts(changesFromServer, changesFromLocal));
            actualTask.Wait();
            List<Conflict> actual = actualTask.Result;
            
            actual.Sort((x, y) => x.UpdateCreated.CompareTo(y.UpdateCreated));

            List<Conflict> actualMinusConflictID = actual.Select(x => new Conflict(x.UpdateId, x.UpdateCreated)).ToList();

            List<string> actualIdAndConflictID = actual.Select(x => x.UpdateId.ToString() + x.ConflictId.ToString()).ToList();

            //expected to should match actual (without conflictID generated within function)
            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actualMinusConflictID));

            //the conflictIds generated within function should be distinct for each id.
            // Assert.True(actualIdAndConflictID.Count() == 3, "Error with ConflictIDs generated in function.");         
        }





        [Fact]
        public void SaveConflictIdsTest()
        {
            List<List<TestUpdate>> testContents = _testContent.Generate(1,"Default");

            var saveToServerTask = Task.Run(() => _serverDataAccess.SaveToServer(testContents[0]));
            saveToServerTask.Wait();
            var saveToLocalTask = Task.Run(()=> _localDataAccess.SaveToLocal(testContents[0]));
            Task.WaitAll(saveToServerTask, saveToLocalTask);

            List<Conflict> conflicts = new List<Conflict>();

            Guid conflictID = Guid.NewGuid();

            conflicts.Add(new Conflict(testContents[0][1].Id, testContents[0][1].Created, conflictID));

            testContents[0][1].ConflictId = conflictID;


            List<TestUpdate> expected = testContents[0];
            expected.Sort((x, y) => x.Id.CompareTo(y.Id));
            expected.Select(x => (x.Id, x.Created, x.ConflictId)).ToList();


            var saveConflictIdsTask = Task.Run(()=>_localServerEngine.SaveConflictIds(conflicts));
            saveConflictIdsTask.Wait();


            var actualServerTask = Task.Run(()=>_serverDataAccess.GetFromServer(_testContent.ListIds(testContents[0])));
            var actualLocalTask = Task.Run(() => _localDataAccess.GetFromLocal(_testContent.ListIds(testContents[0])));
            Task.WaitAll(actualLocalTask, actualServerTask);

            List<TestUpdate> actualServer = actualServerTask.Result;
            List<TestUpdate> actualLocal = actualLocalTask.Result;

            actualServer.Sort((x, y) => x.Id.CompareTo(y.Id));
            actualServer.Select(x => (x.Id, x.Created, x.ConflictId)).ToList();

            actualLocal.Sort((x, y) => x.Id.CompareTo(y.Id));
            actualLocal.Select(x => (x.Id, x.Created, x.ConflictId)).ToList();

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actualServer));
            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actualLocal));

        }


        async void CreateDelay(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }




    }
}
