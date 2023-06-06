using Microsoft.Identity.Client;
using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    /// <summary>
    /// The interface providing management of data access between local and server storage.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILocalServerEngine<T> where T : ILocalServerModelUpdate
    {

        /// <summary>
        /// Returns all updates for the type sorted By id and created date.
        /// </summary>
        Task<List<T>> GetAllUpdates();
        /// <summary>
        /// Returns all updates for the given id sorted By id and created date.
        /// </summary>
        Task<List<T>> GetAllUpdates(Guid id);
        /// <summary>
        /// Returns all updates for the given ids sorted by id and created date. If ids is null returns all.
        /// </summary>
        Task<List<T>> GetAllUpdates(List<Guid>? ids);


        /// <summary>
        /// Returns latest update for all models matching Id(s) of the type sorted By id and created date. No parameters returns all models.
        /// </summary>
        Task<List<T>> GetLatestUpdates();
        /// <summary>
        /// Returns latest update for all models matching Id(s) of the type sorted By id and created date. No parameters returns all models.
        /// </summary>
        Task<List<T>> GetLatestUpdates(Guid id);
        /// <summary>
        /// Returns latest update for all models matching Id(s) of the type sorted By id and created date. No parameters returns all models.
        /// </summary>
        Task<List<T>> GetLatestUpdates(List<Guid>? ids);



        /// <summary>
        ///  Returns all updates that contain a conflictID matching the conflictId(s)  given. Null returns all updates relating to all latest updates with a conflictID. 
        /// </summary>
        Task<List<T>> GetConflictedUpdates();
        /// <summary>
        /// Returns all updates that contain a conflictID matching the conflictId given.
        /// </summary>
        Task<List<T>> GetConflictedUpdates(Guid id);
        /// <summary>
        /// Returns all updates that contain a conflictID matching the conflictIds given.
        /// </summary>
        Task<List<T>> GetConflictedUpdates(List<Guid>? ids);




        /// <summary>
        /// Saves the update to local and server storage (if it can) (and syncAfterwards parameter is set to true(default))
        /// </summary>
        Task SaveUpdates(T update,bool syncAfterwards = true);
        /// <summary>
        /// Saves the list of updates to local and server storage (if it can) (and syncAfterwards parameter is set to true(default))
        /// </summary>
        Task SaveUpdates(List<T> updates,bool syncAfterwards = true);

        /// <summary>
        /// Mark IsConflicted = false to local and or server storage for the Id passed in
        /// </summary>
        Task<bool> ClearConflictIds(Guid Id);
        /// <summary>
        /// Mark IsConflicted = false to local and or server storage for all updates with Ids passed in.
        /// </summary>
        Task<bool> ClearConflictIds(List<Guid> Ids);
       


        /// <summary>
        /// Attempts to sync local and server storage. Returns true if successfull and the DateTime it synced..
        /// </summary>
        /// <returns></returns>
        Task<(DateTime? syncedDateTime, bool success)> TrySync();
 
        
        /// <summary>
        /// Deletes all updates from local and server storage that match the given Id. This is irreversible.
        /// </summary>
        Task DeleteEntirely(List<T> updates);



        /// <summary>
        /// Changes IlocalDataAccess for the purposes of testing sync failures.
        /// </summary>
        public void ChangeLocalDataAccess(ILocalDataAccess<T> localDataAccess);

        /// <summary>
        /// Changes IServerDataAccess for the purposes of testing sync failures.
        /// </summary>
        public void ChangeServerDataAccess(IServerDataAccess<T> serverDataAccess);
    }
}