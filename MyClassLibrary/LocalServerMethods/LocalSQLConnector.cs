using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace MyClassLibrary.LocalServerMethods
{



    public class LocalSQLConnector : ILocalDataAccess 
    {


        public string ConnectionString { get; set; }

        public LocalSQLConnector(string connectionString)
        {
            ConnectionString = connectionString;
        }


        public DateTime GetLocalLastSyncDate<T>()
        {
            DateTime output;
            
            var parameters = new DynamicParameters();

            parameters.Add("@UpdateType", typeof(T).Name, DbType.String,ParameterDirection.Input);
            parameters.Add("@LastSyncDate", null, DbType.DateTime2, ParameterDirection.Output);

            ExecuteStoredProcedure("spGetLocalLastSyncDate", parameters);
            

            output = parameters.Get<DateTime>("@LastSyncDate");

            return output;

        }


        public void SaveLocalLastSyncDate<T>(DateTime lastSyncDate)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@UpdateType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@LastSyncDate", lastSyncDate, DbType.DateTime2, ParameterDirection.Input);

           ExecuteStoredProcedure("spSaveLocalLastSyncDate",parameters);
            
        }

        public void SaveToLocal<T>(List<T> updates) where T : LocalServerIdentityUpdate
        {
            var parameters = new DynamicParameters();
            string jsonUpdates = JsonConvert.SerializeObject(updates);
            parameters.Add("@UpdateType",typeof(T).Name,DbType.String,ParameterDirection.Input);
            parameters.Add("@Updates",jsonUpdates,DbType.String, ParameterDirection.Input);

            ExecuteStoredProcedure("spSaveToLocal",parameters);

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

            ExecuteStoredProcedure("spGetFromLocal", parameters);

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

            ExecuteStoredProcedure("spGetChangesFromLocal",parameters);

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
            parameters.Add("@UpdatedOnServer", updatedOnServer, DbType.DateTime2, ParameterDirection.Input);

            ExecuteStoredProcedure("spSaveUpdatedOnServerToLocal", parameters);
        }

        public void SaveConflictIdsToLocal<T>(List<Conflict> conflicts) where T : LocalServerIdentityUpdate
        {
            string jsonConflicts = JsonConvert.SerializeObject(conflicts);
            var parameters = new DynamicParameters();
            parameters.Add("@Conflicts",jsonConflicts,DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType",typeof(T).Name,DbType.String,ParameterDirection.Input);

            ExecuteStoredProcedure("spSaveConflictIdsToLocal", parameters);
        }

        public void DeleteFromLocal<T>(List<T> objects) where T : LocalServerIdentityUpdate
        {
            string jsonObjects = JsonConvert.SerializeObject(objects);
            var parameters = new DynamicParameters();
            parameters.Add("@Updates",jsonObjects,DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType", typeof(T).Name);

            ExecuteStoredProcedure("spDeleteFromLocal", parameters);
        }

        private void ExecuteStoredProcedure(string storedProcedure,DynamicParameters parameters)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
