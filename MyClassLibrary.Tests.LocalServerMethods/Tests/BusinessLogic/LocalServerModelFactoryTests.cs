
using MyClassLibrary.LocalServerMethods.Extensions;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;

using System.Text.Json;


namespace MyClassLibrary.Tests.LocalServerMethods.Tests.BusinessLogic
{
    public class LocalServerModelFactoryTests : ILocalServerModelFactoryTests
    {
        private readonly ILocalServerModelFactory<TestModel,TestUpdate> _factory;

        public LocalServerModelFactoryTests(ILocalServerModelFactory<TestModel,TestUpdate> factory)
        {
            _factory = factory;
        }


        //Create Model Tests
        [Fact]
        public async Task CreateModel_NewTest()
        {
            //Test
            TestModel model = await _factory.CreateModel();

            //Assert
            Assert.True(model != null,"Create Model failed to provide Id.");
            Assert.True(model.Latest == null,"Added in Latest prematurely and when there isn't a latest.");
            Assert.True(model.History == null, "Added History prematurely");
            Assert.True(model.Conflicts == null, "Added Conflicts prematurely");
        }

        [Fact]
        public async Task CreateModel_ExistingTest()
        {


            //Test
            TestModel model = await _factory.CreateModel(TestContent.SingleTestId);
            TestUpdate expectedLatest = TestContent.SingleLatestUpdateOnLocal.First();


            //Assert
            Assert.True(model.Id == TestContent.SingleTestId, "New Guid created instead of using one provided.");
            Assert.Equal(JsonSerializer.Serialize(expectedLatest),JsonSerializer.Serialize(model.Latest));
            Assert.True(model.Conflicts == null,"Add Conflicts prematurely and when there aren't any.");
            Assert.True(model.History == null, "Added History prematurely.");
        }

        [Fact]
        public async Task CreateModel_ConflictedTest()
        {
            //Test
            TestModel model = await _factory.CreateModel(TestContent.ConflictedTestId);

            TestUpdate expectedLatest = TestContent.ConflictedTestUpdateLatest.First();
            //Assert
            Assert.True(JsonSerializer.Serialize(expectedLatest)==JsonSerializer.Serialize(model.Latest),"Latest Test Update is Incorrect.");
            Assert.True(JsonSerializer.Serialize(model.Conflicts) == JsonSerializer.Serialize(TestContent.ConflictedTestUpdates), "Conflicts don't match");
            Assert.True(model.Conflicts?.Count > 0, "Conflicts not added.");
        }

        [Fact]
        public async Task CreateModelListTest()
        {
            //Test
            List<TestModel> models = await _factory.CreateModelList(TestContent.TwoTestIds);

            //Assert
            Assert.True(models.Count == 2,"Wrong number of models created.");

            //Get Listupdate details
            List<TestUpdate> updates = new List<TestUpdate>();

            models.ForEach(model =>
            {
                updates.Add(model.Latest!);
            });


            //Assert
            Assert.True(JsonSerializer.Serialize(TestContent.TwoLatestTestUpdatesOnLocal) == JsonSerializer.Serialize(updates), "Latest Updates don't match.");
            
        }

        [Fact]
        public async Task CreateModelList_AllTest()
        {
            //Setup
            int expectedNoModels = TestContent.LocalStartingData.Select(x=>x.Id).Distinct().Count();    
            //Test
            List<TestModel> models = await _factory.CreateModelList();

            //Assert
            Assert.True(models.Count == expectedNoModels, "Wrong number of models created");
        }


        //Refresh Data Tests
        [Fact]
        public async Task RefreshConflictsTest_Model()
        {
            
            //Setup
            TestModel model = await _factory.CreateModel(TestContent.ConflictedTestId);
            TestModel expected = model;
            Assert.True(model.Conflicts?.Count > 0, "Test Error. TestModel created doesn't have Conflcits.");

            model.Conflicts.Clear();


            //Test
            await _factory.RefreshConflicts(model);

            //Assert
            Assert.True(JsonSerializer.Serialize(expected) == JsonSerializer.Serialize(model), "IsConflicted not refreshed back to original value.");
        }
        [Fact]
        public async Task RefreshConflictsTest_ModelList()
        {
            //Setup
            List<TestModel> models = await _factory.CreateModelList();//Gets All sample data
            List<TestUpdate> expectedConflictedUpdates = TestContent.ConflictedTestUpdates;
            models.ForEach(model =>
            {
                Assert.True((model.Conflicts?.Count ?? 0)== 0, "Test Error: Conflicts should not be present from Sample Data after CreateModelList run.");
            });
      
            //Test
            await _factory.RefreshConflicts(models);
            List<TestUpdate> actualConflictedUpdates = new List<TestUpdate>();
            models.ForEach(model =>
            {
                if (model.Conflicts?.Count > 0) { actualConflictedUpdates.AddRange(model.Conflicts); };
            });


            //Assert conflicts are back in.
            Assert.True(JsonSerializer.Serialize(expectedConflictedUpdates) == JsonSerializer.Serialize(actualConflictedUpdates),"Conflicts don't match original conflicts.");

            
        }


