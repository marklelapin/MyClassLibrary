using MongoDB.Bson;
using MongoDB.Driver;

namespace MyClassLibrary.DataAccessMethods
{
    public class MongoDBDataAccess : IMongoDBDataAccess
    {

        private IMongoDatabase db;

        public MongoDBDataAccess(string databaseName, string connectionString)
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            db = client.GetDatabase(databaseName);
        }

        public async Task InsertOneAsync<T>(string collectionName, T record)
        {
            var collection = db.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(record);
        }

        public void InsertOne<T>(string collectionName, T record)
        {
            var collection = db.GetCollection<T>(collectionName);
            collection.InsertOneAsync(record);
        }




        public async Task InsertManyAsync<T>(string collectionName, List<T> records)
        {
            var collection = db.GetCollection<T>(collectionName);
            await collection.InsertManyAsync(records);
        }


        public void InsertMany<T>(string collectionName, List<T> records)
        {
            var collection = db.GetCollection<T>(collectionName);
            collection.InsertManyAsync(records);
        }




        public async Task<List<T>> FindAsync<T>(string collectionName, FilterDefinition<T>? filter = null, SortDefinition<T>? sortBy = null)
        {
            var collection = db.GetCollection<T>(collectionName);
            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }

            if (sortBy == null)
            {

                return await collection.Find(filter).ToListAsync();
            }
            else
            {
                return await collection.Find(filter).Sort(sortBy).ToListAsync();
            }


        }


        public List<T> Find<T>(string collectionName, FilterDefinition<T>? filter = null, SortDefinition<T>? sortBy = null)
        {
            var collection = db.GetCollection<T>(collectionName);
            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }

            if (sortBy == null)
            {

                return collection.Find(filter).ToList();
            }
            else
            {
                return collection.Find(filter).Sort(sortBy).ToList();
            }

        }

        public async Task<(List<T> paginatedRecords, int totalRecords)> FindPaginatedAsync<T>(string collectionName, int skip, int limit, FilterDefinition<T>? filter = null, SortDefinition<T>? sortBy = null)
        {
            var collection = db.GetCollection<T>(collectionName);
            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }

            var query = (sortBy == null) ? collection.Find(filter) : collection.Find(filter).Sort(sortBy);



            var countTask = query.CountDocumentsAsync();

            var recordsTask = query.Skip(skip).Limit(limit).ToListAsync();
            await Task.WhenAll(countTask, recordsTask);
            return (recordsTask.Result, (int)countTask.Result);
        }

        public (List<T> paginatedRecords, int totalRecords) FindPaginated<T>(string collectionName, int skip, int limit, FilterDefinition<T>? filter = null, SortDefinition<T>? sortBy = null)
        {
            var collection = db.GetCollection<T>(collectionName);
            if (filter == null)
            {
                filter = Builders<T>.Filter.Empty;
            }
            var query = (sortBy == null) ? collection.Find(filter) : collection.Find(filter).Sort(sortBy);

            var count = query.CountDocuments();
            var records = query.Skip(skip).Limit(limit).ToList();

            return (records, (int)count);
        }





        public async Task<T?> FindByIdAsync<T>(string collectionName, Guid id)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);

            var listOutput = await collection.Find(filter).ToListAsync();

            T? output = listOutput.FirstOrDefault();

            return output;

        }
        public T? FindById<T>(string collectionName, Guid id)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);

            var listOutput = collection.Find(filter).ToList();

            T? output = listOutput.FirstOrDefault();

            return output;

        }



        public async Task UpsertAsync<T>(string collectionName, Guid id, T record)
        {
            BsonBinaryData binData = new BsonBinaryData(id, GuidRepresentation.Standard);
            var collection = db.GetCollection<T>(collectionName);
            await collection.ReplaceOneAsync(new BsonDocument("_id", binData), record, new ReplaceOptions { IsUpsert = true }); ;
        }

        public void Upsert<T>(string collectionName, Guid id, T record)
        {
            BsonBinaryData binData = new BsonBinaryData(id, GuidRepresentation.Standard);
            var collection = db.GetCollection<T>(collectionName);
            collection.ReplaceOneAsync(new BsonDocument("_id", binData), record, new ReplaceOptions { IsUpsert = true }); ;
        }



        public async Task DeleteRecordAsync<T>(string collectionName, Guid id)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);
            await collection.DeleteOneAsync(filter);

        }

        public void DeleteRecord<T>(string collectionName, Guid id)
        {
            var collection = db.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOneAsync(filter);

        }


    }
}
