using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyClassLibrary.APITesting.Models;

namespace MyClassLibrary.APITesting.Interfaces
{
    /// <summary>
    /// Provides the methods for executing,timing and saving the APITest class or lists of APITest 
    /// </summary>
    public interface IAPITestRunner
    {

        /// <summary>
        /// Runs a list of API Tests in the list order and populates the TestResult property of each test.
        /// </summary>
        /// <returns>
        /// True if all tests were successful.
        /// </returns>
        public void Run(List<APITest> tests);


        /// <summary>
        /// Saves a Test Collection to database
        /// </summary>
        public void Save(APITestCollection testCollection);


        /// <summary>
        /// Runs and saves a test Collection to database
        /// </summary>
        /// <param name="tests"></param>
        /// <returns></returns>
        public void RunAndSave(APITestCollection testCollection);


    }
}