        [Fact]
        public async Task RefreshHistoryTest_Model()
        {
            //Setup
            TestModel model = await _factory.CreateModel(TestContent.SingleTestId);
            Assert.True(model.History == null, "Premature addition of History data.");
            List<TestUpdate> expected = TestContent.SingleTestUpdatesOnLocal; ;
            expected = expected.SortByCreated();

            //Test
            await _factory.RefreshHistory(model);

            //Assert
            Assert.True(JsonSerializer.Serialize(expected) == JsonSerializer.Serialize(model.History), "History doesn't match Sample Data");

        }
        [Fact]
        public async Task RefreshHistoryTest_ListModels()
        {
            //Setup
            List<TestModel> models = await _factory.CreateModelList();
            Assert.True(models[0].History == null, "Test Error: Premature addition of History data by CreateModelList");
            List<TestUpdate> expected = TestContent.LocalStartingData; ;
            expected =  expected.SortByCreated();

            //Test
            await _factory.RefreshHistory(models);

            List<TestUpdate> actual = new();

            //CollateHistory
            models.ForEach(model =>
            {
                actual.AddRange(model.History!);
            }
            );
            actual = actual.SortByCreated();

            //Assert
            Assert.True(JsonSerializer.Serialize(expected) == JsonSerializer.Serialize(actual), "Histories don't match Sample Data");

        }


        [Fact]
        public async Task RefreshLatestTest_Model()
        {
            //Setup
            TestModel model = new TestModel();
            model.Id = TestContent.SingleTestId;
            TestUpdate expected = TestContent.SingleTestUpdatesOnLocal.First();

            //Test
            await _factory.RefreshLatest(model);

            //Assert
            Assert.True(JsonSerializer.Serialize(expected) == JsonSerializer.Serialize(model.Latest), "Latest Update doesn't match sample data.");

        }
        [Fact]
        public async Task RefreshLatest_ListModels()
        {
            //Setup
            List<TestModel> models = await _factory.CreateModelList();
            List<TestModel> expected = models;
            //Check setup
            Assert.True(models[0].Latest != null, "Test error: Latest not being populated by CreateModelList().");
            
            //remove latest
            models.ForEach(model => model.Latest = null);

            //Test
            await _factory.RefreshLatest(models);
            
            //Assert - latest back in.
            Assert.True(JsonSerializer.Serialize(expected) == JsonSerializer.Serialize(models), "Latest Update doesn't match sample data.");
        }


