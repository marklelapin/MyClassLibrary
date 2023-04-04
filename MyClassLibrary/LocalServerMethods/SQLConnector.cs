
using MyClassLibrary.LocalServerMethods;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.ComponentModel;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Runtime.CompilerServices;

namespace TheWhaddonShowClassLibrary.DataAccess
{
    public class SQLConnector : IServerDataAccess
    {
       //TODO = REMOVE AS SOON AS POSSIBLE
        private string _connectionString = "Data Source = (localdb)\\MSSQLLocalDB;Initial Catalog = TheWhaddonShowTestDB; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False"
        
        
        
        
        /// <summary>
        /// Saves Objects Data that inherit from LocalServerIdentity into Server Storage. Pass back UpdatedOnServer dat to objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SaveToServer<T>(List<T> objects) where T : LocalServerIdentity
        
        {
            var parameters = new DynamicParameters();

            
            var opt = new JsonSerializerOptions() { WriteIndented = true };
            parameters.Add("@Objects", JsonSerializer.Serialize<List<T>>(objects, opt),DbType.String,ParameterDirection.Input);
            parameters.Add("@ObjectType", typeof(T).Name, DbType.String,ParameterDirection.Input);
            parameters.Add("@UpdatedOnServer", null, DbType.DateTime, ParameterDirection.Output);

            using (IDbConnection connection = new SqlConnection(_connectionString))
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
        /// <typeparam name="T"></typeparam>
        /// <param name="lastSyncDate">The last time a successfult sync was carried out</param>
        public List<T> LoadChangesFromServer<T>(DateTime lastSyncDate) where T : LocalServerIdentity
        {   
            List<T> output;

            var parameters = new DynamicParameters();

            parameters.Add("@LastSyncDate",lastSyncDate,DbType.DateTime,ParameterDirection.Input);
            parameters.Add("@ObjectType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@JsonOutput","", DbType.String, ParameterDirection.Output);

            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Execute("spLoadChangesFromServer", parameters, commandType: CommandType.StoredProcedure);
            }

           output =  JsonSerializer.Deserialize<List<T>>(parameters.Get<string>("@JsonOutput")) ?? new List<T>();

            return output;
        }

        /// <summary>
        /// Saves a local storage path to the server against the user and device if not using local browser storage.
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void saveLocalStoragePathToServer(string path) 
        {
            throw new NotImplementedException();
        }

    }
}
