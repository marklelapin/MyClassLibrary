

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    /// <summary>
    /// The interface providing all methods to create and amend Updates, LocalServerModels and Lists of updates and models.
    /// </summary>
    public interface ILocalServerModelFactory<T, U> where T : ILocalServerModel<U>, new() where U : ILocalServerModelUpdate, new()
    {
        /// <summary>
        /// Creates a new LocalServerModel populating it with the Latest update from local/server storage if Id passed in. id = null generates new Id and empty model.
        /// </summary>
        public Task<T> CreateModel(Guid? Id = null);
        /// <summary>
        /// Creates a list of LocalServerModels with the Latest update from local/server storage. ids = null returns all models
        /// </summary>
        public Task<List<T>> CreateModelList(List<Guid>? ids = null);





        /// <summary>
        /// Adds/Refreshes the Latest property with the latest update from local/server storage. 
        /// </summary>
        public Task RefreshLatest(T model);
        /// <summary>
        /// Adds/Refreshes the Latest property of the models in ListModel with the latest updates from local/server storage. 
        /// </summary>
        public Task RefreshLatest(List<T> models);




        /// <summary>
        /// Adds/Refreshes the History property of the model with all updates from local/server storage.
        /// </summary>
        public Task RefreshHistory(T model);
        /// <summary>
        /// Adds/Refreshes the History property of the models in ListModel with all updates from local/server storage.
        /// </summary>
        public Task RefreshHistory(List<T> models);


        /// <summary>
        /// Performs the Sync operation to match local with server.
        /// </summary>
        /// <returns></returns>
        public Task Sync();


        /// <summary>
        /// Adds/Refreshes the Conflicts property of the model with updates from local/server storage containing the same conflictId as Latest update. 
        /// </summary>
        public Task RefreshConflicts(T model);
        /// <summary>
        /// Adds/Refreshes the Conflicts property of the models in ListModel with updates from local/server storage containing the same conflictId as Latest update. 
        /// </summary>
        public Task RefreshConflicts(List<T> models);







        /// <summary>
        /// Saves a new update of the model to local/server storage with IsActive = false and updates both Latest and History properties.
        /// </summary>
        public Task DeActivate(T model);



        ////TODO - Make final decision on removing DeleteEntirely functionality.
        ///// <summary>
        ///// Deletes all updates of the model from local/server storage relating to the Id. This is irreversible and there requires bool areYouSure = true.
        ///// </summary>
        //Task DeleteEntirely(List<U> updates,bool areYouSure);



        /// <summary>
        /// Saves a new update of the model to local/server storage based on Latest update but with IsActive = true and updates both Latest and History properties.
        /// </summary>
        public Task ReActivate(T model);
        /// <summary>
        /// Saves a new update of the model to local/server storage based on selected update but with ConflictId removed and updates both Latest and History properties.
        /// </summary>
        public Task ResolveConflict(T model,U update);
        /// <summary>
        /// Saves a new update of the model to local/server storage based on the update passed in and updates both Latest and History properties.
        /// </summary>
        public Task Restore(T model,U update);
        /// <summary>
        /// Adds a new update of the model to local/server storage and updates both Latest and History properties.
        /// </summary>
        public Task Update(T model, U update);

    }


   


}