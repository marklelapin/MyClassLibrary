using Dapper;

namespace MyClassLibrary.DataAccessMethods
{
    public interface ISqlDataAccess
    {
        public void ExecuteStoredProcedure(string storedProcedure, DynamicParameters parameters, string connectionStringName);
    }
}