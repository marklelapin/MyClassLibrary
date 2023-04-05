﻿
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
    public class ServerSQLConnector : IServerDataAccess
    {

        public string ConnectionString { get; set; }

        public ServerSQLConnector(string connectionString) 
        {
            ConnectionString = connectionString;
        }
             
        /// <summary>
        /// Saves Objects Data that inherit from LocalServerIdentity into Server Storage. Pass back UpdatedOnServer date to objects.
        /// </summary>
        public void SaveToServer<T>(List<T> objects) where T : LocalServerIdentity
        
        {
            var parameters = new DynamicParameters();

            
            var opt = new JsonSerializerOptions() { WriteIndented = true };
            parameters.Add("@Objects", JsonSerializer.Serialize<List<T>>(objects, opt),DbType.String,ParameterDirection.Input);
            parameters.Add("@ObjectType", typeof(T).Name, DbType.String,ParameterDirection.Input);
            parameters.Add("@UpdatedOnServer", null, DbType.DateTime, ParameterDirection.Output);

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
        public List<T> LoadChangesFromServer<T>(DateTime lastSyncDate) where T : LocalServerIdentity
        {   
            List<T> output;

            var parameters = new DynamicParameters();

            parameters.Add("@LastSyncDate",lastSyncDate,DbType.DateTime,ParameterDirection.Input);
            parameters.Add("@ObjectType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@JsonOutput","", DbType.String, ParameterDirection.Output);

            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("spLoadChangesFromServer", parameters, commandType: CommandType.StoredProcedure);
            }

           output =  JsonSerializer.Deserialize<List<T>>(parameters.Get<string>("@JsonOutput")) ?? new List<T>();

            return output;
        }



        /// <summary>
        /// Saves a local storage path to the server against the user and device if not using local browser storage.
        /// </summary>
        public void saveLocalStoragePathToServer(string path) 
        {
            throw new NotImplementedException();
        }

    }
}