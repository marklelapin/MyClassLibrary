
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

        public async Task<(List<ApiTestData> records, int total)> GetAllByCollectionId(Guid testCollectionId, DateTime? startDate, DateTime? endDate, int skip = 0, int limit = 10000)
        {
            startDate = startDate ?? DateTime.MinValue;
            endDate = endDate ?? DateTime.MaxValue;


            var builder = Builders<ApiTestData>.Filter;
            var filter = builder.Gte(t => t.TestDateTime, startDate) & builder.Lte(t => t.TestDateTime, endDate) & builder.Eq(t => t.CollectionId, testCollectionId);

            var sort = Builders<ApiTestData>.Sort.Descending("TestDateTime");

            var output = await _mongoDBDataAccess.FindPaginatedAsync<ApiTestData>("Tests", skip, limit, filter, sort);

            output.paginatedRecords = output.paginatedRecords.OrderBy(t => t.TestDateTime).ToList();
            return output;
        }

        public async Task<(List<ApiTestData> records, int total)> GetAllByTestId(Guid testId, DateTime? startDate = null, DateTime? endDate = null, int skip = 0, int limit = 10000)
        {
            startDate = startDate ?? DateTime.MinValue;
            endDate = endDate ?? DateTime.MinValue;

            var builder = Builders<ApiTestData>.Filter;
            var filter = builder.Gte(t => t.TestDateTime, startDate) & builder.Lte(t => t.TestDateTime, endDate) & builder.Eq(t => t.TestId, testId);

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
