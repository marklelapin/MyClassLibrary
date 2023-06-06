
using MyClassLibrary.LocalServerMethods.Models;


namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{

    /// <summary>
    /// Unit Tests of 'Get' methods associated with ILocalDataAccess.
    /// </summary>
    /// <remarks>
    /// These require the Test Database to be Reset known Initial Test Values each time test is run.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public interface IGetTestUpdateFromLocalTests
    {
        
        /// <summary>
        /// Test GetLocalCopyID functionality
        /// </summary>
        /// <remarks>   
        /// Runs two sequential tests:<br/>
        /// 1. If CopyId is missing it puts new copy Id in place.<br/>
        /// 2. If CopyId is not null it returns the copyId<br/>
        /// </remarks>
        public Task GetLocalCopyIDTest();

        /// <summary>
        /// Tests that GetAllUpdates returns all updates relating to the List of Ids passed in.
        /// </summary>
        /// <remarks>
        /// Runs two tests:<br/>
        /// 1. With ids = list of guids<br/>
        /// 2. With Ids = null - (in this case should return all updates in the database)
        /// </remarks>
        public Task GetAllUpdatesTest(List<Guid>? ids,List<TestUpdate> expected);

        /// <summary>
        /// Tests that GetLatestUpdates returns the latest updates relating to the List of Ids passed in.
        /// </summary>
        /// <remarks>
        /// Runs two tests:<br/>
        /// 1. With ids = list of guids<br/>
        /// 2. With Ids = null - (in this case should return all latest updates in the database)
        /// </remarks>
        public Task GetLatestUpdatesTest(List<Guid>? ids, List<TestUpdate> expected);

        /// <summary>
        /// Tests that GetCinflictedUpdates returns all updates in concflict for the give Id.
        /// </summary>
        public Task GetConflictedUpdatesTest();

        /// <summary>
        /// Tests that only updates are returned with Updated On Server = null
        /// </summary>
        public Task GetUnsyncedUpdatesFromLocalTest();

        
    }
}
