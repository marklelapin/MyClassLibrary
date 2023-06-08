
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.ComponentModel;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;
using MyClassLibrary.DataAccessMethods;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.LocalServerMethods.Extensions;
using Microsoft.AspNetCore.Components.Routing;

namespace MyClassLibrary.LocalServerMethods.Models
{
    public class ServerSQLConnector<T> : IServerDataAccess<T> where T : ILocalServerModelUpdate 
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly string _connectionStringName;

        private string UpdateType { get { return typeof(T).Name; } }

        public ServerSQLConnector(ISqlDataAccess dataAccess, string? overrideConnectionStringName = null) //override added for testing of sync with failed connection)
        {
            _dataAccess = dataAccess;
            _connectionStringName = overrideConnectionStringName ?? "ServerSQL";
        }


        public async Task<List<ServerToLocalPostBack>> SaveUpdatesToServer(List<T> updates,Guid localCopyID)
        
        {
            List<ServerToLocalPostBack> output;

            var parameters = new DynamicParameters();

            //var opt = new JsonSerializerOptions() { WriteIndented = true };
            string jsonUpdates = JsonSerializer.Serialize(updates);

            parameters.Add("@CopyID", localCopyID, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@Updates", jsonUpdates, DbType.String,ParameterDirection.Input);
            parameters.Add("@UpdateType", UpdateType, DbType.String,ParameterDirection.Input);
            parameters.Add("@PostBack", null, DbType.String, ParameterDirection.Output, size: int.MaxValue);

            await _dataAccess.ExecuteStoredProcedure("spSaveUpdatesToServer", parameters,_connectionStringName);
            

           string spOutput = parameters.Get<string>("@PostBack");

            output = JsonSerializer.Deserialize<List<ServerToLocalPostBack>>(spOutput) ?? new List<ServerToLocalPostBack>();

            return output;
        }

        public async Task<List<T>> GetUnsyncedFromServer(Guid localCopyId)
        {   
            List<T> output = new List<T>();

            var parameters = new DynamicParameters();

            parameters.Add("@CopyId",localCopyId,DbType.Guid,ParameterDirection.Input);
            parameters.Add("@UpdateType", UpdateType, DbType.String, ParameterDirection.Input);
            parameters.Add("@Output",null, DbType.String, ParameterDirection.Output, size: int.MaxValue);

            await _dataAccess.ExecuteStoredProcedure("spGetUnsyncedFromServer", parameters,_connectionStringName);
            
            string spOutput = parameters.Get<string>("@Output");

            output = spOutput.ConvertSQLJsonUpdateToUpdate<T>();

            return output;
        }

        public async Task<List<T>> GetConflictedUpdatesFromServer(List<Guid>? ids = null)
        {
            List<T> output = new List<T>();

            string? idsCSV = null;

            if (ids != null)
            {
                idsCSV = string.Join(",", ids.Select(i => i.ToString()));
            }

            var parameters = new DynamicParameters();

            parameters.Add("@UpdateIds", idsCSV, DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType", UpdateType, DbType.String, ParameterDirection.Input);
            parameters.Add("@Output", null, DbType.String, ParameterDirection.Output, size: int.MaxValue);

            await _dataAccess.ExecuteStoredProcedure("spGetConflictedUpdates", parameters, _connectionStringName);

            string spOutput = parameters.Get<string>("@Output");

            output = spOutput.ConvertSQLJsonUpdateToUpdate<T>();

            return output;

        }

        public async Task<List<T>> GetUpdatesFromServer(List<Guid>? ids = null,bool latestOnly = false)
        {
            List<T> output = new List<T>();

            string idsCSV = string.Empty;

            if (ids != null)
            {
                idsCSV = String.Join(",", ids.Select(x => x.ToString()));
            }

            var parameters = new DynamicParameters();
            
            parameters.Add("@LatestOnly", latestOnly,DbType.Boolean,ParameterDirection.Input);
            parameters.Add("@UpdateType", UpdateType, DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateIds",idsCSV,DbType.String, ParameterDirection.Input);
            parameters.Add("@Output","",DbType.String, ParameterDirection.Output, size: int.MaxValue);

            await _dataAccess.ExecuteStoredProcedure("spGetUpdates",parameters,_connectionStringName);  
            
            string spOutput = parameters.Get<string>("@Output");

            output = spOutput.ConvertSQLJsonUpdateToUpdate<T>();

            return output;

        }

        public async Task LocalPostBackToServer(List<LocalToServerPostBack> postBacks,Guid localCopyId)
        {
            string jsonUpdates = JsonSerializer.Serialize(postBacks);
            var parameters = new DynamicParameters();

            parameters.Add("@CopyId", localCopyId, DbType.Guid, ParameterDirection.Input);
            parameters.Add("@PostBack",jsonUpdates, DbType.String, ParameterDirection.Input);

           await _dataAccess.ExecuteStoredProcedure("spLocalPostBackToServer", parameters,_connectionStringName);
        }

        public async Task DeleteFromServer(List<T> objects)
        {
            string jsonObjects = JsonSerializer.Serialize(objects);
            var parameters = new DynamicParameters();
            parameters.Add("@Updates",jsonObjects, DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType", UpdateType, DbType.String,ParameterDirection.Input);

            await _dataAccess.ExecuteStoredProcedure("spDeleteUpdates",parameters,_connectionStringName);
        }

        public async Task ClearConflictsFromServer(List<Guid> ids)
        {
            string idsCSV = string.Join(",", ids.Select(x=>x.ToString()));

            var parameters = new DynamicParameters();

            parameters.Add("@Ids",idsCSV, DbType.String, ParameterDirection.Input);

            await _dataAccess.ExecuteStoredProcedure("spClearConflicts",parameters, _connectionStringName);
        }

        public async Task<bool> ResetSampleData(List<T> sampleUpdates,List<ServerSyncLog> sampleServerSyncLogs)
        {

            string jsonSampleUpdates = JsonSerializer.Serialize(sampleUpdates);
            string jsonSampleServerSyncLogs = JsonSerializer.Serialize(sampleServerSyncLogs);   


            var parameters = new DynamicParameters();

            parameters.Add("@Location", "Server", DbType.String, ParameterDirection.Input);
            parameters.Add("@SampleUpdates",jsonSampleUpdates, DbType.String, ParameterDirection.Input);
            parameters.Add("@SampleServerSyncLogs",jsonSampleServerSyncLogs, DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType",UpdateType,DbType.String, ParameterDirection.Input);
            parameters.Add("@Success", false, DbType.Boolean, ParameterDirection.Output);
            await _dataAccess.ExecuteStoredProcedure("spResetSampleData", parameters, _connectionStringName);

            return parameters.Get<bool>("@Success");
        }
    }
}

