using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;
using MyClassLibrary.DataAccessMethods;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Models
{



    public class LocalSQLConnector<T> : ILocalDataAccess<T> where T : LocalServerModelUpdate 
    {

        private readonly ISqlDataAccess _dataAccess;
        private readonly string _connectionStringName;

        public LocalSQLConnector(ISqlDataAccess dataAccess,string? overrideConnectionStringName = null) //override added for testing of sync with failed connection
        {
            _dataAccess = dataAccess;
            _connectionStringName = overrideConnectionStringName ?? "LocalSQL";

        }


        public async Task<DateTime> GetLocalLastSyncDate()
        {
            DateTime output;
            
            var parameters = new DynamicParameters();

            parameters.Add("@UpdateType", typeof(T).Name, DbType.String,ParameterDirection.Input);
            parameters.Add("@LastSyncDate", null, DbType.DateTime2, ParameterDirection.Output);

            await _dataAccess.ExecuteStoredProcedure("spGetLocalLastSyncDate", parameters, _connectionStringName);
            

            output = parameters.Get<DateTime>("@LastSyncDate");

            return output;

        }


        public async Task SaveLocalLastSyncDate(DateTime lastSyncDate)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@UpdateType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@LastSyncDate", lastSyncDate, DbType.DateTime2, ParameterDirection.Input);

           await _dataAccess.ExecuteStoredProcedure("spSaveLocalLastSyncDate",parameters, _connectionStringName);
            
        }

        public async Task SaveToLocal(List<T> updates)
        {
            var parameters = new DynamicParameters();
            string jsonUpdates = JsonSerializer.Serialize(updates);
            parameters.Add("@UpdateType",typeof(T).Name,DbType.String,ParameterDirection.Input);
            parameters.Add("@Updates",jsonUpdates,DbType.String, ParameterDirection.Input);

            await _dataAccess.ExecuteStoredProcedure("spSaveToLocal",parameters, _connectionStringName);

        }

        public async Task<List<T>> GetFromLocal(List<Guid>? ids = null)
        {
            List<T> output;
            
            var parameters = new DynamicParameters();

            string idsCSV = String.Empty;

            if (ids != null)
            {
                idsCSV = String.Join(",", ids.Select(x => x.ToString()));
            }

            parameters.Add("@UpdateType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateIds", idsCSV, DbType.String, ParameterDirection.Input);
            parameters.Add("@Output",null,DbType.String, ParameterDirection.Output,size:int.MaxValue);

            await _dataAccess.ExecuteStoredProcedure("spGetFromLocal", parameters, _connectionStringName);

            string spOutput = parameters.Get<string>("@Output") ?? "[]";

            output = JsonSerializer.Deserialize<List<T>>(spOutput) ?? new List<T>() ;
           
            return output;
        }

        public async Task<List<T>> GetChangesFromLocal()
        {            
            List<T> output;

            var parameters = new DynamicParameters();

            parameters.Add("@UpdateType", typeof(T).Name,DbType.String, ParameterDirection.Input);
            parameters.Add("@Output",null,DbType.String, ParameterDirection.Output,size:int.MaxValue);

            await _dataAccess.ExecuteStoredProcedure("spGetChangesFromLocal",parameters, _connectionStringName);

            string spOutput = parameters.Get<string>("@Output");

            if (spOutput != null)
            {
                output = JsonSerializer.Deserialize<List<T>>(spOutput) ?? new List<T>();
            } else
            {
               output =  new List<T>() ; 
            }
                   
            return output;

        }

        public async Task SaveUpdatedOnServerToLocal(List<T> objects, DateTime updatedOnServer)
        {
            var parameters = new DynamicParameters();
            string jsonObjects = JsonSerializer.Serialize(objects);
            parameters.Add("@Updates",jsonObjects,DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType",typeof(T).Name,DbType.String,ParameterDirection.Input);
            parameters.Add("@UpdatedOnServer", updatedOnServer, DbType.DateTime2, ParameterDirection.Input);

           await  _dataAccess.ExecuteStoredProcedure("spSaveUpdatedOnServerToLocal", parameters, _connectionStringName);
        }

        public async Task SaveConflictIdsToLocal(List<Conflict> conflicts)
        {
            string jsonConflicts = JsonSerializer.Serialize(conflicts);
            var parameters = new DynamicParameters();
            parameters.Add("@Conflicts",jsonConflicts,DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType",typeof(T).Name,DbType.String,ParameterDirection.Input);

            await _dataAccess.ExecuteStoredProcedure("spSaveConflictIdsToLocal", parameters, _connectionStringName);
        }

        public async Task DeleteFromLocal(List<T> objects)
        {
            string jsonObjects = JsonSerializer.Serialize(objects);
            var parameters = new DynamicParameters();
            parameters.Add("@Updates",jsonObjects,DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType", typeof(T).Name);

            await _dataAccess.ExecuteStoredProcedure("spDeleteFromLocal", parameters, _connectionStringName);
        }

       
    }
}
