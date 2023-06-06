
using MyClassLibrary.LocalServerMethods.Models;


namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    /// <summary>
    /// Unit and Integration Tests verifying 'Post' methods of ILocalDataAccess
    /// </summary>
    /// <remarks>
    /// These are largely integrations tests based on performing the test operation and then using 'Get' methods to check actual result from data storage.<br/>
    /// In order for these to work effectively the corresponding ILocalDataAccess GetTests  need to by passed first.
    /// </remarks>
    public interface IPostTestUpdateToLocalTests
    {
        
        /// <summary>
        /// Saves testUpdates to Local Storage and checks that it doesn't error out.
        /// </summary>
        public Task SaveUpdatesTest();
       
        /// <summary>
        /// Saves test Updates to Local Storage and  retrieves and checks that output is same as input.
        /// </summary>
        /// <remarks>
        /// Runs several tests:<br/>
        /// 1. Simple Save and Retrieve <br/>
        /// 2. Save of data with potential conflict - should identify the conflict correctly and save data with isConflicted = true <br/>
        /// <br/>
        /// Two assertions are made that the database has been updated correctly and that the updates Passed Back also match.
        /// </remarks>
        public Task SaveAndGetUpdatesTest();

        /// <summary>
        /// Tests that if an attempt is made to save duplicate updates it doesn't error out and doesn't save the new data.
        /// </summary>
        /// <remarks>
        /// Runs several tests:<br/>
        /// 1. Simple Save and Retrieve <br/>
        /// </remarks>
        public Task SaveAndGetDuplicateUpdatesTest();

        /// <summary>
        /// Saves LastSyncDate to local storage and retrieves it. Checks that output is same as input. Also checks that doesn't error out when no changes present.
        /// </summary>
        public Task SaveAndGetLocalLastSyncDateTest();

        /// <summary>
        /// Generates Test Updates with ConflictIDs then runs ClearConflict Test and tests if IsConflicted is now false
        /// </summary>
        public Task ClearConflictsTest();

        /// <summary>
        /// Test that the Server PostBack to Local Test works by saving new data, running the post back and then getting result back.
        /// </summary>
        public Task ServerPostBackToLocalTest();


    }
}
