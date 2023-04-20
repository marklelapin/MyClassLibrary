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

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class DataAccessServerTests
    {

        DataService dataService = new DataService();

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




        [Fact]
        public void SaveConflictIDToServerTest()
        {
            throw new NotImplementedException();
        }





    }
}
