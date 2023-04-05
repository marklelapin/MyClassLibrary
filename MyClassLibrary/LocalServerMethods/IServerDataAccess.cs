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
        /// Saves Objects Data that inherit from LocalServerIdentity into Server Storage. Pass back UpdatedOnServer dat to objects.
        /// </summary>
        public void SaveToServer<T>(List<T> objects) where T : LocalServerIdentity;

        /// <summary>
        /// Finds all objects on the Server where the UpdatedOnServer date is later than LastSyncSuccessfullDate
        /// </summary>
        public List<T> LoadChangesFromServer<T>(DateTime LastSyncDate) where T : LocalServerIdentity;

        /// <summary>
        /// Saves a local storage path to the server against the user and device if not using local browser storage.
        /// </summary>
        public void saveLocalStoragePathToServer(string path);
  
    
    }
}
