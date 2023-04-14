
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
        /// Saves a local storage path to the server against the user and device if not using local browser storage.
        /// </summary>
          public void SaveLocalStoragePathToServer(string path)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@LocalStoragePath", path, DbType.String, ParameterDirection.Input);

                using (IDbConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Execute("spSaveLocalStoragePathToServer", parameters, commandType: CommandType.StoredProcedure);
                    };
            }    



        /// <summary>
        /// Saves Objects Data that inherit from LocalServerIdentity into Server Storage. Pass back UpdatedOnServer date to objects.
        /// </summary>
        public void SaveToServer<T>(List<T> objects) where T : LocalServerIdentity
        
        {
            var parameters = new DynamicParameters();

            //var opt = new JsonSerializerOptions() { WriteIndented = true };
            string jsonObjects = JsonSerializer.Serialize<List<T>>(objects);

            parameters.Add("@Objects", jsonObjects, DbType.AnsiString,ParameterDirection.Input);
            parameters.Add("@ObjectType", typeof(T).Name, DbType.AnsiString,ParameterDirection.Input);
            parameters.Add("@UpdatedOnServer", null, DbType.DateTime2, ParameterDirection.Output);

            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("spSaveToServer", parameters, commandType: CommandType.StoredProcedure);
            };

            foreach (LocalServerIdentity obj in objects)
            {
                obj.UpdatedOnServer = parameters.Get<DateTime>("@UpdatedOnServer");
            }
        }

        /// <summary>
        /// Finds all objects on the Server where the UpdatedOnServer date is later than lastSyncDate
        /// </summary>
        public List<T> GetChangesFromServer<T>( DateTime lastSyncDate) where T : LocalServerIdentity
        {   
            List<T> output;

            var parameters = new DynamicParameters();

            parameters.Add("@LastSyncDate",lastSyncDate,DbType.DateTime2,ParameterDirection.Input);
            parameters.Add("@ObjectType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@Output","", DbType.String, ParameterDirection.Output,size:int.MaxValue);

            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("spGetChangesFromServer", parameters, commandType: CommandType.StoredProcedure);
            }
            string spOutput = parameters.Get<string>("@Output");
            
            if (spOutput != null )
            { 
                output = JsonSerializer.Deserialize<List<T>>(spOutput);
            } else
            {
                output = new List<T>();
            };

       
            return output;
        }


        public List<T> GetFromServer<T>(List<Guid>? ids = null, bool IsActive = true) where T : LocalServerIdentity
        {
            List<T> output;

            string idsCSV = string.Empty;

            if (ids != null) {
                idsCSV = String.Join(",",ids.Select(x => x.ToString()));
            }

            var parameters = new DynamicParameters();
            
            parameters.Add("@ObjectType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@ObjectIds",idsCSV,DbType.String, ParameterDirection.Input);
            parameters.Add("@IsActive",IsActive, DbType.Boolean, ParameterDirection.Input);
            parameters.Add("@Output","",DbType.String, ParameterDirection.Output,size:int.MaxValue);

            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("spGetFromServer",parameters,commandType: CommandType.StoredProcedure);  
            }
            string spOutput = parameters.Get<string>("@Output");

            if (spOutput != null)
            {
                output = JsonSerializer.Deserialize<List<T>>(spOutput);
            } else {
                output = new List<T>(); 
            }
            //for testing:
            string jsonOutput = JsonSerializer.Serialize<List<T>>(output);

            return output;

        }



   
       
    }
}
