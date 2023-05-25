
using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    public interface ILocalDataAccess<T> where T : LocalServerModelUpdate
    {

        /// <summary>
        /// Gets the Last Successfull Sync Date from Local Storage
        /// </summary>
        public Task<DateTime> GetLocalLastSyncDate();


        /// <summary>
        /// Save the Last Successufl Sync Date to Local Storage
        /// </summary>
        public Task SaveLocalLastSyncDate(DateTime lastSyncDate);

        /// <summary>
        /// Gets all updates relating to the list of Ids.
        /// </summary>
        public Task<List<T>> GetFromLocal(List<Guid>? ids = null);

        ///// <summary>
        ///// Gets latest update relating to the list of Ids.
        ///// </summary>
        //public Task<List<T>> GetLatestFromLocal<T>(List<Guid>? ids = null) where T : LocalServerIdentityUpdate;

        /// <summary>
        /// Loads Objects inhereting from LocalServerIdentity from Local Storage with no UpdatedOnServer date.
        /// </summary>
        public Task<List<T>> GetChangesFromLocal();


        /// <summary>
        /// Saves Objects inhereting from LocalServerIdentity to Local Storage
        /// </summary>
        public Task SaveToLocal(List<T> updates);

        /// <summary>
        /// Sets the UpdatedOnServerDate property for objects inhereting from LocalServerIdentity to LocalStorage
        /// </summary>
        public Task SaveUpdatedOnServerToLocal(List<T> updates, DateTime UpdatedOnServer);

        /// <summary>
        /// Adds the corresponding Conflict Id (value) to all objects inhereting from LocalServerIdentity with Id = (key)
        /// </summary>
        public Task SaveConflictIdsToLocal(List<Conflict> conflicts);

        /// <summary>
        /// Permanently deletes the objects inheriting from LocalServerIdentiy passed in from local storage.
        /// </summary>
        public Task DeleteFromLocal(List<T> updates);
    }
}
