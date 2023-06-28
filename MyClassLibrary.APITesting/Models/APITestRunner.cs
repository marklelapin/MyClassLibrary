using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MyClassLibrary.APITesting.Interfaces;

namespace MyClassLibrary.APITesting.Models
{
    internal class APITestRunner : IAPITestRunner
    {
        private readonly IAPITestingDataAccess _dataAccess;
        private readonly IDownstreamApi _downstreamAPI;

        public APITestRunner(IAPITestingDataAccess dataAccess, IDownstreamApi downstreamApi)
        {
            _dataAccess = dataAccess;
            _downstreamAPI = downstreamApi;
        }


        public APITestResult Run(APITest test)
        {
            return Run(new List<APITest> { test }).First();
        }

        public void Run(List<APITest> tests)
        {
            //Start Timer
            //CAll downstreamApi
            //End Timer
            //Run Tests
            //save test results to tests

            throw new NotImplementedException();
        }



        public void Save(APITestCollection testCollection)
        {
            _dataAccess.Save(testCollection);
        }

        public void RunAndSave(APITestCollection testCollection)
        {
            Run(testCollection.Tests);
            Save(testCollection);
        }
    }
}
