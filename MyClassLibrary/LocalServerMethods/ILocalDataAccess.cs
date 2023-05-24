using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.LocalServerMethods
{
    public interface ILocalDataAccess
    {

        /// <summary>
        /// Gets the Last Successfull Sync Date from Local Storage
        /// </summary>
        public Task<DateTime> GetLocalLastSyncDate<T>();


        /// <summary>
        /// Save the Last Successufl Sync Date to Local Storage
        /// </summary>
        public Task SaveLocalLastSyncDate<T>(DateTime lastSyncDate);

        /// <summary>
        /// Gets all updates relating to the list of Ids.
        /// </summary>
        public Task<List<T>> GetFromLocal<T>(List<Guid>? ids = null) where T : LocalServerIdentityUpdate;

        ///// <summary>
        ///// Gets latest update relating to the list of Ids.
        ///// </summary>
        //public Task<List<T>> GetLatestFromLocal<T>(List<Guid>? ids = null) where T : LocalServerIdentityUpdate;

        /// <summary>
        /// Loads Objects inhereting from LocalServerIdentity from Local Storage with no UpdatedOnServer date.
        /// </summary>
        public Task<List<T>> GetChangesFromLocal<T>() where T : LocalServerIdentityUpdate;  


        /// <summary>
        /// Saves Objects inhereting from LocalServerIdentity to Local Storage
        /// </summary>
        public Task SaveToLocal<T>(List<T> updates) where T : LocalServerIdentityUpdate;

        /// <summary>
        /// Sets the UpdatedOnServerDate property for objects inhereting from LocalServerIdentity to LocalStorage
        /// </summary>
        public Task SaveUpdatedOnServerToLocal<T>( List<T> updates,DateTime UpdatedOnServer) where T : LocalServerIdentityUpdate;

        /// <summary>
        /// Adds the corresponding Conflict Id (value) to all objects inhereting from LocalServerIdentity with Id = (key)
        /// </summary>
        public Task SaveConflictIdsToLocal<T>(List<Conflict> conflicts) where T : LocalServerIdentityUpdate;

        /// <summary>
        /// Permanently deletes the objects inheriting from LocalServerIdentiy passed in from local storage.
        /// </summary>
        public Task DeleteFromLocal<T>(List<T> updates) where T : LocalServerIdentityUpdate;
    }
}
