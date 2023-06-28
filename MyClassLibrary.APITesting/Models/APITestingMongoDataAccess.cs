using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MyClassLibrary.APITesting.Interfaces;
using MyClassLibrary.DataAccessMethods;
using System.Runtime.InteropServices;

namespace MyClassLibrary.APITesting.Models
{
    public class APITestingMongoDataAccess : IAPITestingDataAccess
    {
        private readonly IMongoDBDataAccess _mongoDBDataAccess;

        public APITestingMongoDataAccess(IMongoDBDataAccess mongoDBDataAccess)
        {
            _mongoDBDataAccess = mongoDBDataAccess;
        }

        public List<APITestData> GetAllBetweenDates(int testCollectionID, DateTime startDate, DateTime endDate)
        {
            var builder = Builders<APITestData>.Filter;
            var filter = builder.Gte(t=>t.TestDateTime, startDate) & builder.Lte(t=>t.TestDateTime, endDate) & builder.Eq(t=>t.CollectionId,testCollectionID);
            var output = _mongoDBDataAccess.Find<APITestData>("Tests", filter);
            return output;
        }

        public List<APITestData> GetAllByDateTime(int testCollectionId, DateTime testDateTime)
        {
            var builder = Builders<APITestData>.Filter;
            var filter = builder.Eq(t=>t.TestDateTime, testDateTime) & builder.Eq(t=>t.CollectionId,testCollectionId);
            var output = _mongoDBDataAccess.Find<APITestData>("Tests", filter);
            return output;
        }

        public List<APITestData> GetAllByTestCollectionId(int testCollectionId)
        {
            var builder = Builders<APITestData>.Filter;
            var filter = builder.Eq(t=>t.CollectionId,testCollectionId);
            var output = _mongoDBDataAccess.Find<APITestData>("Tests", filter);
            return output;
        }

        public List<APITestData> GetAllByTestId(int testCollectionId, int testId)
        {
            var builder = Builders<APITestData>.Filter;
            var filter = builder.Eq(t=>t.TestId,testId);
            var output = _mongoDBDataAccess.Find<APITestData>("Tests", filter);
            return output;

        }

        public void Save(APITestCollection testCollection)
        {
            var listTests = testCollection.CreateAPITestData();
            _mongoDBDataAccess.InsertManyAsync("Tests", listTests);
        }
    }
}
