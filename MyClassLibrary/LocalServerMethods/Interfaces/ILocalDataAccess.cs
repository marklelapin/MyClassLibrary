
using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    /// <summary>
    /// The interface providing access to Local Data Storage
    /// </summary>
    public interface ILocalDataAccess<T> where T : ILocalServerModelUpdate 
    {
        /// <summary>
        /// Gets all updates relating to the list of Ids.
        /// </summary>
        public Task<List<T>> GetUpdatesFromLocal(List<Guid>? ids = null,bool latestOnly=false);

        /// <summary>
        /// Gets the Guid associated with the particular local copy being used.
        /// </summary>
        /// <returns></returns>
        public Task<Guid> GetLocalCopyID();

        /// <summary>
        /// Returns all updates inherting from LocalServerIdentity on local storage yet to be marked as saved to Server.
        /// </summary>
        public Task<List<T>> GetUnsyncedFromLocal();

        /// <summary>
        /// Returns all updates marked a IsConflicted = true from local storage relating to ids provided.
        /// </summary>
        public Task<List<T>> GetConflictedUpdatesFromLocal(List<Guid>? ids = null);


        /// <summary>
        /// Saves Updates inhereting from LocalServerIdentity to Local Storage
        /// </summary>
        public Task<List<LocalToServerPostBack>> SaveUpdatesToLocal(List<T> updates);

        /// <summary>
        /// Operation performed after saving to server to confirm save has happened. Saves UpdatedOnServerDate and IsConflict = true if a conflict is found.
        /// </summary>
        public Task ServerPostBackToLocal(List<ServerToLocalPostBack> postBacks);

        /// <summary>
        /// Permanently deletes the objects inheriting from LocalServerIdentiy passed in from local storage.
        /// </summary>
        public Task DeleteFromLocal(List<T> updates);

        /// <summary>
        /// Mark IsConflicted = false on all updates associated with the ids passed in.
        /// </summary>
        public Task ClearConflictsFromLocal(List<Guid> ids);

        /// <summary>
        /// Gets the Last Successfull Sync Date from Local Storage
        /// </summary>
        public Task<DateTime> GetLocalLastSyncDate();


        /// <summary>
        /// Save the Last Successufl Sync Date to Local Storage
        /// </summary>
        public Task SaveLocalLastSyncDate(DateTime lastSyncDate);


        /// <summary>
        /// Resets the sample data on Local Storage without affecting other data.
        /// </summary>
        /// <returns></returns>
        public Task<bool> ResetSampleData(string folderPath);
    }
}
