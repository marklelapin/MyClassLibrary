using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using MyClassLibrary.DataAccessMethods;

namespace MyClassLibrary.LocalServerMethods
{



    public class LocalSQLConnector : ILocalDataAccess 
    {

        private readonly ISqlDataAccess _dataAccess;
        private readonly string _connectionStringName;

        public LocalSQLConnector(ISqlDataAccess dataAccess,string? overrideConnectionStringName = null) //override added for testing of sync with failed connection
        {
            _dataAccess = dataAccess;
            _connectionStringName = overrideConnectionStringName ?? "LocalSQL";

        }


        public DateTime GetLocalLastSyncDate<T>()
        {
            DateTime output;
            
            var parameters = new DynamicParameters();

            parameters.Add("@UpdateType", typeof(T).Name, DbType.String,ParameterDirection.Input);
            parameters.Add("@LastSyncDate", null, DbType.DateTime2, ParameterDirection.Output);

            _dataAccess.ExecuteStoredProcedure("spGetLocalLastSyncDate", parameters, _connectionStringName);
            

            output = parameters.Get<DateTime>("@LastSyncDate");

            return output;

        }


        public void SaveLocalLastSyncDate<T>(DateTime lastSyncDate)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@UpdateType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@LastSyncDate", lastSyncDate, DbType.DateTime2, ParameterDirection.Input);

           _dataAccess.ExecuteStoredProcedure("spSaveLocalLastSyncDate",parameters, _connectionStringName);
            
        }

        public void SaveToLocal<T>(List<T> updates) where T : LocalServerIdentityUpdate
        {
            var parameters = new DynamicParameters();
            string jsonUpdates = JsonConvert.SerializeObject(updates);
            parameters.Add("@UpdateType",typeof(T).Name,DbType.String,ParameterDirection.Input);
            parameters.Add("@Updates",jsonUpdates,DbType.String, ParameterDirection.Input);

            _dataAccess.ExecuteStoredProcedure("spSaveToLocal",parameters, _connectionStringName);

        }

        public List<T> GetFromLocal<T>(List<Guid>? ids = null) where T : LocalServerIdentityUpdate
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

            _dataAccess.ExecuteStoredProcedure("spGetFromLocal", parameters, _connectionStringName);

            string spOutput = parameters.Get<string>("@Output") ?? "[]";

            output = JsonConvert.DeserializeObject<List<T>>(spOutput) ?? new List<T>() ;
           
            return output;
        }

        public List<T> GetChangesFromLocal<T>() where T : LocalServerIdentityUpdate
        {            
            List<T> output;

            var parameters = new DynamicParameters();

            parameters.Add("@UpdateType", typeof(T).Name,DbType.String, ParameterDirection.Input);
            parameters.Add("@Output",null,DbType.String, ParameterDirection.Output,size:int.MaxValue);

            _dataAccess.ExecuteStoredProcedure("spGetChangesFromLocal",parameters, _connectionStringName);

            string spOutput = parameters.Get<string>("@Output");

            if (spOutput != null)
            {
                output = JsonConvert.DeserializeObject<List<T>>(spOutput) ?? new List<T>();
            } else
            {
               output =  new List<T>() ; 
            }
                   
            return output;

        }

        public void SaveUpdatedOnServerToLocal<T>(List<T> objects, DateTime updatedOnServer) where T : LocalServerIdentityUpdate
        {
            var parameters = new DynamicParameters();
            string jsonObjects = JsonConvert.SerializeObject(objects);
            parameters.Add("@Updates",jsonObjects,DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType",typeof(T).Name,DbType.String,ParameterDirection.Input);
            parameters.Add("@UpdatedOnServer", updatedOnServer, DbType.DateTime2, ParameterDirection.Input);

            _dataAccess.ExecuteStoredProcedure("spSaveUpdatedOnServerToLocal", parameters, _connectionStringName);
        }

        public void SaveConflictIdsToLocal<T>(List<Conflict> conflicts) where T : LocalServerIdentityUpdate
        {
            string jsonConflicts = JsonConvert.SerializeObject(conflicts);
            var parameters = new DynamicParameters();
            parameters.Add("@Conflicts",jsonConflicts,DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType",typeof(T).Name,DbType.String,ParameterDirection.Input);

            _dataAccess.ExecuteStoredProcedure("spSaveConflictIdsToLocal", parameters, _connectionStringName);
        }

        public void DeleteFromLocal<T>(List<T> objects) where T : LocalServerIdentityUpdate
        {
            string jsonObjects = JsonConvert.SerializeObject(objects);
            var parameters = new DynamicParameters();
            parameters.Add("@Updates",jsonObjects,DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType", typeof(T).Name);

            _dataAccess.ExecuteStoredProcedure("spDeleteFromLocal", parameters, _connectionStringName);
        }

       
    }
}
