using MongoDB.Driver;

namespace MyClassLibrary.DataAccessMethods
{
    public interface IMongoDBDataAccess
    {
        /// <summary>
        /// Deletes a record from Collection asynchronously matching the given Document Id.
        /// </summary>
        /// <returns></returns>
        public Task DeleteRecordAsync<T>(string collectionName, Guid id);
        /// <summary>
        /// Finds all records asynchronously from the Collection with optional filter applied.
        /// </summary>
        public Task<List<T>> FindAsync<T>(string collectionName, FilterDefinition<T>? filter = null);
        /// <summary>
        /// Finds all records synchronously from the Collection with optional filter applied.
        /// </summary>
        public Task<T?> FindByIdAsync<T>(string collectionName, Guid id);
        /// <summary>
        /// Finds paginated records asynchronously from the Collection with optional filter applied.
        /// </summary>
        /// <returns>
        /// Returns both the paginated records and the total no of records available
        /// </returns>
        /// <remarks>
        /// int skip = the no of records to skip.
        /// int limit = the no of records to return.
        /// </remarks>
        public Task<(List<T> paginatedRecords, int totalRecords)> FindPaginatedAsync<T>(string collectionName, int skip, int limit, FilterDefinition<T>? filter = null);

        /// <summary>
        /// Finds paginated records asynchronously from the Collection with optional filter applied.
        /// </summary>
        /// <returns>
        /// Returns both the paginated records and the total no of records available
        /// </returns>
        /// <remarks>
        /// int skip = the no of records to skip.
        /// int limit = the no of records to return.
        /// </remarks>
        public (List<T> paginatedRecords, int totalRecords) FindPaginated<T>(string collectionName, int skip, int limit, FilterDefinition<T>? filter = null);

        public Task InsertManyAsync<T>(string collectionName, List<T> records);
        public Task InsertOneAsync<T>(string collectionName, T record);
        public Task UpsertAsync<T>(string collectionName, Guid id, T record);
        public void DeleteRecord<T>(string collectionName, Guid id);
        public List<T> Find<T>(string collectionName, FilterDefinition<T>? filter = null);
        public T? FindById<T>(string collectionName, Guid id);
        public void InsertMany<T>(string collectionName, List<T> records);
        public void InsertOne<T>(string collectionName, T record);
        public void Upsert<T>(string collectionName, Guid id, T record);
    }
}