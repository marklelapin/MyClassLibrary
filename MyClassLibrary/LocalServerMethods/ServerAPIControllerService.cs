using Microsoft.AspNetCore.Mvc;
using MyExtensions;
using System.Text.Json;

namespace MyClassLibrary.LocalServerMethods
{
    public class ServerAPIControllerService<T> : IServerAPIControllerService<T> where T : LocalServerIdentityUpdate
    {

        private readonly IServerDataAccess _serverDataAccess;

        public ServerAPIControllerService(IServerDataAccess serverDataAccess)
        {
            _serverDataAccess = serverDataAccess;
        }

        public string Get(string ids)
        {
            List<Guid> guids = ids.ToListGuid();

            List<T> partUpdates = _serverDataAccess.GetFromServer<T>(guids);

            string output = JsonSerializer.Serialize(partUpdates);

            return output;
        }

        public string GetChanges(DateTime lastSyncDate)
        {
            (List<T> updates, DateTime lastUpdatedOnServer) = _serverDataAccess.GetChangesFromServer<T>(lastSyncDate);

            string output = JsonSerializer.Serialize(updates);

            return output;
        }

        public DateTime PostUpdates(List<T> updates)
        {
            DateTime result;

            result = _serverDataAccess.SaveToServer(updates);

            return result;

        }

        public void PostConflicts(List<Conflict> conflicts)
        {
            _serverDataAccess.SaveConflictIdsToServer<T>(conflicts);

        }

        ////// DELETE api/Part/
        ////[HttpDelete("{updates}")]
        ////public void Delete([FromBody] string updates)
        ////{

        ////    List<PartUpdate> partUpdates = JsonSerializer.Deserialize<List<PartUpdate>>(updates) ?? new List<PartUpdate>();

        ////    _serverDataAccess.DeleteFromServer(partUpdates);
        ////}
    }
}
