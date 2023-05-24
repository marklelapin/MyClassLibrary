using System.Net;

namespace MyClassLibrary.LocalServerMethods
{
    public interface IServerAPIControllerService<T> where T : LocalServerIdentityUpdate
    {
        Task<(HttpStatusCode statusCode,string result)> Get(string ids);
        Task<(HttpStatusCode statusCode, string result)> GetChanges(DateTime lastSyncDate);
        Task<(HttpStatusCode statusCode, string result)> PostConflicts(List<Conflict> conflicts);
        Task<(HttpStatusCode statusCode, string result)> PostUpdates(List<T> updates);
    }
}