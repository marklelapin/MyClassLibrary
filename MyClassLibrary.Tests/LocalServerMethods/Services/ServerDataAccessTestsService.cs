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

namespace MyClassLibrary.Tests.LocalServerMethods.Services

{
    public class ServerDataAccessTestsService<T> : IServerDataAccessTests<T> where T : LocalServerIdentityUpdate
    {

        private static IServiceConfiguration _serviceConfiguration;

        public static IServiceConfiguration ServiceConfiguration
        {
            get { return _serviceConfiguration; }
            set { _serviceConfiguration = value; }
        }

        public ServerDataAccessTestsService(IServiceConfiguration? serviceConfiguration = null)
        {
            _serviceConfiguration = serviceConfiguration ?? throw new ArgumentNullException("No Services Configuration passed through for Testing.");
        }

        private static IServerDataAccess serverDataAccess = ServiceConfiguration.ServerDataAccess();

        private static ITestContent<T> testContent = ServiceConfiguration.TestContent<T>();


        public static readonly object[][] SaveTestUpdates = { new object[] { testContent.Generate(1, "Default")[0] } };
        [Theory, MemberData(nameof(SaveTestUpdates))]
        public void SaveTest(List<T> testUpdates)
        {
           serverDataAccess.SaveToServer<T>(testUpdates);
        }



        private static readonly List<List<T>> SaveAndGetTestContent = testContent.Generate(3, "Default");

        public static readonly object[][] SaveAndGetTestData =
        {
            new object[] {
                            SaveAndGetTestContent[0]
                            ,testContent.ListIds(SaveAndGetTestContent[0])
                            ,SaveAndGetTestContent[0]
                          },
            new object[] {
                            SaveAndGetTestContent[1]
                            ,testContent.ListIds(SaveAndGetTestContent[1])
                            ,SaveAndGetTestContent[1]
                          },
            new object[] {
                            SaveAndGetTestContent[2]
                            ,testContent.ListIds(SaveAndGetTestContent[2])[2]
                            ,SaveAndGetTestContent[2].Where(x=>x.Id == testContent.ListIds(SaveAndGetTestContent[2])[2]).ToList()
                          },
        };
        [Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<T> updates, List<Guid> getIds, List<T> expected)
        {
            serverDataAccess.SaveToServer(updates);

            List<T> actual = serverDataAccess.GetFromServer<T>(getIds);

            expected.Sort((x, y) => x.Id.CompareTo(y.Id));
            actual.Sort((x, y) => x.Id.CompareTo(y.Id));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }




        public static readonly List<List<T>> GetChangesTestContent = testContent.Generate(3, "Default");

        public static readonly object[][] GetChangesTestData =
        {
            new object[] { GetChangesTestContent[0],+1,new List<T>()}
           ,new object[] { GetChangesTestContent[1],0,new List<T>()}
           ,new object[] { GetChangesTestContent[2],-1,GetChangesTestContent[2]}
        };

        [Theory, MemberData(nameof(GetChangesTestData))]
        async public void GetChangesTest(List<T> updates, int lastSyncDateAdjustment, List<T> expected)
        {
            await Task.Delay(2000); //waits for 2 second to ensure that the last sync date produced will be more than the 1 second potential test gap.

            DateTime lastSyncDate = serverDataAccess.SaveToServer(updates);

            (List<TestObject> actualChangesFromServer, DateTime actualLastUpdatedOnServer) = serverDataAccess.GetChangesFromServer<TestObject>(lastSyncDate.AddSeconds(lastSyncDateAdjustment));

            expected.Sort((x, y) => x.Id.CompareTo(y.Id));
            actualChangesFromServer.Sort((x, y) => x.Id.CompareTo(y.Id));
            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actualChangesFromServer));
            Assert.Equal(lastSyncDate, actualLastUpdatedOnServer);
        }



        public static readonly List<List<T>> saveConflictIDTestContents = testContent.Generate(2, "Default");

        public static List<Conflict> conflicts
        {
            get
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


        public static readonly object[][] saveConflictIDTestDate =
        {
            new object[]{saveConflictIDTestContents[0],conflicts,conflicts},
            new object[]{saveConflictIDTestContents[1],new List<Conflict>(),new List<Conflict>()}
        };

        [Theory, MemberData(nameof(saveConflictIDTestDate))]
        public void SaveConflictIdTest(List<T> updates, List<Conflict> conflicts,List<Conflict> expected)
        {

            serverDataAccess.SaveToServer<T>(updates);
            serverDataAccess.SaveConflictIdsToServer<T>(conflicts);

            List<Conflict> actual = new List<Conflict>();

            actual = serverDataAccess.GetFromServer<T>(testContent.ListIds(updates))
                                                    .Where(x => x.ConflictId != null)
                                                    .Select(x => new Conflict(x.Id, x.Created, x.ConflictId))
                                                    .ToList();

            actual.Sort((x, y) => x.ObjectCreated.CompareTo(y.ObjectCreated));
            expected.Sort((x, y) => x.ObjectCreated.CompareTo(y.ObjectCreated));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        }


        public static readonly object[][] DeleteUpdates = { new object[] { testContent.Generate(1, "Default")[0] } };
        [Theory, MemberData(nameof(DeleteUpdates))]
        public void DeleteTest(List<T> testUpdatesToDelete)
        {
            throw new NotImplementedException();
        }
    }
}
