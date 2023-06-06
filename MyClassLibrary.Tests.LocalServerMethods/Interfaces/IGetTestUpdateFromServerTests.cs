
using MyClassLibrary.LocalServerMethods.Models;


namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{

    /// <summary>
    /// Unit Tests of 'Get' methods associated with IServerDataAccess.
    /// </summary>
    /// <remarks>
    /// These require the Test Database to be Reset known Initial Test Values each time test is run.
    /// </remarks>

    public interface IGetTestUpdateFromServerTests
    {
        /// <summary>
        /// Tests that GetAllUpdates returns all updates relating to the List of Ids passed in.
        /// </summary>
        /// <remarks>
        /// Runs two tests:<br/>
        /// 1. With ids = list of guids<br/>
        /// 2. With Ids = null - (in this case should return all updates in the database)
        /// </remarks>
        public Task GetAllUpdatesTest(List<Guid> ids,List<TestUpdate> expected);

        /// <summary>
        /// Tests that GetLatestUpdates returns the latest updates relating to the List of Ids passed in.
        /// </summary>
        /// <remarks>
        /// Runs two tests:<br/>
        /// 1. With ids = list of guids<br/>
        /// 2. With Ids = null - (in this case should return all latest updates in the database)
        /// </remarks>
        public Task GetLatestUpdatesTest(List<Guid>? ids,List<TestUpdate> expected);

        /// <summary>
        /// Tests that GetConflictedUpdates returns all updates in conflict for the given Id.
        /// </summary>
        /// 
        public Task GetConflictedUpdatesTest();

        /// <summary>
        /// Tests that only updates are returned where there isn't the copy id agains it on the ServerSyncInfo table
        /// </summary>
        public Task GetUnsyncedUpdatesFromServerTest(Guid copyId, List<TestUpdate> expected);


        //TODO - ADD in Delete to these interfaces
        //public object[][] DeleteTestData();
        //public Task DeleteTest(List<T> testUpdatesTpDelete);
    }
}
