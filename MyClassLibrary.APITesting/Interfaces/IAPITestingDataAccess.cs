using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MyClassLibrary.APITesting.Models;

namespace MyClassLibrary.APITesting.Interfaces
{
    /// <summary>
    /// Provides the method for saving API Tests to a database.
    /// </summary>
    public interface IAPITestingDataAccess
    {
        /// <summary>
        /// Saves a testCollection to database.
        /// </summary>
        public void Save(APITestCollection testCollection);

        /// <summary>
        /// Returns all tests on the database with a given testCollectionId.
        /// </summary>
        public List<APITestData> GetAllByTestCollectionId(int testCollectionId);

     
        /// <summary>
        /// Returs all tests on the database with a given testId.
        /// </summary>
        public List<APITestData> GetAllByTestId(int testCollectionId, int testId);

        /// <summary>
        /// Returns all test run at a specific datetime.
        /// </summary>
        /// <param name="testDateTime"></param>
        public List<APITestData> GetAllByDateTime(int testCollectionId, DateTime testDateTime);



        /// <summary>
        /// Returns all tests run between specific dates.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public List<APITestData> GetAllBetweenDates(int testCollectionID, DateTime startDate, DateTime endDate);
    }
}
