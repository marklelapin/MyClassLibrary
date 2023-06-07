using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Extensions;
using System.Text.Json;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;

namespace MyClassLibrary.Tests.LocalServerMethods.Services
{
    public class SaveAndGetUpdateTypeTestService<T> : ISaveAndGetUpdateTypeTests<T> where T : ILocalServerModelUpdate
    {

        private readonly ILocalDataAccess<T> _localDataAccess;
        private readonly IServerDataAccess<T> _serverDataAccess;
        private readonly ISaveAndGetTestContent<T> _testContent;

        public SaveAndGetUpdateTypeTestService (ILocalDataAccess<T> localDataAccess, IServerDataAccess<T> serverDataAccess, ISaveAndGetTestContent<T> testContent)
        {
            _localDataAccess = localDataAccess;
            _serverDataAccess = serverDataAccess;
            _testContent = testContent;
        }

        //[Fact]
        public async Task SaveAndGetLocalTest()
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

        //[Fact]
        public async Task SaveAndGetServerTest()
        {
            //Setup
            List<T> updates = _testContent.getNewUpdates();

            //Test
            await _serverDataAccess.SaveUpdatesToServer(updates,_testContent.CopyId);

            //Get Result From Local
            List<T> actual = await _serverDataAccess.GetUpdatesFromServer(updates.Select(x => x.Id).ToList(), false);
            actual = actual.SortByCreated();

            //Get expected (can only be done once UpdatedOnServer date is known.
            DateTime updatedOnServer = (DateTime)actual.First().UpdatedOnServer!;
            List<T> expected = updates;
            expected.ForEach(update =>
            {
                if (update.UpdatedOnServer == null) { update.UpdatedOnServer = updatedOnServer; };
            });
            expected = expected.SortByCreated();


            //Assert
            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));


        }
    }
}
