using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace MyClassLibrary.DataAccessMethods
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;
        private readonly string? _overrideConnectionStringName;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public void ExecuteStoredProcedure(string storedProcedure, DynamicParameters parameters, string connectionStringName)
        {
            
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionStringName)))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        //public async Task<List<T>> Query<T,U>(string storedProcedure,U parameters,string connectionStringName)
        //{
        //    using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionStringName)))
        //    {
        //        var rows = await connection.QueryAsync<T>(storedProcedure,parameters,commandType: CommandType.StoredProcedure);

        //        return rows.ToList();
        //    }


        //}

    }

}
