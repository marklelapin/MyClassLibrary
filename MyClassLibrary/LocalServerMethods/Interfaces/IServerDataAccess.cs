
using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    /// <summary>
    /// The interface providing access to Server Data Storage
    /// </summary>
    public interface IServerDataAccess<T> where T : ILocalServerModelUpdate
    {

        /// <summary>
        /// Returns the data stored on server for the list of Guids provided.
        /// </summary>
        public Task<List<T>> GetUpdatesFromServer(List<Guid>? ids = null,bool latestOnly = false);

        /// <summary>
        /// Finds all objects on the Server where the UpdatedOnServer date is later than LastSyncDate
        /// </summary>
        public Task<List<T>> GetUnsyncedFromServer(Guid localCopyId);

        /// <summary>
        /// Returns all updates marked a IsConflicted = true from server storage relating to ids provided.
        /// </summary>
        public Task<List<T>> GetConflictedUpdatesFromServer(List<Guid>? ids = null);

        /// <summary>
        /// Saves Objects Data that inherit from LocalServerIdentity into Server Storage. Pass back UpdatedOnServer dat to objects.
        /// </summary>
        public Task<List<ServerToLocalPostBack>> SaveUpdatesToServer(List<T> updates,Guid localCopyId);

        /// <summary>
        /// Updates server storage with data to confirm save from server to local has completed satisfactorily. Also updates any conflicts that may have been found when saving to Local.
        /// </summary>
        public Task LocalPostBackToServer(List<LocalToServerPostBack> postBacks, Guid localCopyId);

        /// <summary>
        /// Permanently deletes the objects inheriting from LocalServerIdentiy passed in from server storage.
        /// </summary>
        public Task DeleteFromServer(List<T> updates);

        /// <summary>
        /// Mark IsConflicted = false on all updates associated with the ids passed in.
        /// </summary>
        public Task ClearConflictsFromServer(List<Guid> ids);

        /// <summary>
        /// Resets the sample data on Server Storage without affecting other data.
        /// </summary>
        public Task<bool> ResetSampleData(List<T> sampleUpdates,List<ServerSyncLog> sampleServerSyncLogs);

    }
}
