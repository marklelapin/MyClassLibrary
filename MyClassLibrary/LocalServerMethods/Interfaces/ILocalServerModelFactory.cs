using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    /// <summary>
    /// The interface providing all methods to create and amend Updates, LocalServerModels and Lists of Loc.
    /// </summary>
    public interface ILocalServerModelFactory<T, U> where T : ILocalServerModel<U>, new() where U : ILocalServerModelUpdate, new()
    {
        /// <summary>
        /// Creates a new LocalServerModel populating it with the Latest update from local/server storage if Id passed in.
        /// </summary>
        Task<T> CreateModel(Guid? Id = null);
        /// <summary>
        /// Creates a list of LocalServerModels with the Latest update from local/server storage.
        /// </summary>
        Task<List<T>> CreateModelsList(List<Guid>? ids = null);





        /// <summary>
        /// Adds/Refreshes the Latest property with the latest update from local/server storage. 
        /// </summary>
        Task RefreshLatest(T model);
        /// <summary>
        /// Adds/Refreshes the Latest property of the models in ListModel with the latest updates from local/server storage. 
        /// </summary>
        Task RefreshLatest(List<T> models);




        /// <summary>
        /// Adds/Refreshes the History property of the model with all updates from local/server storage.
        /// </summary>
        Task RefreshHistory(T model);
        /// <summary>
        /// Adds/Refreshes the History property of the models in ListModel with all updates from local/server storage.
        /// </summary>
        Task RefreshHistory(List<T> models);


        /// <summary>
        /// Performs the Sync operation to match local with server.
        /// </summary>
        /// <returns></returns>
        Task Sync();


        /// <summary>
        /// Adds/Refreshes the Conflicts property of the model with updates from local/server storage containing the same conflictId as Latest update. 
        /// </summary>
        Task RefreshConflicts(T model);
        /// <summary>
        /// Adds/Refreshes the Conflicts property of the models in ListModel with updates from local/server storage containing the same conflictId as Latest update. 
        /// </summary>
        Task RefreshConflicts(List<T> models);







        /// <summary>
        /// Saves a new update of the model to local/server storage with IsActive = false and updates both Latest and History properties.
        /// </summary>
        Task DeActivate(T model);
        /// <summary>
        /// Deletes all updates of the model from local/server storage relating to the Id. This is irreversible and there requires bool areYouSure = true.
        /// </summary>
        Task DeleteEntirely(List<U> updates,bool areYouSure);
        /// <summary>
        /// Saves a new update of the model to local/server storage based on Latest update but with IsActive = true and updates both Latest and History properties.
        /// </summary>
        Task ReActivate(T model);
        /// <summary>
        /// Saves a new update of the model to local/server storage based on selected update but with ConflictId removed and updates both Latest and History properties.
        /// </summary>
        Task ResolveConflict(T model,U update);
        /// <summary>
        /// Saves a new update of the model to local/server storage based on the update passed in and updates both Latest and History properties.
        /// </summary>
        Task Restore(T model,U update);
        /// <summary>
        /// Adds a new update of the model to local/server storage and updates both Latest and History properties.
        /// </summary>
        Task Update(T model, U update);

    }


   


}