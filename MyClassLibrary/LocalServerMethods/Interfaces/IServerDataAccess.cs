
using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    public interface IServerDataAccess<T> where T : LocalServerModelUpdate
    {

        /// <summary>
        /// Returns the data stored on server for the list of Guids provided.
        /// </summary>
        public Task<List<T>> GetFromServer(List<Guid>? ids = null);

        ///// <summary>
        ///// Returns the data stored on server for the list of Guids provided.
        ///// </summary>
        //public Task<List<T>> GetLatestFromServer<T>(List<Guid>? ids = null) where T : LocalServerIdentityUpdate;

        /// <summary>
        /// Finds all objects on the Server where the UpdatedOnServer date is later than LastSyncDate
        /// </summary>
        public Task<(List<T> changesFromServer, DateTime lastUpdatedOnServer)> GetChangesFromServer(DateTime LastSyncDate);


        /// <summary>
        /// Saves Objects Data that inherit from LocalServerIdentity into Server Storage. Pass back UpdatedOnServer dat to objects.
        /// </summary>
        public Task<DateTime> SaveToServer(List<T> updates);

        /// <summary>
        /// Adds the corresponding Conflict Id (value) to all objects inhereting from local server identity with Id = (key)
        /// </summary>
        public Task SaveConflictIdsToServer(List<Conflict> conflicts);

        /// <summary>
        /// Permanently deletes the objects inheriting from LocalServerIdentiy passed in from server storage.
        /// </summary>
        public Task DeleteFromServer(List<T> updates);



    }
}
