using MongoDB.Driver;

namespace MyClassLibrary.DataAccessMethods
{
    public interface IMongoDBDataAccess
    {
        public Task DeleteRecordAsync<T>(string collectionName, Guid id);
        public Task<List<T>> FindAsync<T>(string collectionName, FilterDefinition<T>? filter = null);
        public Task<T?> FindByIdAsync<T>(string collectionName, Guid id);
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