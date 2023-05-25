﻿
using System.Data;

using System.Text.Json;

using MyClassLibrary.Tests.LocalServerMethods.Interfaces;

using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.LocalServerMethods.Interfaces;

namespace MyClassLibrary.Tests.LocalServerMethods.Services

{
    public class ServerDataAccessTestsService<T> : IServerDataAccessTests<T> where T : LocalServerModelUpdate
    {
        private ITestContent<T> _testContent;
        private IServiceConfiguration<T> _serviceConfiguration;
        private readonly IServerDataAccess<T> _serverDataAccess;

        public ServerDataAccessTestsService(IServiceConfiguration<T> serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;
            _serverDataAccess = _serviceConfiguration.ServerDataAccess();
            _testContent = _serviceConfiguration.TestContent();
        }


        public object[][] SaveTestData() { return new object[][] { new object[] { _testContent.Generate(1, "Default")[0] } }; }
        //[Theory, MemberData(nameof(SaveTestData))]
        public void SaveTest(List<T> testUpdates)
        {

            var saveToServerTask = Task.Run(() => _serverDataAccess.SaveToServer(testUpdates));
           saveToServerTask.Wait();
           DateTime updatedOnServer = saveToServerTask.Result;

           bool checkUpdatedOnServerDate = true;

           foreach (var testUpdate in testUpdates) { 
            
                if (testUpdate.UpdatedOnServer != updatedOnServer) { checkUpdatedOnServerDate = false; };
            
            }

           Assert.True(checkUpdatedOnServerDate);

        }



      
        public object[][] SaveAndGetTestData()
        {
            List<List<T>> saveAndGetTestContent = _testContent.Generate(3, "Default");

            return new object[][] {
                        new object[] {
                                        saveAndGetTestContent[0]
                                        ,_testContent.ListIds(saveAndGetTestContent[0])
                                        ,saveAndGetTestContent[0]
                                      },
                        new object[] {
                                        saveAndGetTestContent[1]
                                        ,_testContent.ListIds(saveAndGetTestContent[1])
                                        ,saveAndGetTestContent[1]
                                      },
                        new object[] {
                                        saveAndGetTestContent[2]
                                        ,new List<Guid> {_testContent.ListIds(saveAndGetTestContent[2])[2] }
                                        ,saveAndGetTestContent[2].Where(x=>x.Id == _testContent.ListIds(saveAndGetTestContent[2])[2]).ToList()
                                      },
            };

        }
        //[Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<T> updates, List<Guid> getIds, List<T> expected)
        {
            Task.Run(()=>_serverDataAccess.SaveToServer(updates)).Wait();

            var actualTask = Task.Run(() => _serverDataAccess.GetFromServer(getIds));
            actualTask.Wait();
            List<T> actual = actualTask.Result;

            expected.Sort((x, y) => x.Id.CompareTo(y.Id));
            actual.Sort((x, y) => x.Id.CompareTo(y.Id));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }





        public object[][] GetChangesTestData()
        {
            List<List<T>> GetChangesTestContent = _testContent.Generate(3, "Default");

            return new object[][]
            {
                new object[] { GetChangesTestContent[0],+1,new List<T>()}
                ,new object[] { GetChangesTestContent[1],0,new List<T>()}
                ,new object[] { GetChangesTestContent[2],-1,GetChangesTestContent[2]}
            };
        }
        //[Theory, MemberData(nameof(GetChangesTestData))]
        async public void GetChangesTest(List<T> updates, int lastSyncDateAdjustment, List<T> expected)
        {
            Task.Run(()=>Task.Delay(5*1000)).Wait(); //waits for 5 second to ensure that other tests don't get in the way.




            var lastSyncDateTask = Task.Run(()=>_serverDataAccess.SaveToServer(updates));
            lastSyncDateTask.Wait();
            DateTime lastSyncDate = lastSyncDateTask.Result;

            var getChangesTask = Task.Run(() => _serverDataAccess.GetChangesFromServer(lastSyncDate.AddSeconds(lastSyncDateAdjustment)));
            getChangesTask.Wait();
            (List<T> actualChangesFromServer, DateTime actualLastUpdatedOnServer) = getChangesTask.Result;

            expected.Sort((x, y) => x.Id.CompareTo(y.Id));
            actualChangesFromServer.Sort((x, y) => x.Id.CompareTo(y.Id));
            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actualChangesFromServer));
            if (actualChangesFromServer.Count > 0)
            {
                Assert.Equal(lastSyncDate, actualLastUpdatedOnServer);
            }
        }




        private List<Conflict> Conflicts(List<List<T>> saveConflictIDTestContents)
        {
            List<Conflict> output = new List<Conflict>();

            List<T> conflictedObjects = new List<T>();
            conflictedObjects.Add(saveConflictIDTestContents[0][1]);
            conflictedObjects.Add(saveConflictIDTestContents[0][5]);


            output.Add(new Conflict(conflictedObjects[0].Id, conflictedObjects[0].Created, Guid.NewGuid()));
            output.Add(new Conflict(conflictedObjects[1].Id, conflictedObjects[1].Created, Guid.NewGuid()));

            return output;
        }
        public object[][] SaveConflictIdTestData()
        {
            List<List<T>> saveConflictIDTestContents = _testContent.Generate(2, "Default");
            List<Conflict> conflicts = Conflicts(saveConflictIDTestContents);

            return new object[][] {
            new object[]{saveConflictIDTestContents[0],conflicts,conflicts},
            new object[]{saveConflictIDTestContents[1],new List<Conflict>(),new List<Conflict>()}
            };
        }
        //[Theory, MemberData(nameof(saveConflictIDTestData))]
        public void SaveConflictIdTest(List<T> updates, List<Conflict> conflicts,List<Conflict> expected)
        {

           Task.Run(()=> _serverDataAccess.SaveToServer(updates)).Wait();
           Task.Run(()=> _serverDataAccess.SaveConflictIdsToServer(conflicts)).Wait();

            List<Conflict> actual = new List<Conflict>();

            var actualTask = Task.Run(() => _serverDataAccess.GetFromServer(_testContent.ListIds(updates)));
            actual = actualTask.Result.Where(x => x.ConflictId != null)
                                    .Select(x => new Conflict(x.Id, x.Created, x.ConflictId))
                                    .ToList();

            actual.Sort((x, y) => x.UpdateCreated.CompareTo(y.UpdateCreated));
            expected.Sort((x, y) => x.UpdateCreated.CompareTo(y.UpdateCreated));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        }


        //public object[][] DeleteTestData() { return new object[][] { new object[] { _testContent.Generate(1, "Default")[0] } }; }
        ////[Theory, MemberData(nameof(DeleteTestData))]
        //public void DeleteTest(List<T> testUpdatesToDelete)
        //{
        //    throw new NotImplementedException(testUpdatesToDelete.ToString());
        //}
    }
}
