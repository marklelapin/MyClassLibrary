using System.Net;

namespace MyClassLibrary.LocalServerMethods
{
    public interface IServerAPIControllerService<T> where T : LocalServerIdentityUpdate
    {
        (HttpStatusCode statusCode,string result) Get(string ids);
        (HttpStatusCode statusCode, string result) GetChanges(DateTime lastSyncDate);
        (HttpStatusCode statusCode, string result) PostConflicts(List<Conflict> conflicts);
        (HttpStatusCode statusCode, string result) PostUpdates(List<T> updates);
    }
}