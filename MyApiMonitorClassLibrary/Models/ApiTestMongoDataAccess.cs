﻿
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

        public List<ApiTestData> GetAllBetweenDates(Guid testCollectionID, DateTime? startDate, DateTime? endDate)
        {
            startDate = startDate ?? DateTime.MinValue;
            endDate = endDate ?? DateTime.MaxValue;


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

        public void Save(ApiTestCollection testCollection)
        {
            var listTests = testCollection.CreateApiTestData();
            _mongoDBDataAccess.InsertManyAsync("Tests", listTests).Wait();
        }
    }
}
