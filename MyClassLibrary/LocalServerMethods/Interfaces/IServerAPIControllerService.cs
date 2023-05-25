using System.Net;
using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    public interface IServerAPIControllerService<T> where T : LocalServerModelUpdate
    {
        Task<(HttpStatusCode statusCode, string result)> Get(string ids);
        Task<(HttpStatusCode statusCode, string result)> GetChanges(DateTime lastSyncDate);
        Task<(HttpStatusCode statusCode, string result)> PostConflicts(List<Conflict> conflicts);
        Task<(HttpStatusCode statusCode, string result)> PostUpdates(List<T> updates);
    }
}