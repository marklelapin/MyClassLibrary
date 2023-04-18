using MyClassLibrary.LocalServerMethods;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class DataAccessLocalTests
    {

        DataService dataService = new DataService();



        private static readonly List<TestContent> SaveAndGetTestContents = new List<TestContent>().GenerateTestContents(3);

        public static readonly object[][] SaveAndGetTestData =
        {
            new object[] {
                            SaveAndGetTestContents[0].TestObjects
                            ,SaveAndGetTestContents[0].TestIds()
                            ,true
                            ,SaveAndGetTestContents[0].ActiveTestObjects()
                           },
            new object[]
                        {
                            SaveAndGetTestContents[1].TestObjects
                            ,SaveAndGetTestContents[1].TestIds()
                            ,false
                            ,SaveAndGetTestContents[1].TestObjects
                           },
            new object[]
                        {
                            SaveAndGetTestContents[2].TestObjects
                            ,new List<Guid> {SaveAndGetTestContents[2].TestIds()[2]}
                            ,true
                            ,SaveAndGetTestContents[2].TestObjects.Where(x => x.Id == SaveAndGetTestContents[2].TestIds()[2]).ToList()
                           }
        };
        [Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<TestObject> testObjects,List<Guid> testIds,bool isActive,List<TestObject> expected)
        {
            dataService.localDataAccess.SaveToLocal(testObjects);

           List<TestObject> actual =  dataService.localDataAccess.GetFromLocal<TestObject>(testIds,isActive);

            actual.Sort((x,y)=>x.Id.CompareTo(y.Id));
            expected.Sort((x, y) => x.Id.CompareTo(y.Id));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }

        

        [Fact]
         public void GetChangesFromLocalTest() //also tests functionality of passing null into GetFromLocal
        {
         
            TestContent testContent = new List<TestContent>().GenerateTestContents(1)[0];

            dataService.localDataAccess.SaveToLocal(testContent.TestObjects);

            List<TestObject> allTestObjects = dataService.localDataAccess.GetFromLocal<TestObject>(null,false);

            List<TestObject> expected = allTestObjects.Where(x=>x.UpdatedOnServer == null).ToList();

            List<TestObject> actual = dataService.localDataAccess.GetChangesFromLocal<TestObject>();

            actual.Sort((x, y) => x.Id.CompareTo(y.Id));
            expected.Sort((x, y) => x.Id.CompareTo(y.Id));

            Assert.Equal(JsonSerializer.Serialize(expected),JsonSerializer.Serialize(actual));
        }

        [Fact]
        public void SaveAndGetLocalLastSyncDate()
        {
            DateTime expected = DateTime.Now;
            dataService.localDataAccess.SaveLocalLastSyncDate<TestObject>(expected);

            DateTime actual = dataService.localDataAccess.GetLocalLastSyncDate<TestObject>();

            Assert.Equal(expected, actual);
        }



        async static private void InsertDelay(int milliSeconds)
        {
            await Task.Delay(milliSeconds);
        }


    }
}
