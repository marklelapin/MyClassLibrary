
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.ComponentModel;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;

namespace MyClassLibrary.LocalServerMethods
{
    public class ServerSQLConnector : IServerDataAccess
    {

        public string ConnectionString { get; set; }

        public ServerSQLConnector(string connectionString) 
        {
            ConnectionString = connectionString;
        }
         


        /// <summary>
        /// Saves Objects Data that inherit from LocalServerIdentity into Server Storage. Pass back UpdatedOnServer date as return value but also updates List of Objects.
        /// </summary>
        public DateTime SaveToServer<T>(List<T> objects) where T : LocalServerIdentityUpdate
        
        {
            var parameters = new DynamicParameters();

            //var opt = new JsonSerializerOptions() { WriteIndented = true };
            string jsonObjects = JsonSerializer.Serialize<List<T>>(objects);

            parameters.Add("@Objects", jsonObjects, DbType.AnsiString,ParameterDirection.Input);
            parameters.Add("@ObjectType", typeof(T).Name, DbType.AnsiString,ParameterDirection.Input);
            parameters.Add("@UpdatedOnServer", null, DbType.DateTime2, ParameterDirection.Output);

            ExecuteStoredProcedure("spSaveToServer", parameters);
            

            DateTime output = parameters.Get<DateTime>("@UpdatedOnServer");

            foreach(T obj in objects)
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
            parameters.Add("@ObjectType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@Output","", DbType.String, ParameterDirection.Output,size:int.MaxValue);

            ExecuteStoredProcedure("spGetChangesFromServer", parameters);
            
            string spOutput = parameters.Get<string>("@Output");

            if (spOutput != null)
            {
                output = JsonSerializer.Deserialize<List<T>>(spOutput) ?? new List<T>();
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
            
            parameters.Add("@ObjectType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@ObjectIds",idsCSV,DbType.String, ParameterDirection.Input);
            parameters.Add("@Output","",DbType.String, ParameterDirection.Output,size:int.MaxValue);

            ExecuteStoredProcedure("spGetFromServer",parameters);  
            
            string spOutput = parameters.Get<string>("@Output");

            if (spOutput != null)
            {
                output = JsonSerializer.Deserialize<List<T>>(spOutput) ?? new List<T>();
            } else {
                output = new List<T>(); 
            }
            //for testing
            
            string jsonOutput = JsonSerializer.Serialize<List<T>>(output);

            return output;

        }

        public void SaveConflictIdsToServer<T>(List<Conflict> conflicts) where T : LocalServerIdentityUpdate
        {
            string jsonConflicts = JsonSerializer.Serialize(conflicts);
            var parameters = new DynamicParameters();

            parameters.Add("@Conflicts",jsonConflicts, DbType.String, ParameterDirection.Input);
            parameters.Add("@ObjectType",typeof(T).Name,DbType.String, ParameterDirection.Input);

            ExecuteStoredProcedure("spSaveConflictIdsToServer", parameters);
        }

        public void DeleteFromServer<T>(List<T> objects) where T : LocalServerIdentityUpdate
        {
            string jsonObjects = JsonSerializer.Serialize(objects);
            var parameters = new DynamicParameters();
            parameters.Add("@Objects",jsonObjects, DbType.String, ParameterDirection.Input);
            parameters.Add("ObjectType", typeof(T).Name, DbType.String,ParameterDirection.Input);

            ExecuteStoredProcedure("spDeleteFromServer",parameters);
        }


        private void ExecuteStoredProcedure(string storedProcedure, DynamicParameters parameters)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }


    }
}
