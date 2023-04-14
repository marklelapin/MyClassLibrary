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

        private static readonly ConnectionStringDictionary connectionStringDictionary = new ConnectionStringDictionary();

        private ILocalDataAccess _localDataAccess = new LocalSQLConnector(connectionStringDictionary.LocalSQL);



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
            _localDataAccess.SaveToLocal(testObjects);

           List<TestObject> actual =  _localDataAccess.GetFromLocal<TestObject>(testIds,isActive);

            actual.SortById();
            expected.SortById();

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }

        

        [Fact]
         public void GetChangesFromLocalTest() //also tests functionality of passing null into GetFromLocal
        {
            TestContent testContent = new List<TestContent>().GenerateTestContents(1)[0];

            _localDataAccess.SaveToLocal(testContent.TestObjects);

            List<TestObject> allTestObjects = _localDataAccess.GetFromLocal<TestObject>(null,false);

            List<TestObject> expected = allTestObjects.Where(x=>x.UpdatedOnServer == null).ToList();

            List<TestObject> actual = _localDataAccess.GetChangesFromLocal<TestObject>();

            actual.SortById();
            expected.SortById();

            Assert.Equal(JsonSerializer.Serialize(expected),JsonSerializer.Serialize(actual));
        }
    }
}
