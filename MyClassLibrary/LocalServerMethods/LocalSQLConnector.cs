using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

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

            parameters.Add("@ObjectType", typeof(T).Name, DbType.String,ParameterDirection.Input);
            parameters.Add("@LastSyncDate", null, DbType.DateTime2, ParameterDirection.Output);

            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("spGetLocalLastSyncDate", parameters, commandType: CommandType.StoredProcedure);
            }

            output = parameters.Get<DateTime>("@LastSyncDate");

            return output;

        }


        public void SaveLocalLastSyncDate<T>(DateTime lastSyncDate)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@ObjectType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@LastSyncDate", lastSyncDate, DbType.DateTime2, ParameterDirection.Input);

            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("spSaveLocalLastSyncDate",parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void SaveToLocal<T>(List<T> objects) where T : LocalServerIdentity
        {
            var parameters = new DynamicParameters();
            string jsonObjects = JsonSerializer.Serialize(objects);
            parameters.Add("@ObjectType",typeof(T).Name,DbType.String,ParameterDirection.Input);
            parameters.Add("@Objects",jsonObjects,DbType.String, ParameterDirection.Input);

            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("spSaveToLocal",parameters,commandType: CommandType.StoredProcedure);
            }

        }

        public List<T> GetFromLocal<T>(List<Guid>? ids = null) where T : LocalServerIdentity
        {
            List<T> output;
            
            var parameters = new DynamicParameters();

            string idsCSV = String.Empty;

            if (ids != null)
            {
                idsCSV = String.Join(",", ids.Select(x => x.ToString()));
            }

            parameters.Add("@ObjectType", typeof(T).Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@ObjectIds", idsCSV, DbType.String, ParameterDirection.Input);
            parameters.Add("@Output",null,DbType.String, ParameterDirection.Output,size:int.MaxValue);

            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("spGetFromLocal", parameters, commandType: CommandType.StoredProcedure);
            }

            string spOutput = parameters.Get<string>("@Output") ?? "[]";

            output = JsonSerializer.Deserialize<List<T>>(spOutput) ?? new List<T>() ;
           
            return output;
        }

        public List<T> GetChangesFromLocal<T>() where T : LocalServerIdentity
        {            
            List<T> output;

            var parameters = new DynamicParameters();

            parameters.Add("@ObjectType", typeof(T).Name,DbType.String, ParameterDirection.Input);
            parameters.Add("@Output",null,DbType.String, ParameterDirection.Output,size:int.MaxValue);

            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("spGetChangesFromLocal",parameters,commandType: CommandType.StoredProcedure);
            }

            output = JsonSerializer.Deserialize<List<T>>(parameters.Get<string>("@Output")) ?? new List<T>();

            return output;

        }

        public void SaveUpdatedOnServerDate<T>(List<T> objects, DateTime updatedOnServer) where T : LocalServerIdentity
        {
            var parameters = new DynamicParameters();
            string jsonObjects = JsonSerializer.Serialize(objects);
            parameters.Add("@Objects",jsonObjects,DbType.String, ParameterDirection.Input);
            parameters.Add("@UpdatedOnServer", updatedOnServer, DbType.DateTime2, ParameterDirection.Input);

            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Execute("spSaveUpdatedOnServerToLocal",parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