        //Utility Tests
        [Fact]
        public async Task DeActivateTest()
        {
            //Setup Model
            TestModel model = await _factory.CreateModel();
            TestUpdate update = TestContent.GetNewUpdates()[0];
            Assert.True(update.IsActive == true, "Test Error. Update created must be active.");
            model.Id = update.Id;
            model.Latest = update;
            model.History = new List<TestUpdate> { update };
            
            //Setup Expected
            List<TestUpdate> expectedHistory = new List<TestUpdate>() { update};
            TestUpdate expectedLatest = update;
            expectedLatest.IsActive = false;
            expectedHistory.Add(expectedLatest);
            expectedHistory = expectedHistory.SortByCreated();

            //Test
            await _factory.DeActivate(model);

            //Assert
            Assert.True(model.Latest.IsActive == false, "Latest Update is not deactivated.");
            Assert.True(expectedHistory.Count() == model.History.Count(), "Additional update not added into History.");
            Assert.True(JsonSerializer.Serialize(model.History.SortByCreated()) == JsonSerializer.Serialize(model.History),"History in wrong order.");
        }
        [Fact]
        public async Task ReActivateTest()
        {
            //Setup Model
            TestModel model = await _factory.CreateModel();
            TestUpdate update = TestContent.GetDeactivatedUpdate();
            Assert.True(update.IsActive == false, "Test Error. Update created must be inActive.");
            model.Id = update.Id;
            model.Latest = update;
            model.History = new List<TestUpdate> { update };

            //Setup Expected
            List<TestUpdate> expectedHistory = new List<TestUpdate>() { update };
            TestUpdate expectedLatest = update;
            expectedLatest.IsActive = true;
            expectedHistory.Add(expectedLatest);
            expectedHistory = expectedHistory.SortByCreated();

            //Test
            await _factory.ReActivate(model);

            //Assert
            Assert.True(model.Latest.IsActive == true, "Latest Update is not active.");
            Assert.True(expectedHistory.Count() == model.History.Count(), "Additional update not added into History.");
            Assert.True(JsonSerializer.Serialize(model.History.SortByCreated()) == JsonSerializer.Serialize(model.History), "History in wrong order.");
        }
        [Fact]
        public async Task ResolveConflictTest() 
        {
            //Setup
            TestModel model = await _factory.CreateModel(TestContent.ConflictedTestId);
            await _factory.RefreshHistory(model);
            int originalHistoryCount = model.History?.Count() ?? 0;
            Assert.True(originalHistoryCount > 0, "Test Error: RefreshHistory not providing History from Sample Data.");
            Assert.True(model.Conflicts?.Count > 1, "Test Error: Need more than 1 conflict from sample data.");
            
            
            TestUpdate chosenUpdate = model.Conflicts!.OrderBy(x=>x.Created).First(); //picks out earliest conflict
            
            //Test
            await _factory.ResolveConflict(model,chosenUpdate);

            //ammend expected to match created dates as this is set afterwards.
            TestUpdate expectedLatest = chosenUpdate;
            expectedLatest.Created = model.Latest!.Created;

            //Assert
            Assert.True(model.Conflicts.Count == 0, "Not all Conflicts have been cleared.");
            Assert.True(JsonSerializer.Serialize(expectedLatest) == JsonSerializer.Serialize(model.Latest), "Latest doesn't match chosen update.");
            Assert.True(model.History?.Count == originalHistoryCount+1, "New update wasn't added to history.");

        }
        [Fact]
        public async Task RestoreTest()
        {
            //Setup Model
            TestModel model = await _factory.CreateModel();
            TestUpdate update = TestContent.GetNewUpdates()[0];
            model.Id = update.Id;
            model.Latest = update;
            model.Latest.Created = model.Latest.Created.AddSeconds(-20);
            model.History = new List<TestUpdate> { update };

            //Setup Expected
            List<TestUpdate> expectedHistory = model.History;
            TestUpdate expectedLatest = update;//to have created date ammended later on.

            //Test
            await _factory.Restore(model,update);


            //Update Expected
            expectedLatest.Created = model.Latest.Created;
            expectedHistory.Add(expectedLatest);
           expectedHistory = expectedHistory.SortByCreated();

  
            //Assert
            Assert.True(expectedHistory.Count() == model.History.Count(), "Additional update not added into History.");
            Assert.True(JsonSerializer.Serialize(model.History.SortByCreated()) == JsonSerializer.Serialize(model.History), "History doesn't match expected. Likely history in wrong order.");
            Assert.True(JsonSerializer.Serialize(expectedLatest)== JsonSerializer.Serialize(model.Latest), "Latest doesn't match expected.");
        }
        [Fact]
        public async Task UpdateTest()
        {
            //Setup Model
            TestModel model = await _factory.CreateModel();
            List<TestUpdate> history = TestContent.GetNewUpdatesWithSameId();
            history = history.SortByCreated();
            TestUpdate latest = history.First();

            model.Id = latest.Id;
            model.Latest = latest;
            model.History = history;

            TestUpdate newUpdate = latest;
            newUpdate.FirstName = "New First Name";
            newUpdate.LastName = "New Last Name";
            newUpdate.FavouriteDate = DateTime.Parse("1918-11-11T11:11:11.1234567");
            newUpdate.FavouriteFoods = new List<string> { "Rice", "Bread" };


            //Test
            await _factory.Update(model, newUpdate);

            //update newUpdate with created date so that they match
            newUpdate.Created = model.Latest.Created;
            //DefineExpected
            TestUpdate expectedLatest = newUpdate;
            List<TestUpdate> expectedHistory = history;
            expectedHistory.Add(expectedLatest);
            expectedHistory = expectedHistory.SortByCreated();



            //Assert
             Assert.True(JsonSerializer.Serialize(expectedLatest) == JsonSerializer.Serialize(model.Latest), "Latest doesn't match expected.");
             Assert.True(JsonSerializer.Serialize(expectedHistory) == JsonSerializer.Serialize(model.History), "History doesn't match expected.");
           
        }
    }
}
