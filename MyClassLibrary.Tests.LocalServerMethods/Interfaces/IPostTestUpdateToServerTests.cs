
using MyClassLibrary.LocalServerMethods.Models;


namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    /// <summary>
    /// Unit and Integration Tests verifying 'Post' methods of IServerDataAccess
    /// </summary>
    /// <remarks>
    /// These are largely integrations tests based on performing the test operation and then using 'Get' methods to check for actual result from data storage.<br/>
    /// In order for these to work effectively the corresponding IServerDataAccess GetTests  need to by passed first.
    /// </remarks>
    public interface IPostTestUpdateToServerTests
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
        /// Two assertions are made with this test that the database has updated correctly and that the PostBackUpdates are also correct
        /// </remarks>
        public Task SaveAndGetUpdatesTest();

        /// <summary>
        /// Tests that if an attempt is made to save duplicate updates it doesn't error out and doesn't save the new data.
        /// </summary>
        public Task SaveAndGetDuplicateUpdatesTest();
       
       
        /// <summary>
        /// Generates Test Updates with isConflicted = true then runs ClearConflicts and tests if IsConflicted is now false
        /// </summary>
        public Task ClearConflictsTest();

        /// <summary>
        /// Test that the Local PostBack to Server works by saving new data, running the post back and then getting database result back.
        /// </summary>
        public Task LocalPostBackToServerTest();

    }
}
