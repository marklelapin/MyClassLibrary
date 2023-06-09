

using Microsoft.AspNetCore.Mvc;
using MyClassLibrary.DataAccessMethods;
using MyClassLibrary.LocalServerMethods.Extensions;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using System.Text.Json;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests.DataAccess
{
    public class LocalServerEngineTests : ILocalServerEngineTests
    {
        private readonly ILocalServerEngine<TestUpdate> _localServerEngine;
        private readonly ILocalServerEngine<TestUpdate> _localServerEngine_FailedServer;
        private readonly ILocalServerEngine<TestUpdate> _localServerEngine_FailedLocal;
        private readonly ILocalDataAccess<TestUpdate> _localDataAccess;
        private readonly IServerDataAccess<TestUpdate> _serverDataAccess;

        public LocalServerEngineTests(ILocalServerEngine<TestUpdate> localServerEngine
                                        , ILocalDataAccess<TestUpdate> localDataAccess
                                        , IServerDataAccess<TestUpdate> serverDataAccess
                                        , ISqlDataAccess sqlDataAccess
                                        , ILocalServerEngine<TestUpdate> localServerEngine_FailedServer
                                        , ILocalServerEngine<TestUpdate> localServerEngine_FailedLocal)
        {
            _serverDataAccess = serverDataAccess;
            _localDataAccess = localDataAccess;
            _localServerEngine = localServerEngine;
            _localServerEngine_FailedLocal = localServerEngine_FailedLocal;
            _localServerEngine_FailedServer = localServerEngine_FailedServer;

            //Add failures
            var failedLocalDataAccess = new LocalSQLConnector<TestUpdate>(sqlDataAccess, "Error");
            _localServerEngine_FailedLocal.ChangeLocalDataAccess(failedLocalDataAccess);

            var failedServerDataAccess = new ServerSQLConnector<TestUpdate>(sqlDataAccess, "Error");
            _localServerEngine_FailedServer.ChangeServerDataAccess(failedServerDataAccess);




        }


        [Fact]
        public async Task GetAllUpdatesTest()
        {
            List<TestUpdate> actualNull = await _localServerEngine.GetAllUpdates();

            List<TestUpdate> actualId = await _localServerEngine.GetAllUpdates(new Guid("b01df9cc-4af9-7d5b-57e4-3ec0ec922e8b"));

            List<TestUpdate> actualIds = await _localServerEngine.GetAllUpdates(new List<Guid> { new Guid("b01df9cc-4af9-7d5b-57e4-3ec0ec922e8b"), new Guid("db516527-fcc3-6da6-b090-fb5ff747c7c2") });

            Assert.True(actualNull.Count > 0, "Failed to return updates when passed null.");
            Assert.True(actualId.Count > 0, " Failed to return updates when passed Id.");
            Assert.True(actualIds.Count > 0, "Faile to return updates when passed Ids.");
        }

        [Fact]
        public async Task GetConflictedUpdatesTest()
        {
            List<TestUpdate> actual = await _localServerEngine.GetConflictedUpdates(TestContent.ConflictedTestId);

            Assert.True(actual.Count > 0, "Failed to return known conflicted updates.");
        }

        [Fact]
        public async Task GetLatestUpdatesTest()
        {
            List<TestUpdate> actualNull = await _localServerEngine.GetLatestUpdates();

            List<TestUpdate> actualId = await _localServerEngine.GetLatestUpdates(new Guid("b01df9cc-4af9-7d5b-57e4-3ec0ec922e8b"));

            List<TestUpdate> actualIds = await _localServerEngine.GetLatestUpdates(new List<Guid> { new Guid("b01df9cc-4af9-7d5b-57e4-3ec0ec922e8b"), new Guid("db516527-fcc3-6da6-b090-fb5ff747c7c2") });

            Assert.True(actualNull.Count > 0, "Failed to return updates when passed null.");
            Assert.True(actualId.Count > 0, " Failed to return updates when passed Id.");
            Assert.True(actualIds.Count > 0, "Failed to return updates when passed Ids.");
        }

        [Fact]
        public async Task SaveUpdatesTest()
        {
            //Setup
            List<TestUpdate> testUpdates = TestContent.GetNewUpdates();

            //Test
            await _localServerEngine.SaveUpdates(testUpdates);

            //Get Result from Local
            List<TestUpdate> actual = await _localServerEngine.GetAllUpdates(testUpdates.Select(x => x.Id).ToList());

            //Assert
            Assert.True(actual.Count > 0, "Nothing saved.");

        }


        [Fact]
        public async Task TrySyncTest()
        {
            //Setup
            List<TestUpdate> testUpdates = TestContent.GetNewUpdatesWithoutUpdatedOnServerDates();

            //Test
            await _localDataAccess.SaveUpdatesToLocal(testUpdates);
            (DateTime? lastSynced, bool syncSuccess) = await _localServerEngine.TrySync();


            //Get result from local
            List<TestUpdate> local = await _localDataAccess.GetUpdatesFromLocal(testUpdates.Select(x => x.Id).ToList(), false);
            local = local.SortByCreated();
            DateTime actualLastSynced = await _localDataAccess.GetLocalLastSyncDate();
            //Get result from server
            List<TestUpdate> server = await _serverDataAccess.GetUpdatesFromServer(testUpdates.Select(x => x.Id).ToList(), false);
            server = server.SortByCreated();

            //Assert
            Assert.True(syncSuccess, "TrySync failed to report a successful sync.");
            Assert.True(local.Count > 0, "Nothing saved to Local");
            Assert.True(server.Count > 0, "Nothing saved to Server");
            Assert.True(JsonSerializer.Serialize(local) == JsonSerializer.Serialize(server), "Local and Server Records do not Match");
            Assert.True(actualLastSynced == lastSynced, "Last Synced Date not saved properly.");
        }


        [Fact]
        public async Task TrySyncServerConnectionStringFailureTest()
        {
            //Setup
            List<TestUpdate> testUpdates = TestContent.GetNewUpdates();

            //Test
            (DateTime? lastSynced, bool syncSuccess) = await _localServerEngine_FailedServer.TrySync();

            //Assert
            Assert.True(true, "Sync Errored");
            Assert.True(syncSuccess = false, "TrySync reported success with FailedServer");
            //TODO ADD Assert to check that error handling caught this correctly.
        }

        [Fact]
        public async Task TrySyncServerGeneralFailureTest()
        {
            //Setup
            List<TestUpdate> testUpdate = TestContent.GetNewUpdatesToError();

            try
            {
                //Test
                await _localServerEngine.TrySync();
            }
            catch
            {
                Assert.True(true);
            }

            Assert.True(false, "No error thrown when duplicate updates passed in.");

        }

        [Fact]
        public async Task TrySyncLocalFailureTest()
        {
            //Setup
            List<TestUpdate> testUpdate = TestContent.GetNewUpdates();

            try
            {
                //Test
                (DateTime? lastSynced, bool syncSuccess) = await _localServerEngine_FailedLocal.TrySync();
            }
            catch
            {
                Assert.True(true);
            }

            //Assert
            Assert.True(false, "Local Server failed by no error message.");
        }

        [Fact]
        public async Task ClearConflictedIdsTest()
        {
            //Setup
            List<TestUpdate> testUpdate = TestContent.GetNewUpdatesWithConflicts();

            List<Guid> testIds = testUpdate.Select(x => x.Id).ToList();

            //Test
            await _localServerEngine.ClearConflictIds(testIds);

            //Get Results from local and server
            List<TestUpdate> local = await _localDataAccess.GetUpdatesFromLocal(testIds);
            List<TestUpdate> server = await _serverDataAccess.GetUpdatesFromServer(testIds);

            //Assert
            Assert.True(local.Where(x => x.IsConflicted = true).Count() == 0, "Not all conflicts removed from local.");
            Assert.True(server.Where(x => x.IsConflicted = true).Count() == 0, "Not all conflictes removed from server.");

        }

        [Fact]
        public async Task TrySyncWithConflictsTest()
        {
            //Setup
            (List<TestUpdate> localUpdate, List<TestUpdate> serverUpdate) = TestContent.GetNewServerAndLocalUpdatesThatConflict();
            List<Guid> localIds = localUpdate.Select(x => x.Id).ToList();
            List<Guid> serverIds = localUpdate.Select(x => x.Id).ToList();
            List<Guid> combinedIds = localIds.Union(serverIds).ToList();

            await _localDataAccess.SaveUpdatesToLocal(localUpdate); // save of update to local
            List<ServerToLocalPostBack> serverPostBack = await _serverDataAccess.SaveUpdatesToServer(serverUpdate, TestContent.CopyId2); //save to the server from a different local
            List<TestUpdate> expectedFromLocalUpdate = localUpdate;
            List<TestUpdate> expectedFromServerUpdate = serverUpdate;
            expectedFromServerUpdate.ForEach(update =>
            {
                update.UpdatedOnServer = serverPostBack.First().UpdatedOnServer;
            });

            List<TestUpdate> expected = expectedFromLocalUpdate.Union(expectedFromServerUpdate).ToList();
            expected.ForEach(update =>
            {
                update.IsConflicted = true;
            });

            List<TestUpdate> setupLocalActual = await _localDataAccess.GetUpdatesFromLocal(localIds);
            List<TestUpdate> setupServerActual = await _serverDataAccess.GetUpdatesFromServer(serverIds);


            //Check Setup
            Assert.True(setupLocalActual.Count() > 0, "Test Setup failed - nothing saved to local.");
            Assert.True(setupServerActual.Count() > 0, "Test Setup failed = nothing saved to server.");



            //Test
            await _localServerEngine.TrySync();



            //Get Results from Server
            List<TestUpdate> actualServer = await _serverDataAccess.GetUpdatesFromServer(combinedIds);
            DateTime updateOnServer = actualServer.Where(x => localIds.Contains(x.Id)).Select(x => (DateTime)x.UpdatedOnServer!).FirstOrDefault();
            expected.ForEach(update =>
            {
                if (update.UpdatedOnServer == null) { update.UpdatedOnServer = updateOnServer; };  //add in updated onserver to remaining expected values as now known.
            });

            //Get Results from Local
            List<TestUpdate> actualLocal = await _localDataAccess.GetUpdatesFromLocal(combinedIds);

            Assert.True(actualServer.Where(x => x.IsConflicted = true).Count() == actualServer.Count(), "Not all updates have been marked as isConflicted");
            Assert.True(actualServer.Where(x => localIds.Contains(x.Id)).Count() > 0, "Not all LocalUpdates saved to Server");
            Assert.True(actualLocal.Where(x => serverIds.Contains(x.Id)).Count() > 0, "Not all ServerUpdates saved to Local");

            Assert.Equal(JsonSerializer.Serialize(actualLocal), JsonSerializer.Serialize(actualServer));

        }
    }

}

