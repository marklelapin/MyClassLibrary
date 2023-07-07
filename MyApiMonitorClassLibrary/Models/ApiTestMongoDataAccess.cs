
using MongoDB.Driver;
using MyApiMonitorClassLibrary.Interfaces;
using MyClassLibrary.DataAccessMethods;

namespace MyApiMonitorClassLibrary.Models
{
    public class ApiTestMongoDataAccess : IApiTestDataAccess
    {
        private readonly IMongoDBDataAccess _mongoDBDataAccess;

        public ApiTestMongoDataAccess(IMongoDBDataAccess mongoDBDataAccess)
        {
            _mongoDBDataAccess = mongoDBDataAccess;
        }

        public async Task<(List<ApiTestData> records, int total)> GetAllBetweenDates(Guid testCollectionID, DateTime? startDate, DateTime? endDate, int skip = 0, int limit = 10000)
        {
            startDate = startDate ?? DateTime.MinValue;
            endDate = endDate ?? DateTime.MaxValue;


            var builder = Builders<ApiTestData>.Filter;
            var filter = builder.Gte(t => t.TestDateTime, startDate) & builder.Lte(t => t.TestDateTime, endDate) & builder.Eq(t => t.CollectionId, testCollectionID);

            var sort = Builders<ApiTestData>.Sort.Descending("TestDateTime");

            var output = await _mongoDBDataAccess.FindPaginatedAsync<ApiTestData>("Tests", skip, limit, filter, sort);

            output.paginatedRecords = output.paginatedRecords.OrderBy(t => t.TestDateTime).ToList();
            return output;
        }

        public async Task<(List<ApiTestData> records, int total)> GetAllByDateTime(Guid testCollectionId, DateTime testDateTime, int skip = 0, int limit = 10000)
        {
            var builder = Builders<ApiTestData>.Filter;
            var filter = builder.Eq(t => t.TestDateTime, testDateTime) & builder.Eq(t => t.CollectionId, testCollectionId);

            var sort = Builders<ApiTestData>.Sort.Descending("TestDateTime");

            var output = await _mongoDBDataAccess.FindPaginatedAsync<ApiTestData>("Tests", skip, limit, filter, sort);
            return output;
        }

        public async Task<(List<ApiTestData> records, int total)> GetAllByTestCollectionId(Guid testCollectionId, int skip = 0, int limit = 10000)
        {
            var builder = Builders<ApiTestData>.Filter;
            var filter = builder.Eq(t => t.CollectionId, testCollectionId);

            var sort = Builders<ApiTestData>.Sort.Descending("TestDateTime");

            var output = await _mongoDBDataAccess.FindPaginatedAsync<ApiTestData>("Tests", skip, limit, filter, sort);
            return output;
        }

        public async Task<(List<ApiTestData> records, int total)> GetAllByTestId(Guid testId, int skip = 0, int limit = 10000)
        {
            var builder = Builders<ApiTestData>.Filter;
            var filter = builder.Eq(t => t.TestId, testId);

            var sort = Builders<ApiTestData>.Sort.Descending("TestDateTime");

            var output = await _mongoDBDataAccess.FindPaginatedAsync<ApiTestData>("Tests", skip, limit, filter, sort);

            return output;

        }

        public async Task Save(ApiTestCollection testCollection)
        {
            var listTests = testCollection.CreateApiTestData();

            await _mongoDBDataAccess.InsertManyAsync("Tests", listTests);
        }
    }
}
