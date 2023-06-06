using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    /// <summary>
    /// Unit and Integration Tests to assert that ILocalServerEngine is operating correctly.
    /// </summary>
    /// <remarks>
    /// Requires that ILocalDataAccess and IServerDataAccess Tests are working first.
    /// </remarks>
    public interface ILocalServerEngineTests
    {
        /// <summary>
        /// Test that all updates are returned for given Ids.
        /// </summary>
        /// <returns></returns>
        public Task GetAllUpdatesTest();

        /// <summary>
        /// Test that latest updates are returned for given Ids.
        /// </summary>
        /// <returns></returns>
        public Task GetLatestUpdatesTest();

        /// <summary>
        /// Test that conflicted updates are returned for given Ids.
        /// </summary>
        /// <returns></returns>
        public Task GetConflictedUpdatesTest();   
    
        /// <summary>
        /// Test that save is successfull (will fail if sync isn't working as this part of the Save process)
        /// </summary>
        /// <returns></returns>
        public Task SaveUpdatesTest();

        /// <summary>
        /// Tests that the sync process is working when both Local and Server are accessible
        /// </summary>
        /// <returns></returns>
        public Task TrySyncTest();

        /// <summary>
        /// Tests that the sync process errors out if can't get local access.
        /// </summary>
        /// <returns></returns>
        public Task TrySyncLocalFailureTest();

        /// <summary>
        /// Tests that the sync process reports a failed attempt but doesn't error out if connection failure.
        /// </summary>
        /// <returns></returns>
        public Task TrySyncServerConnectionStringFailureTest();

        /// <summary>
        /// Tests that the sync processes errors out if there is a failure not covered by connection or authorization.
        /// </summary>
        /// <returns></returns>
        public Task TrySyncServerGeneralFailureTest();


        /// <summary>
        /// Test that Given Ids are successfully cleared of conflicts.
        /// </summary>
        /// <returns></returns>
        public Task ClearConflictedIdsTest();


        /// <summary>
        /// Tests that conflicts between local and server are identified and returned to both local and server 
        /// </summary>
        /// <remarks>
        /// Updates two conflicting sets of updates to local and server without Syncing. <br/>
        /// Then runs TrySync.<br/>
        /// The updates on server and local should be identical at end and all have isConflicted = true.
        /// </remarks>
        /// <returns></returns>
        public Task TrySyncWithConflictsTest();

      
    }
}
