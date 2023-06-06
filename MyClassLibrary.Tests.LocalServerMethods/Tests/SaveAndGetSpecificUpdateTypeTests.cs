using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.LocalServerMethods.Extensions;
using System.Text.Json;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class SaveAndGetSpecificUpdateTypeTests<T> where T : LocalServerModelUpdate
    {

        private readonly ILocalDataAccess<T> _localDataAccess;

        private readonly IServerDataAccess<T> _serverDataAccess;

        private readonly ISaveAndGetTestContent<T> _testContent;

        public SaveAndGetSpecificUpdateTypeTests(ILocalDataAccess<T> localDataAccess,IServerDataAccess<T> serverDataAccess,ISaveAndGetTestContent<T> testContent)
        {
            _localDataAccess = localDataAccess;
            _serverDataAccess = serverDataAccess;
            _testContent = testContent;
        }



    
        [Fact]
        public async Task SaveAndGetUpdatesTest()
        {
            //Setup
            List<T> updates = _testContent.getNewUpdates();
            List<T> expected = updates;
            expected = expected.SortByCreated();

            //Test
            await _localDataAccess.SaveUpdatesToLocal(updates);
            
            //Get Result From Local
            List<T> actual = await _localDataAccess.GetUpdatesFromLocal(updates.Select(x => x.Id).ToList(), false);
            actual = actual.SortByCreated();

            //Assert
            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));


        }
    }
}
