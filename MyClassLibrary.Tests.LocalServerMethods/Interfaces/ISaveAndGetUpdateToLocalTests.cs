using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.LocalServerMethods.Extensions;


namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    /// <summary>
    /// Tests methods that write to Database by writing to the test Database and then getting values back.
    /// </summary>
    /// <remarks>
    /// In order for these to work effectively the corresponding ILocalDataAccessGetTest need to by passed first.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public interface ISaveAndGetUpdateToLocalTests<T> where T : LocalServerModelUpdate
    {
        /// <summary>
        /// Saves and then gets Updates to Local Storage and checks that output is same as input.
        /// </summary>
        /// <remarls>
        /// Should be setup for each new UpdateType created.
        /// </remarls>
        [Fact]
        public async Task SaveAndGetUpdatesTest(List<T> updates)
        {
            //Setup
            List<T> expected = updates;
            List<LocalToServerPostBack> expectedPostBack = updates.Select(x => new LocalToServerPostBack(x.Id, x.Created, x.IsConflicted)).ToList();
            expectedPostBack = expectedPostBack.SortByCreated();

            //Test
            List<LocalToServerPostBack> actualPostBack = await _localDataAccess.SaveUpdatesToLocal(testUpdates);
            actualPostBack = actualPostBack.SortByCreated();

            //Get Result From Local
            List<TestUpdate> actual = await _localDataAccess.GetUpdatesFromLocal(testUpdates.Select(x => x.Id).ToList(), false);
            actual = actual.SortByCreated();

            //Assert
            Assert.Equal(JsonSerializer.Serialize(expectedPostBack), JsonSerializer.Serialize(actualPostBack));
            Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));


        }


    }
}
