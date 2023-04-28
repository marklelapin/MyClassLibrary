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

namespace MyClassLibrary.Tests.LocalServerMethods
{
    public class ServerDataAccessTests 
    {

        ServiceConfiguration dataService = new ServiceConfiguration();



        private static readonly List<TestContent> SaveAndGetTestContent = new List<TestContent>().GenerateTestContents(3);

        //[Theory, MemberData(nameof(TestData))]
        public static readonly object[][] SaveAndGetTestData =
        {
            new object[] {
                            SaveAndGetTestContent[0].TestObjects
                            ,SaveAndGetTestContent[0].TestIds()
                            ,SaveAndGetTestContent[0].TestObjects
                          },
            new object[] {
                            SaveAndGetTestContent[1].TestObjects
                            ,SaveAndGetTestContent[1].TestIds()
                            ,SaveAndGetTestContent[1].TestObjects
                          },
            new object[] {
                            SaveAndGetTestContent[2].TestObjects
                            ,new List<Guid>() { SaveAndGetTestContent[2].TestIds()[2] }
                            ,SaveAndGetTestContent[2].TestObjects.Where(x=>x.Id == SaveAndGetTestContent[2].TestIds()[2]).ToList()
                          },
        };
        [Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<TestObject> testObjects,List<Guid> ids,List<TestObject> expected)
        {
            dataService.serverDataAccess.SaveToServer(testObjects);
           
            List<TestObject> actual = dataService.serverDataAccess.GetFromServer<TestObject>(ids);

            expected.Sort((x,y)=>x.Id.CompareTo(y.Id));
            actual.Sort((x,y)=>x.Id.CompareTo((y.Id)));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }






        public static readonly List<TestContent> GetChangesTestContent = new List<TestContent>().GenerateTestContents(3);
           
        public static readonly object[][] GetChangesTestData =
        {
            new object[] { GetChangesTestContent[0].TestObjects,+1,new List<TestObject>()}
           ,new object[] { GetChangesTestContent[1].TestObjects,0,new List<TestObject>()}
           ,new object[] { GetChangesTestContent[2].TestObjects,-1,GetChangesTestContent[2].TestObjects}
        };

        [Theory, MemberData(nameof(GetChangesTestData))]
        async public void GetChangesTest(List<TestObject> testObjects,int lastSyncDateAdjustment, List<TestObject> expected)
        {
            await Task.Delay(2000); //waits for 2 second to ensure that the last sync date produced will be more than the 1 second potential test gap.
            
            DateTime lastSyncDate = dataService.serverDataAccess.SaveToServer(testObjects);

            (List<TestObject> actualChangesFromServer,DateTime actualLastUpdatedOnServer) = dataService.serverDataAccess.GetChangesFromServer<TestObject>(lastSyncDate.AddSeconds(lastSyncDateAdjustment));

            expected.Sort((x , y)=>x.Id.CompareTo(y.Id));
            actualChangesFromServer.Sort((x, y) => x.Id.CompareTo(y.Id));
            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actualChangesFromServer));
      }


        public static readonly List<TestContent> saveConflictIDTestContents = new List<TestContent>().GenerateTestContents(2);

        public static readonly object[][] saveConflictIDTestDate =
        {
            new object[]{saveConflictIDTestContents[0],true},
            new object[]{saveConflictIDTestContents[1],false}
        };

        [Theory, MemberData(nameof(saveConflictIDTestDate))]
        public void SaveConflictIDToServerTest(TestContent testContent,bool conflictsExist)
        {
           
            
            List<TestObject> conflictedObjects = new List<TestObject>();
            conflictedObjects.Add(testContent.TestObjects[1]);
            conflictedObjects.Add(testContent.TestObjects[5]);
            
            Dictionary<Guid,Guid> conflictDictionary = new Dictionary<Guid, Guid>();

            conflictDictionary.Add(conflictedObjects[0].Id, Guid.NewGuid());
            conflictDictionary.Add(conflictedObjects[1].Id, Guid.NewGuid());

             List<Conflict> conflicts = new List<Conflict>();
            if (conflictsExist)
            {
                conflicts.Add(new Conflict(conflictedObjects[0].Id, conflictedObjects[0].Created,conflictDictionary.GetValueOrDefault(conflictedObjects[0].Id)));
                conflicts.Add(new Conflict(conflictedObjects[1].Id, conflictedObjects[1].Created,conflictDictionary.GetValueOrDefault(conflictedObjects[1].Id)));
            }

            dataService.serverDataAccess.SaveToServer(testContent.TestObjects);
            dataService.serverDataAccess.SaveConflictIdsToServer<TestObject>(conflicts);

            List<Conflict> actual = new List<Conflict>();

            actual = dataService.serverDataAccess.GetFromServer<TestObject>(testContent.TestIds())
                                                    .Where(x=>x.ConflictId != null)
                                                    .Select(x=> new Conflict(x.Id,x.Created,x.ConflictId))
                                                    .ToList();
            
            List<Conflict> expected = new List<Conflict>();

            if (conflictsExist)
            {
                    foreach (TestObject obj in conflictedObjects)
                                {
                                    expected.Add(new Conflict(obj.Id, obj.Created, conflictDictionary.GetValueOrDefault(obj.Id)));
                                }
            }
            

            actual.Sort((x, y) => x.ObjectCreated.CompareTo(y.ObjectCreated));
            expected.Sort((x, y) => x.ObjectCreated.CompareTo(y.ObjectCreated));

            Assert.Equal(JsonSerializer.Serialize(expected),JsonSerializer.Serialize(actual));      
 
        }





    }
}
