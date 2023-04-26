using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.LocalServerMethods
{
    public interface IServerDataAccess
    {

        /// <summary>
        /// Returns the data stored on server for the list of Guids provided.
        /// </summary>
        public List<T> GetFromServer<T>(List<Guid>? ids = null) where T : LocalServerIdentityUpdate;


        /// <summary>
        /// Finds all objects on the Server where the UpdatedOnServer date is later than LastSyncDate
        /// </summary>
        public (List<T> changesFromServer ,DateTime lastUpdatedOnServer) GetChangesFromServer<T>(DateTime LastSyncDate) where T : LocalServerIdentityUpdate;


        /// <summary>
        /// Saves Objects Data that inherit from LocalServerIdentity into Server Storage. Pass back UpdatedOnServer dat to objects.
        /// </summary>
        public DateTime SaveToServer<T>(List<T> objects) where T : LocalServerIdentityUpdate;

        /// <summary>
        /// Adds the corresponding Conflict Id (value) to all objects inhereting from local server identity with Id = (key)
        /// </summary>
        public void SaveConflictIdsToServer<T>(List<Conflict> conflicts) where T : LocalServerIdentityUpdate;

        /// <summary>
        /// Permanently deletes the objects inheriting from LocalServerIdentiy passed in from server storage.
        /// </summary>
        public void DeleteFromServer<T>(List<T> objects) where T : LocalServerIdentityUpdate;


    }
}
