using System.Net;
using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    public interface IServerAPIControllerService<T> where T : ILocalServerModelUpdate
    {
        Task<(HttpStatusCode statusCode, string result)> GetUpdates(string ids,bool latestOnly = false);
        Task<(HttpStatusCode statusCode, string result)> GetUnsyncedUpdates(Guid localCopyId);
        Task<(HttpStatusCode statusCode, string result)> GetConflictedUpdates(List<Guid> ids);
        Task<(HttpStatusCode statusCode, string result)> PostUpdates(List<T> updates,Guid localCopyId);
        Task<(HttpStatusCode statusCode, string result)> PutPostBackToServer(List<LocalToServerPostBack> postBacks,Guid localCopyId);
        Task<(HttpStatusCode statusCode, string result)> PutClearConflicts(List<Guid> ids);
        Task<(HttpStatusCode statusCode, string result)> PostDeleteUpdates(List<T> updates);
    }
}