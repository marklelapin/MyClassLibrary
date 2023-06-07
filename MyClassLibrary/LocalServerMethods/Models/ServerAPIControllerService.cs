using Microsoft.AspNetCore.Mvc;
using MyExtensions;
using System.Text.Json;
using System.Net;
using MyClassLibrary.ErrorHandling;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using System.Data;

namespace MyClassLibrary.LocalServerMethods.Models
{
    public class ServerAPIControllerService<T> : IServerAPIControllerService<T> where T : ILocalServerModelUpdate
    {

        private readonly IServerDataAccess<T> _serverDataAccess;
        private readonly ILogger<T> _logger;

        public ServerAPIControllerService(IServerDataAccess<T> serverDataAccess,ILogger<T> logger)
        {
            _serverDataAccess = serverDataAccess;
            _logger = logger;
        }

        public async Task<(HttpStatusCode statusCode, string result)> GetUpdates(string? ids,bool latestOnly)
        {
            try
            {
                List<Guid>? guids;

                if ((ids ?? "all").ToLower() == "all")
                {
                    guids = null;
                } else
                {
                    guids = ids?.ToListGuid();
                }
                

                List<T> updates = await _serverDataAccess.GetUpdatesFromServer(guids,latestOnly);

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

        public async Task<(HttpStatusCode statusCode, string result)> GetUnsyncedUpdates(Guid copyId)
        {
            try
            {
                List<T> updates = await _serverDataAccess.GetUnsyncedFromServer(copyId);

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



        public async Task<(HttpStatusCode statusCode, string result)> GetConflictedUpdates(string? ids)
        {
            try
            {
                List<Guid>? guids;

                if ((ids ?? "all").ToLower() == "all")
                {
                    guids = null;
                }
                else
                {
                    guids = ids?.ToListGuid();
                }

                List<T> updates = await _serverDataAccess.GetConflictedUpdatesFromServer(guids);

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


        public async Task<(HttpStatusCode statusCode, string result)> PostUpdates(List<T> updates,Guid copyId)
        {
            try
            {
                List<ServerToLocalPostBack> result;

                result = await _serverDataAccess.SaveUpdatesToServer(updates,copyId);

                string resultJson = JsonSerializer.Serialize(result);

                return (HttpStatusCode.OK, resultJson);
            }
            catch (Exception ex)
            {
                return APIErrorResponse(ex);
            }

        }

        public async Task<(HttpStatusCode statusCode,string result)> PutPostBackToServer(List<LocalToServerPostBack> postBacks,Guid copyID)
        {
            try
            {
               await _serverDataAccess.LocalPostBackToServer(postBacks,copyID);

                return (HttpStatusCode.OK, "Post Back from Local to Server was Successful.");
            }
            catch (Exception ex)
            {
                return APIErrorResponse(ex);
            };

        }


        public async Task<(HttpStatusCode statusCode, string result)> PutClearConflicts(string ids)
        {
            List<Guid>? guids;
            try
            {
                guids = ids.ToListGuid();
            }
            catch (Exception ex)
            {
                return APIErrorResponse(ex);
            }
            
            try
            {
                await _serverDataAccess.ClearConflictsFromServer(guids);

                return (HttpStatusCode.OK, "Conflicts successfully cleared.");
            }
            catch (Exception ex)
            {
                return APIErrorResponse(ex);
            };
        }


        //public async Task<(HttpStatusCode statusCode, string result)> PostDeleteUpdates(List<T> updates)
        //{
        //    try
        //    {
        //        await _serverDataAccess.DeleteFromServer(updates);

        //        return (HttpStatusCode.OK, "Updates successfully deleted.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return APIErrorResponse(ex);
        //    };

        //}








        private (HttpStatusCode statusCode, string result) APIErrorResponse(Exception ex)
        {
            ApiErrorResponse<T> error = new ApiErrorResponse<T>(ex,_logger);
            return (error.StatusCode, error.Body);
        }

        
    }
}
    



