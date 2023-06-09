

namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    /// <summary>
    /// Unit/Integration tests of ILocalServerModelFactory 
    /// </summary>
    public interface ILocalServerModelFactoryTests
    {

        /// <summary>
        /// Passes Id = null and checks that a new empty Model is created
        /// </summary>
        /// <remarks>
        /// Latest,History and Conflicts should all be null.
        /// </remarks>
        public Task CreateModel_NewTest();
        /// <summary>
        /// Passes existing Id from Sample Data and checks that Latest property matches Sample Data AND that History is null.
        /// </summary>
        public Task CreateModel_ExistingTest();
        /// <summary>
        /// Passes single Id from Sample Data with IsConflicted = true. Checks that Conflicts is also populated with other conflicted updates from Sample Data.
        /// </summary>
        public Task CreateModel_ConflictedTest();
        /// <summary>
        /// Passes two Ids from Sample Data and checks that Latest properties of each of them in the list matches Sample Data.
        /// </summary>
        public Task CreateModelListTest();
        /// <summary>
        /// Passes null into CreateModelList. Checks that all Sample Data is brought back.
        /// </summary>
        public Task CreateModelList_AllTest();

        /// <summary>
        /// Creates a model from Sample Data, changes it, then refreshes it. Checks that it matches back to Sample Data
        /// </summary>
        public Task RefreshLatestTest_Model();
        /// <summary>
        /// Creates a list of models from Sample Data, changes them, then refreshes them. Checks that all match their original Sample Data. 
        /// </summary>
        public Task RefreshLatest_ListModels();




        /// <summary>
        /// Creates a model from Sample Data. Refreshes History. Checks that History is not null and matches Sample Data.
        /// </summary>
        public Task RefreshHistoryTest_Model();
        /// <summary>
        /// Creates a list of model from Sample Data. Refreshes History. Checks that History is not null and matches Sample Data for each model.
        /// </summary>
        public Task RefreshHistoryTest_ListModels();


     
        /// <summary>
        /// Creates a model from Sample Data. Changes IsConflicted. Refreshes Conflicts. Checks that Conflicts is not null and matches Sample Data.
        /// </summary>
        public Task RefreshConflictsTest_Model();
        /// <summary>
        /// Creates a list of models from Sample Data. Changes IsConflcited. Refreshes Conflicts. Checks that Conflicts is not null and matches Sample Data for each model.
        /// </summary>
        public Task RefreshConflictsTest_ModelList();




        /// <summary>
        /// Creates a model with isActive = true. Runs DeActivate. Checks that the model is the same but for IsActive = false.
        /// </summary>
        public Task DeActivateTest();

        /// <summary>
        /// Creates a model with isActive = false. Runs Reactive. Checks that the model has an additonial update with everything the same except IsActive = false.
        /// </summary>
        public Task ReActivateTest();
        /// <summary>
        /// Gets a model with conflicts from SampleData. ResolvesConflict with one of the conflicted updates. Checks that all conflicts have been removed and chose update is latest update.
        /// </summary>
        public Task ResolveConflictTest();
        /// <summary>
        /// Creates a new model from Sample Data and gets its history. chooses a previous update from the model to restore. Restores it. Checks that a new update has been created and model LAtest and History Updated.
        /// </summary>
        public Task RestoreTest();
        /// <summary>
        /// Creates a new model from Sample Data. Adds a new Update for the model. Runs Update. Checks that the models Latest and History properties are updated.
        /// </summary>
        public Task UpdateTest();


        ////TODO - Make final decision on removing DeleteEntirely functionality.
        ///// <summary>
        ///// Deletes all updates of the model from local/server storage relating to the Id. This is irreversible and there requires bool areYouSure = true.
        ///// </summary>
        //Task DeleteEntirely(List<U> updates, bool areYouSure);
    }
}
