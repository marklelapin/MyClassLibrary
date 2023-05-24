namespace MyClassLibrary.LocalServerMethods
{
    public interface ILocalServerEngine<T> where T : LocalServerIdentityUpdate
    {

        Task<List<T>> FilterLatest(List<T> updates);
        Task<List<Conflict>> FindConflicts(List<T> changesFromServer, List<T> changesFromLocal);




        /// <summary>
        /// Returns all updates for the sorted By id and created date.
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
        /// Saves an update to local and or server storage. 
        /// </summary>
        Task Save(T update);
        /// <summary>
        /// Saves a list of updates to local and or server storage.
        /// </summary>
        /// <param name="updates"></param>
        Task Save(List<T> updates);

        /// <summary>
        /// Sorts a list of updates by created date.
        /// </summary>
        /// <param name="updates"></param>
        Task SortByCreated(List<T> updates);
        /// <summary>
        /// Sorts a list of updates by id.
        /// </summary>
        /// <param name="updates"></param>
        Task SortById(List<T> updates);
        /// <summary>
        /// Sorts a list of updates by id and then created date.
        /// </summary>
        /// <param name="updates"></param>
        Task SortByIdAndCreated(List<T> updates);
        /// <summary>
        /// Attempts to sync the data between local and server storage. Returns false if failed.
        /// </summary>
        /// <returns></returns>
        Task<bool> TrySync();
        /// <summary>
        /// Saves a list of conflicts to local and or server storage.
        /// </summary>
        /// <param name="conflicts"></param>
        /// <returns></returns>
        Task<bool> SaveConflictIds(List<Conflict> conflicts);


    }
}