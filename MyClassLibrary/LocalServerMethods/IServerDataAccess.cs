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
        public List<T> GetFromServer<T>(List<Guid>? ids = null,bool IsActive = true) where T : LocalServerIdentity;


        /// <summary>
        /// Finds all objects on the Server where the UpdatedOnServer date is later than LastSyncDate
        /// </summary>
        public (List<T> changesFromServer ,DateTime lastUpdatedOnServer) GetChangesFromServer<T>(DateTime LastSyncDate) where T : LocalServerIdentity;


        /// <summary>
        /// Saves Objects Data that inherit from LocalServerIdentity into Server Storage. Pass back UpdatedOnServer dat to objects.
        /// </summary>
        public void SaveToServer<T>(List<T> objects) where T : LocalServerIdentity;

        

       
  
    
    }
}
