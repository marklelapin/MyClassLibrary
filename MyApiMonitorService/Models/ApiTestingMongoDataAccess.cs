
using MongoDB.Driver;
using MyApiMonitorService.Interfaces;
using MyClassLibrary.DataAccessMethods;

namespace MyApiMonitorService.Models
{
    public class ApiTestingMongoDataAccess : IApiTestingDataAccess
    {
        private readonly IMongoDBDataAccess _mongoDBDataAccess;

        public ApiTestingMongoDataAccess(IMongoDBDataAccess mongoDBDataAccess)
        {
            _mongoDBDataAccess = mongoDBDataAccess;
        }

        public List<ApiTestData> GetAllBetweenDates(Guid testCollectionID, DateTime startDate, DateTime endDate)
        {
            var builder = Builders<ApiTestData>.Filter;
            var filter = builder.Gte(t => t.TestDateTime, startDate) & builder.Lte(t => t.TestDateTime, endDate) & builder.Eq(t => t.CollectionId, testCollectionID);
            var output = _mongoDBDataAccess.Find<ApiTestData>("Tests", filter);
            return output;
        }

        public List<ApiTestData> GetAllByDateTime(Guid testCollectionId, DateTime testDateTime)
        {
            var builder = Builders<ApiTestData>.Filter;
            var filter = builder.Eq(t => t.TestDateTime, testDateTime) & builder.Eq(t => t.CollectionId, testCollectionId);
            var output = _mongoDBDataAccess.Find<ApiTestData>("Tests", filter);
            return output;
        }

        public List<ApiTestData> GetAllByTestCollectionId(Guid testCollectionId)
        {
            var builder = Builders<ApiTestData>.Filter;
            var filter = builder.Eq(t => t.CollectionId, testCollectionId);
            var output = _mongoDBDataAccess.Find<ApiTestData>("Tests", filter);
            return output;
        }

        public List<ApiTestData> GetAllByTestId(Guid testId)
        {
            var builder = Builders<ApiTestData>.Filter;
            var filter = builder.Eq(t => t.TestId, testId);
            var output = _mongoDBDataAccess.Find<ApiTestData>("Tests", filter);
            return output;

        }

        public async Task Save(ApiTestCollection testCollection)
        {
            var listTests = testCollection.CreateApiTestData();
            await _mongoDBDataAccess.InsertManyAsync("Tests", listTests);
        }
    }
}
