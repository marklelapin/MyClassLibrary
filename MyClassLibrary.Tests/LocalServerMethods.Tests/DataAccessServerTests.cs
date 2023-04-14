using MyClassLibrary.Extensions;
using MyClassLibrary.LocalServerMethods;
using MyExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class DataAccessServerTests
    {
        private static readonly ConnectionStringDictionary connectionStringDictionary = new ConnectionStringDictionary();
        

        private IServerDataAccess _serverDataAccess = new ServerSQLConnector(connectionStringDictionary.ServerSQL);

        private static readonly List<TestContent> SaveAndGetTestContent = new List<TestContent>().GenerateTestContents(3);

        //[Theory, MemberData(nameof(TestData))]
        public static readonly object[][] SaveAndGetTestData =
        {
            new object[] {
                            SaveAndGetTestContent[0].TestObjects
                            ,SaveAndGetTestContent[0].TestIds()
                            ,true
                            ,SaveAndGetTestContent[0].ActiveTestObjects()
                          },
            new object[] {
                            SaveAndGetTestContent[1].TestObjects
                            ,SaveAndGetTestContent[1].TestIds()
                            ,false
                            ,SaveAndGetTestContent[1].TestObjects
                          },
            new object[] {
                            SaveAndGetTestContent[2].TestObjects
                            ,new List<Guid>() { SaveAndGetTestContent[2].TestIds()[2] }
                            ,true
                            ,SaveAndGetTestContent[2].TestObjects.Where(x=>x.Id == SaveAndGetTestContent[2].TestIds()[2]).ToList()
                          },
        };
        [Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<TestObject> testObjects,List<Guid> ids,bool isActive,List<TestObject> expected)
        {
            _serverDataAccess.SaveToServer(testObjects);
           
            List<TestObject> actual = _serverDataAccess.GetFromServer<TestObject>(ids,isActive);

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }

        public static readonly List<TestContent> GetChangesTestContent = new List<TestContent>().GenerateTestContents(3);
           
        //public static readonly object[][] GetChangesTestData =
        //{
        //    new object[] { GetChangesTestContent[0].TestObjects,+1000,new List<TestObject>()}
        //   ,new object[] { GetChangesTestContent[1].TestObjects,0,new List<TestObject>()}
        //   ,new object[] { GetChangesTestContent[2].TestObjects,-1000,GetChangesTestContent[2].TestObjects}
        //};

        //[Theory, MemberData(nameof(GetChangesTestData))]
        //async public void GetChangesTest(List<TestObject> testObjects,int lastSyncDateAdjustment, List<TestObject> expected)
        //{
        //    _serverDataAccess.SaveToServer<TestObject>(testObjects);

        //    DateTime lastSyncDate = testObjects[0].UpdatedOnServer.AddMilliseconds(lastSyncDateAdjustment);

        //    List<TestObject> actual = _serverDataAccess.GetChangesFromServer<TestObject>(lastSyncDate);

        //    expected.SortById<TestObject>();
        //    actual.SortById<TestObject>();
        //    Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        //    await Task.Delay(2000); //waits for 1 second so that there is a 

        //}









    }
}
