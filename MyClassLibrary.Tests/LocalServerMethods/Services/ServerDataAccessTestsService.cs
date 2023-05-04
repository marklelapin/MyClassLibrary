using Microsoft.Extensions.Hosting;
using MyClassLibrary.Extensions;
using MyClassLibrary.LocalServerMethods;
using MyExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using Xunit.Sdk;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using Newtonsoft.Json;

namespace MyClassLibrary.Tests.LocalServerMethods.Services

{
    public class ServerDataAccessTestsService<T> : IServerDataAccessTests<T> where T : LocalServerIdentityUpdate
    {
        private ITestContent<T> _testContent;
        private IServiceConfiguration _serviceConfiguration;
        private readonly IServerDataAccess _serverDataAccess;

        public ServerDataAccessTestsService(IServiceConfiguration serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;
            _serverDataAccess = _serviceConfiguration.ServerDataAccess();
            _testContent = _serviceConfiguration.TestContent<T>();
        }


        public object[][] SaveTestData() { return new object[][] { new object[] { _testContent.Generate(1, "Default")[0] } }; }
        //[Theory, MemberData(nameof(SaveTestData))]
        public void SaveTest(List<T> testUpdates)
        {
            _serverDataAccess.SaveToServer<T>(testUpdates);
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
            _serverDataAccess.SaveToServer(updates);

            List<T> actual = _serverDataAccess.GetFromServer<T>(getIds);

            expected.Sort((x, y) => x.Id.CompareTo(y.Id));
            actual.Sort((x, y) => x.Id.CompareTo(y.Id));

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
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
            await Task.Delay(4000); //waits for 2 second to ensure that the last sync date produced will be more than the 1 second potential test gap.

            DateTime lastSyncDate = _serverDataAccess.SaveToServer(updates);

            (List<T> actualChangesFromServer, DateTime actualLastUpdatedOnServer) = _serverDataAccess.GetChangesFromServer<T>(lastSyncDate.AddSeconds(lastSyncDateAdjustment));

            expected.Sort((x, y) => x.Id.CompareTo(y.Id));
            actualChangesFromServer.Sort((x, y) => x.Id.CompareTo(y.Id));
            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actualChangesFromServer));
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

            _serverDataAccess.SaveToServer<T>(updates);
            _serverDataAccess.SaveConflictIdsToServer<T>(conflicts);

            List<Conflict> actual = new List<Conflict>();

            actual = _serverDataAccess.GetFromServer<T>(_testContent.ListIds(updates))
                                                    .Where(x => x.ConflictId != null)
                                                    .Select(x => new Conflict(x.Id, x.Created, x.ConflictId))
                                                    .ToList();

            actual.Sort((x, y) => x.UpdateCreated.CompareTo(y.UpdateCreated));
            expected.Sort((x, y) => x.UpdateCreated.CompareTo(y.UpdateCreated));

            Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));

        }


        //public object[][] DeleteTestData() { return new object[][] { new object[] { _testContent.Generate(1, "Default")[0] } }; }
        ////[Theory, MemberData(nameof(DeleteTestData))]
        //public void DeleteTest(List<T> testUpdatesToDelete)
        //{
        //    throw new NotImplementedException(testUpdatesToDelete.ToString());
        //}
    }
}
