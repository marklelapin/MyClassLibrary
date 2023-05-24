using Microsoft.AspNetCore.Mvc;
using MyExtensions;
using System.Text.Json;
using System.Net;
using MyClassLibrary.ErrorHandling;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace MyClassLibrary.LocalServerMethods
{
    public class ServerAPIControllerService<T> : IServerAPIControllerService<T> where T : LocalServerIdentityUpdate
    {

        private readonly IServerDataAccess _serverDataAccess;
        private readonly ILogger<T> _logger;

        public ServerAPIControllerService(IServerDataAccess serverDataAccess,ILogger<T> logger)
        {
            _serverDataAccess = serverDataAccess;
            _logger = logger;
        }

        public async Task<(HttpStatusCode statusCode, string result)> Get(string ids)
        {
            try
            {
                List<Guid> guids = ids.ToListGuid();

                List<T> updates = await _serverDataAccess.GetFromServer<T>(guids);

                if (updates.Count == 0)
                {
                    return (HttpStatusCode.NotFound, "[]");
                }

                string output = JsonSerializer.Serialize(updates);

                return (HttpStatusCode.OK, output);
            }
            catch (Exception ex)
            {
                return APIErrorResponse(ex);
            }


        }

        public async Task<(HttpStatusCode statusCode, string result)> GetChanges(DateTime lastSyncDate)
        {
            try
            {
                (List<T> updates, DateTime lastUpdatedOnServer) = await _serverDataAccess.GetChangesFromServer<T>(lastSyncDate);

                if (updates.Count == 0)
                {
                    return (HttpStatusCode.NotFound, "[]");
                }

                string output = JsonSerializer.Serialize(updates);

                return (HttpStatusCode.OK, output);
            }
            catch (Exception ex)
            {
                return APIErrorResponse(ex);
            }

        }


        public async Task<(HttpStatusCode statusCode, string result)> PostUpdates(List<T> updates)
        {
            try
            {
                DateTime result;

                result = await _serverDataAccess.SaveToServer(updates);

                if (result == DateTime.MinValue)
                {
                    return (HttpStatusCode.BadRequest, "Post failed to save update to server - check values in request body.");
                }

                string resultJson = JsonSerializer.Serialize(result);

                return (HttpStatusCode.OK, resultJson);
                ;
            }
            catch (Exception ex)
            {
                return APIErrorResponse(ex);
            }

        }

        public async Task<(HttpStatusCode statusCode, string result)> PostConflicts(List<Conflict> conflicts)
        {
            try
            {
               await _serverDataAccess.SaveConflictIdsToServer<T>(conflicts);

                return (HttpStatusCode.OK, "Conflict Successfully Posted.");
            }
            catch (Exception ex)
            {
                return APIErrorResponse(ex);
            };

        }

        private (HttpStatusCode statusCode, string result) APIErrorResponse(Exception ex)
        {
            ApiErrorResponse<T> error = new ApiErrorResponse<T>(ex,_logger);
            return (error.StatusCode, error.Body);
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
