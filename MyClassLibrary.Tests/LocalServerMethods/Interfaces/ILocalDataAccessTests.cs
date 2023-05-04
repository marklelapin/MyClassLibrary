using MyClassLibrary.LocalServerMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    public interface ILocalDataAccessTests<T> where T : LocalServerIdentityUpdate
    {
        /// <summary>
        /// Gets the data for SaveTest
        /// </summary>
        public object[][] SaveTestData();
        /// <summary>
        /// Saves testUpdates to Local Storage and checks that it doesn't error out.
        /// </summary>
        public void SaveTest(List<T> testUpdates);
       

        /// <summary>
        /// Gets the data for SaveAndGetTest
        /// </summary>
        public object[][] SaveAndGetTestData();
        /// <summary>
        /// Saves test Updates to Local Storage and the retrieves and checks that output is same as input.
        /// </summary>
        public void SaveAndGetTest(List<T> testUpdates, List<Guid> idsToGet, List<T> expected);
        

        /// <summary>
        /// Gets the data for GetChangesTest
        /// </summary>
        public object[][]GetChangesTestData();
        /// <summary>
        /// Save testUpdates to Local Storage and the looks for changes. Checks that output contains only those testUpdates that have UpdatedOnServer == null. 
        /// </summary>
        public void GetChangesTest(List<T> testUpdates); //also tests functionality of passing null into GetFromLocal
        

        /// <summary>
        /// Gets the data for SaveAndGetLocalLastSyncDateTest
        /// </summary>
        public object[][] SaveAndGetLocalLastSyncDateTestData();
        /// <summary>
        /// Saves LastSyncDate to local storage and retrieves it. Checks that output is same as input. Also checks that doesn' error out when no changes present.
        /// </summary>
        public void SaveAndGetLocalLastSyncDateTest(DateTime expected);



        /// <summary>
        /// Gets the data for SaveUpdatedOnServerTest
        /// </summary>
        public object[][] SaveUpdatedOnServerTestData();
        /// <summary>
        /// Generates and saves testUpdates to Local Storage. Then Saves UpdatedonServer date against given Ids before retrieving all of the updates from Local Storage.
        /// Checks that updateonServerData has only been added to the specific ids.
        /// </summary>
        public void SaveUpdatedOnServerTest(List<T> testUpdates);


        /// <summary>
        /// Gets the data for SaveConflictIdTest
        /// </summary>
        public object[][] SaveConflictIdTestData();
        /// <summary>
        /// Saves testUpdates to Local Storage. Then runs SaveConflictIdsToLocal with given on conflicts. Checks that the correct guid has been applied to the correct testUpdates.
        /// Also checks that no conflicts doesn't error out.
        /// </summary>
        public void SaveConflictIdTest(List<T> testUpdates, List<Conflict> conflicts, List<Conflict> expected);
        /// <summary>
        /// Saves testUpdates to Local Storage plus additional set of updates. then deletes them. Checks to see if the deleted updates are not there and the others are.
        /// </summary>


        //TODO - ADD in Delete to these interfaces
        ///// <summary>
        ///// Gets the data for DeleteTest
        ///// </summary>
        //public object[][] DeleteTestData();
        ///// <summary>
        ///// Saves testUpdates then tries to delete them entirely.
        ///// </summary>
        //public void DeleteTest(List<T> testUpdatesToDelete);
    }
}
