using System.Net;
using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    /// <summary>
    /// A set of generic methods to be used for each update type for server data access in an api controller.
    /// </summary>
    /// <remarks>
    /// This allows for one set of code that can be applied across multiple different types of model types in the LocalServerModel system.
    /// </remarks>
    public interface IServerAPIControllerService<T> where T : ILocalServerModelUpdate
    {

        Task<(HttpStatusCode statusCode, string result)> GetUpdates(string ids,bool latestOnly = false);
        /// <summary>
        /// Returns all updates from the server that haven't been synced to the specific local copy (identified by localCopyId)
        /// </summary>
        Task<(HttpStatusCode statusCode, string result)> GetUnsyncedUpdates(Guid localCopyId);
        /// <summary>
        /// Returns any updates that are conflicted with the latest update of the model identified by the id(s) given.
        /// </summary>
        Task<(HttpStatusCode statusCode, string result)> GetConflictedUpdates(string ids);
        /// <summary>
        /// Saves update to server storage and returns ServerToLocalPostBack info with updated on server date and details of any conflicts found in string result.
        /// </summary>
        Task<(HttpStatusCode statusCode, string result)> PostUpdates(List<T> updates,Guid localCopyId);
        /// <summary>
        /// Updates server storage with LocalToServerPostBack information to confirm a full sync has occured.
        /// </summary>
        Task<(HttpStatusCode statusCode, string result)> PutPostBackToServer(List<LocalToServerPostBack> postBacks,Guid localCopyId);
        /// <summary>
        /// Sets IsConflicted = false for all updates relating to the ids passed in.
        /// </summary>
        Task<(HttpStatusCode statusCode, string result)> PutClearConflicts(string ids);



      //  Task<(HttpStatusCode statusCode, string result)> PostDeleteUpdates(List<T> updates);
    }
}