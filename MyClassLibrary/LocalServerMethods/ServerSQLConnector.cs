
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.ComponentModel;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;
using MyClassLibrary.DataAccessMethods;

namespace MyClassLibrary.LocalServerMethods
{
    public class ServerSQLConnector : IServerDataAccess
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly string _connectionStringName;

        public ServerSQLConnector(ISqlDataAccess dataAccess, string? overrideConnectionStringName = null) //override added for testing of sync with failed connection)
        {
            _dataAccess = dataAccess;
            _connectionStringName = overrideConnectionStringName ?? "ServerSQL";
        }



        /// <summary>
        /// Saves Objects Data that inherit from LocalServerIdentity into Server Storage. Pass back UpdatedOnServer date as return value but also updates List of Objects.
        /// </summary>
        public DateTime SaveToServer<T>(List<T> updates) where T : LocalServerIdentityUpdate
        
        {
            var parameters = new DynamicParameters();

            //var opt = new JsonSerializerOptions() { WriteIndented = true };
            string jsonUpdates = JsonConvert.SerializeObject(updates);

            parameters.Add("@Updates", jsonUpdates, DbType.AnsiString,ParameterDirection.Input);
            parameters.Add("@UpdateType", typeof(T).Name, DbType.AnsiString,ParameterDirection.Input);
            parameters.Add("@UpdatedOnServer", null, DbType.DateTime2, ParameterDirection.Output);

            _dataAccess.ExecuteStoredProcedure("spSaveToServer", parameters,_connectionStringName);
            

            DateTime output = parameters.Get<DateTime>("@UpdatedOnServer");

            foreach(T obj in updates)
            {
                obj.UpdatedOnServer = output;
            }

            return output;
        }

        /// <summary>
        /// Finds all objects on the Server where the UpdatedOnServer date is later than lastSyncDate
        /// </summary>
        public (List<T> changesFromServer,DateTime lastUpdatedOnServer) GetChangesFromServer<T>( DateTime lastSyncDate) where T : LocalServerIdentityUpdate
        {   
            List<T> output = new List<T>();

            var parameters = new DynamicParameters();

            parameters.Add("@LastSyncDate",lastSyncDate,DbType.DateTime2,ParameterDirection.Input);
            parameters.Add("@UpdateType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@Output","", DbType.String, ParameterDirection.Output,size:int.MaxValue);

            _dataAccess.ExecuteStoredProcedure("spGetChangesFromServer", parameters,_connectionStringName);
            
            string spOutput = parameters.Get<string>("@Output");

            if (spOutput != null)
            {
                output = JsonConvert.DeserializeObject<List<T>>(spOutput) ?? new List<T>();
            }
            
            DateTime lastUpdatedOutput = (output ?? new List<T>()).Max(x => x.UpdatedOnServer) ?? DateTime.MinValue;
            
       
            return (changesFromServer: output, lastUpdatedOnServer: lastUpdatedOutput);
        }


        public List<T> GetFromServer<T>(List<Guid>? ids = null) where T : LocalServerIdentityUpdate
        {
            List<T> output;

            string idsCSV = string.Empty;

            if (ids != null) {
                idsCSV = String.Join(",",ids.Select(x => x.ToString()));
            }

            var parameters = new DynamicParameters();
            
            parameters.Add("@UpdateType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateIds",idsCSV,DbType.String, ParameterDirection.Input);
            parameters.Add("@Output","",DbType.String, ParameterDirection.Output,size:int.MaxValue);

            _dataAccess.ExecuteStoredProcedure("spGetFromServer",parameters,_connectionStringName);  
            
            string spOutput = parameters.Get<string>("@Output");

            if (spOutput != null)
            {
                output = JsonConvert.DeserializeObject<List<T>>(spOutput) ?? new List<T>();
            } else {
                output = new List<T>(); 
            }
            //for testing
            
            string jsonOutput = JsonConvert.SerializeObject(output);

            return output;

        }

        public void SaveConflictIdsToServer<T>(List<Conflict> conflicts) where T : LocalServerIdentityUpdate
        {
            string jsonConflicts = JsonConvert.SerializeObject(conflicts);
            var parameters = new DynamicParameters();

            parameters.Add("@Conflicts",jsonConflicts, DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType",typeof(T).Name,DbType.String, ParameterDirection.Input);

            _dataAccess.ExecuteStoredProcedure("spSaveConflictIdsToServer", parameters,_connectionStringName);
        }

        public void DeleteFromServer<T>(List<T> objects) where T : LocalServerIdentityUpdate
        {
            string jsonObjects = JsonConvert.SerializeObject(objects);
            var parameters = new DynamicParameters();
            parameters.Add("@Updates",jsonObjects, DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdateType", typeof(T).Name, DbType.String,ParameterDirection.Input);

            _dataAccess.ExecuteStoredProcedure("spDeleteFromServer",parameters,_connectionStringName);
        }


    }
}
