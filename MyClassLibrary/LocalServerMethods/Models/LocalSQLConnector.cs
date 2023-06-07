using Dapper;
using System.Data;
using System.Text.Json;
using MyClassLibrary.DataAccessMethods;
using MyClassLibrary.LocalServerMethods.Interfaces;
using Microsoft.IdentityModel.Abstractions;
using MyClassLibrary.LocalServerMethods.Extensions;

namespace MyClassLibrary.LocalServerMethods.Models
{



    public class LocalSQLConnector<T> : ILocalDataAccess<T> where T : ILocalServerModelUpdate 
    {

        private readonly ISqlDataAccess _dataAccess;
        private readonly string _connectionStringName;

        private string UpdateType {  get {return typeof(T).Name;}   }


        private Guid? CopyId { get; set; }

        public LocalSQLConnector(ISqlDataAccess dataAccess,string? overrideConnectionStringName = null) //override added for testing of sync with failed connection
        {
            _dataAccess = dataAccess;
            _connectionStringName = overrideConnectionStringName ?? "LocalSQL";
        }

        public async Task SaveLocalLastSyncDate(DateTime lastSyncDate)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@UpdateType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@LastSyncDate", lastSyncDate, DbType.DateTime2, ParameterDirection.Input);

           await _dataAccess.ExecuteStoredProcedure("spSaveLocalLastSyncDate",parameters, _connectionStringName); 
        }

        public async Task<List<LocalToServerPostBack>> SaveUpdatesToLocal(List<T> updates)
        {
            List<LocalToServerPostBack> output;

            var parameters = new DynamicParameters();
            string jsonUpdates = JsonSerializer.Serialize(updates);
            parameters.Add("@UpdateType",typeof(T).Name,DbType.String,ParameterDirection.Input);
            parameters.Add("@Updates",jsonUpdates,DbType.String, ParameterDirection.Input);
            parameters.Add("@PostBack", jsonUpdates, DbType.String, ParameterDirection.Output, size: int.MaxValue);

            await _dataAccess.ExecuteStoredProcedure("spSaveUpdatesToLocal",parameters, _connectionStringName);

            string spOutput = parameters.Get<string>("@PostBack") ?? "[]";

            output = JsonSerializer.Deserialize<List<LocalToServerPostBack>>(spOutput) ?? new List<LocalToServerPostBack>();

            return output;
;        }

        public async Task<List<T>> GetUpdatesFromLocal(List<Guid>? ids = null,bool latestOnly = false)
        {
            List<T> output  = new List<T>();
            
            var parameters = new DynamicParameters();

            string idsCSV = String.Empty;

            if (ids != null)
            {
                idsCSV = String.Join(",", ids.Select(x => x.ToString()));
            }
            parameters.Add("@LatestOnly",latestOnly,DbType.Boolean,ParameterDirection.Input);
            parameters.Add("@UpdateType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateIds", idsCSV, DbType.String, ParameterDirection.Input);
            parameters.Add("@Output",null,DbType.String, ParameterDirection.Output,size:int.MaxValue);

            await _dataAccess.ExecuteStoredProcedure("spGetUpdates", parameters, _connectionStringName);

            string spOutput = parameters.Get<string>("@Output") ?? "[]";

            output = spOutput.ConvertSQLJsonUpdateToUpdate<T>();

            return output;
        }

        public async Task<List<T>> GetUnsyncedFromLocal()
        {            
            List<T> output;

            var parameters = new DynamicParameters();

            parameters.Add("@UpdateType", typeof(T).Name,DbType.String, ParameterDirection.Input);
            parameters.Add("@Output",null,DbType.String, ParameterDirection.Output,size:int.MaxValue);

            await _dataAccess.ExecuteStoredProcedure("spGetUnsyncedFromLocal",parameters, _connectionStringName);

            string spOutput = parameters.Get<string>("@Output");

            output = spOutput.ConvertSQLJsonUpdateToUpdate<T>();

            return output;

        }

