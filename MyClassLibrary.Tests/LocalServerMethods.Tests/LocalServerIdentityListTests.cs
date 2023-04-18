using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyClassLibrary.LocalServerMethods;
using Xunit.Sdk;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class LocalServerIdentityListTests
    {
        DataService dataService = new DataService();

        public static readonly List<TestContent> SaveAndHistoryTestContents = new List<TestContent>().GenerateTestContents(3);


        public static readonly object[][] SaveAndHistoryTestData =
        {
                            new object[]{ SaveAndHistoryTestContents[0].TestObjects
                                           ,SaveAndHistoryTestContents[0].TestIds()
                                           ,SaveAndHistoryTestContents [0].TestObjects
                                        }
                            ,new object[]{ SaveAndHistoryTestContents[1].TestObjects
                                           ,new List<Guid> {SaveAndHistoryTestContents[1].TestIds()[1] }
                                           ,SaveAndHistoryTestContents [1].TestObjects.Where(x=>x.Id == SaveAndHistoryTestContents[1].TestIds()[1]).ToList()
                                        }
                             ,new object[]{ SaveAndHistoryTestContents[2].TestObjects
                                           ,new List<Guid> {SaveAndHistoryTestContents[2].TestIds()[2] }
                                           ,SaveAndHistoryTestContents [2].TestObjects.Where(x=>x.Id == SaveAndHistoryTestContents[2].TestIds()[2]).ToList()
                                        }
        };
        [Theory, MemberData(nameof(SaveAndHistoryTestData))]
        public void SaveAndGetTest(List<TestObject> objects, List<Guid> ids, List<TestObject> expected)
        {
            LocalServerIdentityList<TestObject> testList = new LocalServerIdentityList<TestObject>(
                objects
                , dataService.serverDataAccess
                , dataService.localDataAccess
                );

            testList.Save();

            LocalServerIdentityList<TestObject> actualList = new LocalServerIdentityList<TestObject>(
                    null
                    , dataService.serverDataAccess
                    , dataService.localDataAccess
                );

            actualList.GetObjects(ids);

            List<TestObject> actual = actualList.Objects;

            expected.Sort((x, y) => (x.Id.CompareTo(y.Id)));
            actual.Sort((x, y) => (x.Id.CompareTo(y.Id)));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        }



        [Fact] public void SortByIdTest() 
        {
            List<Guid> overrideIds = new List<Guid>();

            for (int i = 0; i < 10; i++)
            {
                overrideIds.Add(Guid.NewGuid());
            }

            TestContent actualTestContent = new List<TestContent>().GenerateTestContents(1, "Unsorted",overrideIds, DateTime.Now)[0];
            TestContent expectedTestContent = new List<TestContent>().GenerateTestContents(1, "SortedById", overrideIds, DateTime.Now)[0];

            LocalServerIdentityList<TestObject> actualList = new LocalServerIdentityList<TestObject>(actualTestContent.TestObjects);
            
            actualList.SortById();

            List<TestObject> actual = actualList.Objects;

            List<TestObject> expected = expectedTestContent.TestObjects;

            Assert.Equal(JsonSerializer.Serialize(expected),JsonSerializer.Serialize(actual));

        }


        [Fact]
        public void SortByCreatedTest()
        {
            List<Guid> overrideIds = new List<Guid>();

            for (int i = 0; i < 10; i++)
            {
                overrideIds.Add(Guid.NewGuid());
            }

            TestContent actualTestContent = new List<TestContent>().GenerateTestContents(1, "Unsorted", overrideIds, DateTime.Now)[0];
            TestContent expectedTestContent = new List<TestContent>().GenerateTestContents(1, "SortedByCreated", overrideIds, DateTime.Now)[0];


            LocalServerIdentityList<TestObject> actualList = new LocalServerIdentityList<TestObject>(actualTestContent.TestObjects);

            actualList.SortByCreated();

            List<TestObject> actual = actualList.Objects;

            List<TestObject> expected = expectedTestContent.TestObjects;

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        }


        [Fact]
        public void HistoryTest()
        {
            List<Guid> overrideIds = new List<Guid>();

            for (int i = 0; i < 10; i++)
            {
                overrideIds.Add(Guid.NewGuid());
            }

            TestContent actualTestContent = new List<TestContent>().GenerateTestContents(1, "Unsorted", overrideIds, DateTime.Now)[0];
            TestContent expectedTestContent = new List<TestContent>().GenerateTestContents(1, "History", overrideIds, DateTime.Now)[0];

            LocalServerIdentityList<TestObject> actualList = new LocalServerIdentityList<TestObject>(actualTestContent.TestObjects);
            
            List<TestObject> actual = actualList.History();

            List<TestObject> expected = expectedTestContent.TestObjects;

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        }



    }
}
