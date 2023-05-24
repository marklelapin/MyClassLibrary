using Dapper;

namespace MyClassLibrary.DataAccessMethods
{
    public interface ISqlDataAccess
    {
        public Task ExecuteStoredProcedure(string storedProcedure, DynamicParameters parameters, string connectionStringName);
    }
}