        public async Task<List<T>> GetConflictedUpdatesFromLocal(List<Guid>? ids = null)
        {
            //TODO Add Unit Testing for Get ConflictedUpdates

            List<T> output = new List<T>();

            string? idsCSV = null;

            if (ids != null)
            {
                idsCSV = string.Join(",",ids.Select(i => i.ToString()));    
            }

            var parameters = new DynamicParameters();

            parameters.Add("@UpdateIds",idsCSV,DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@Output", null, DbType.String, ParameterDirection.Output, size: int.MaxValue);

            await _dataAccess.ExecuteStoredProcedure("spGetConflictedUpdates", parameters, _connectionStringName);

            string spOutput = parameters.Get<string>("@Output");

            output = spOutput.ConvertSQLJsonUpdateToUpdate<T>();

            return output;

        }
        
        public async Task ServerPostBackToLocal(List<ServerToLocalPostBack> updates)
        {
            var parameters = new DynamicParameters();
            string jsonUpdates = JsonSerializer.Serialize(updates);
            parameters.Add("@PostBack",jsonUpdates,DbType.String, ParameterDirection.Input);

           await  _dataAccess.ExecuteStoredProcedure("spServerPostBackToLocal", parameters, _connectionStringName);
        }

        public async Task DeleteFromLocal(List<T> objects)
        {
            string jsonObjects = JsonSerializer.Serialize(objects);
            var parameters = new DynamicParameters();
            parameters.Add("@Updates",jsonObjects,DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType", typeof(T).Name);

            await _dataAccess.ExecuteStoredProcedure("spDeleteUpdates", parameters, _connectionStringName);
        }

        public async Task<DateTime> GetLocalLastSyncDate()
        {
            DateTime output;

            var parameters = new DynamicParameters();

            parameters.Add("@UpdateType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@LastSyncDate", null, DbType.DateTime2, ParameterDirection.Output);

            await _dataAccess.ExecuteStoredProcedure("spGetLocalLastSyncDate", parameters, _connectionStringName);


            output = parameters.Get<DateTime>("@LastSyncDate");

            return output;

        }

        public async Task<Guid> GetLocalCopyID()
        {
            if (CopyId == null)
            {

                Guid output;

                var parameters = new DynamicParameters();
                parameters.Add("@CopyID", null, DbType.Guid, ParameterDirection.Output);

                await _dataAccess.ExecuteStoredProcedure("spGetLocalCopyID", parameters, _connectionStringName);

                output = parameters.Get<Guid>("@CopyID");

                CopyId = output;

            }

            return (Guid)CopyId;
        }

        public async Task ClearConflictsFromLocal(List<Guid> ids)
        {
            string idsCSV = String.Join(",", ids.Select(x => x.ToString()));

            var parameters = new DynamicParameters();
            parameters.Add("@Ids",idsCSV, DbType.String, ParameterDirection.Input);

            await _dataAccess.ExecuteStoredProcedure("spClearConflicts",parameters,_connectionStringName);
        }

        public async Task<bool> ResetSampleData(List<T> sampleUpdates)
        {
            string jsonSampleUpdates = JsonSerializer.Serialize(sampleUpdates);

            var parameters = new DynamicParameters();

            parameters.Add("@Location","Local",DbType.String, ParameterDirection.Input);
            parameters.Add("@SampleUpdates",jsonSampleUpdates,DbType.String, ParameterDirection.Input);
            parameters.Add("@SampleServerSyncLogs",null,DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType", UpdateType, DbType.String, ParameterDirection.Input);
            parameters.Add("@Success", false, DbType.Boolean, ParameterDirection.Output);
            await _dataAccess.ExecuteStoredProcedure("spResetSampleData", parameters, _connectionStringName);

            return parameters.Get<bool>("@Success");
        }
    }
}
