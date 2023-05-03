﻿using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods

{
    public class LocalDataAccessTests
    {
        private static IServiceConfiguration _serviceConfiguration;

        private static IServiceConfiguration ServiceConfiguration
        {
            get { return _serviceConfiguration; }
            set { _serviceConfiguration = value; }
        }


        public LocalDataAccessTests(IServiceConfiguration? serviceConfiguration = null)
        {
            _serviceConfiguration = serviceConfiguration ?? new Tests.LocalServerMethods.DefaultServiceConfiguration();
        }


        private static ILocalDataAccess _localDataAccess = ServiceConfiguration.LocalDataAccess
        private static ITestContent _testContent = ServiceConfiguration.TestContent;

        private static readonly List<TestContent> SaveAndGetTestContents = new List<TestContent>().GenerateTestContents(3);

        public static readonly object[][] SaveAndGetTestData =
        {
            new object[] {
                            SaveAndGetTestContents[0].TestObjects
                            ,SaveAndGetTestContents[0].TestIds()
                            ,SaveAndGetTestContents[0].TestObjects
                           },
            new object[]
                        {
                            SaveAndGetTestContents[1].TestObjects
                            ,SaveAndGetTestContents[1].TestIds()
                            ,SaveAndGetTestContents[1].TestObjects
                           },
            new object[]
                        {
                            SaveAndGetTestContents[2].TestObjects
                            ,new List<Guid> {SaveAndGetTestContents[2].TestIds()[2]}
                            ,SaveAndGetTestContents[2].TestObjects.Where(x => x.Id == SaveAndGetTestContents[2].TestIds()[2]).ToList()
                           }
        };
        [Theory, MemberData(nameof(SaveAndGetTestData))]
        public void SaveAndGetTest(List<TestObject> testObjects,List<Guid> testIds,List<TestObject> expected)
        {
            dataService.localDataAccess.SaveToLocal(testObjects);

           List<TestObject> actual =  dataService.localDataAccess.GetFromLocal<TestObject>(testIds);

            actual.Sort((x,y)=>x.Id.CompareTo(y.Id));
            expected.Sort((x, y) => x.Id.CompareTo(y.Id));

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
        }

        

        [Fact]
         public void GetChangesFromLocalTest() //also tests functionality of passing null into GetFromLocal
        {
         
            TestContent testContent = new List<TestContent>().GenerateTestContents(1)[0];

            dataService.localDataAccess.SaveToLocal(testContent.TestObjects);

            List<TestObject> allTestObjects = dataService.localDataAccess.GetFromLocal<TestObject>(null);

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







        [Fact]
        public void SaveUpdatedOnServerTest()
        {
            DateTime updatedOnServer = DateTime.Now;

            List<TestContent> testContents = new List<TestContent>().GenerateTestContents(1);

            dataService.localDataAccess.SaveToLocal(testContents[0].TestObjects);

            dataService.localDataAccess.SaveUpdatedOnServerToLocal(testContents[0].TestObjects,updatedOnServer);

            List<TestObject> actualUpdated = dataService.localDataAccess.GetFromLocal<TestObject>(testContents[0].TestIds());

            List<TestObject> actualNotUpdated = dataService.localDataAccess.GetFromLocal<TestObject>().Where(x => !actualUpdated.Any(y=>y.Id==x.Id)).ToList();

            Assert.True(actualNotUpdated.Where(x=>x.UpdatedOnServer == updatedOnServer).Count() == 0,"No Other Objects Have UpdatedOnServerDate");
            Assert.True(actualUpdated.Where(x => x.UpdatedOnServer == updatedOnServer).Count() == testContents[0].TestObjects.Count());
        }


        public static readonly List<TestContent> saveConflictIDTestContents = new List<TestContent>().GenerateTestContents(2);

        public static readonly object[][] saveConflictIDTestDate =
        {
            new object[]{saveConflictIDTestContents[0],true},
            new object[]{saveConflictIDTestContents[1],false}
        };

        [Theory, MemberData(nameof(saveConflictIDTestDate))]
        public void SaveConflictIDToLocalTest(TestContent testContent, bool conflictsExist)
        {


            List<TestObject> conflictedObjects = new List<TestObject>();
            conflictedObjects.Add(testContent.TestObjects[1]);
            conflictedObjects.Add(testContent.TestObjects[5]);

            Dictionary<Guid, Guid> conflictDictionary = new Dictionary<Guid, Guid>();

            conflictDictionary.Add(conflictedObjects[0].Id, Guid.NewGuid());
            conflictDictionary.Add(conflictedObjects[1].Id, Guid.NewGuid());

            List<Conflict> conflicts = new List<Conflict>();
            if (conflictsExist)
            {
                conflicts.Add(new Conflict(conflictedObjects[0].Id, conflictedObjects[0].Created, conflictDictionary.GetValueOrDefault(conflictedObjects[0].Id)));
                conflicts.Add(new Conflict(conflictedObjects[1].Id, conflictedObjects[1].Created, conflictDictionary.GetValueOrDefault(conflictedObjects[1].Id)));
            }

            dataService.localDataAccess.SaveToLocal(testContent.TestObjects);
            dataService.localDataAccess.SaveConflictIdsToLocal<TestObject>(conflicts);

            List<Conflict> actual = new List<Conflict>();

            actual = dataService.localDataAccess.GetFromLocal<TestObject>(testContent.TestIds())
                                                    .Where(x => x.ConflictId != null)
                                                    .Select(x => new Conflict(x.Id, x.Created, x.ConflictId))
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

            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));

        }


        async static private void InsertDelay(int milliSeconds)
        {
            await Task.Delay(milliSeconds);
        }


    }
}