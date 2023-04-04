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
            /// <typeparam name="T"></typeparam>
            /// <param name="objects"></param>
            /// <exception cref="NotImplementedException"></exception>
            public void SaveToServer<T>(List<T> objects) where T : LocalServerIdentity
            {
                throw new NotImplementedException();
            }
            /// <summary>
            /// Finds all objects on the Server where the UpdatedOnServer date is later than LastSyncSuccessfullDate
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <exception cref="NotImplementedException"></exception>
            public List<T> LoadChangesFromServer<T>() where T : LocalServerIdentity
        {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Saves a local storage path to the server against the user and device if not using local browser storage.
            /// </summary>
            /// <param name="path"></param>
            /// <exception cref="NotImplementedException"></exception>
            public void saveLocalStoragePathToServer(string path)
            {
                throw new NotImplementedException();
            }

    
    }
}
