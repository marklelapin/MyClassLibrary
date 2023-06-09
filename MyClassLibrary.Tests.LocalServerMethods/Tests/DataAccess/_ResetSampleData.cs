using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests.DataAccess
{
    public class _ResetSampleData
    {
        private readonly ILocalDataAccess<TestUpdate> _localDataAccess;
        private readonly IServerDataAccess<TestUpdate> _serverDataAccess;
        public _ResetSampleData(ILocalDataAccess<TestUpdate> localDataAccess, IServerDataAccess<TestUpdate> serverDataAccess)
        {
            _localDataAccess = localDataAccess;
            _serverDataAccess = serverDataAccess;
        }


        [Fact]
        public async void ResetTest()
        {

            var resetLocalSampleData = await _localDataAccess.ResetSampleData(TestContent.LocalStartingData);
            var resetServerSampleData = await _serverDataAccess.ResetSampleData(TestContent.ServerStartingData, TestContent.ServerSyncInfoStartingData);



            Assert.True(resetLocalSampleData && resetServerSampleData); //This isn't a test - it is designed to be run before testing all to reset the Sample Data in the database.
        }


    }
}
