using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.LocalServerMethods
{
    public interface ILocalDataAccess
    {
        /// <summary>
        /// Saves Objects inhereting from LocalServerIdentity to Local Storage (quickly identifiable as Changes). Passess UpdatedLocally Date back into objects
        /// </summary>
        /// <typeparam name="T">Object inheriting from LocalServerIdentity</typeparam>
        /// <param name="objects">The Objects to be Saved</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SaveToLocalChanges<T>(List<T> objects) where T : LocalServerIdentity /// TODO if this works then add to all other methods
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Saves Objects inhereting from LocalServerIdentity to the Main Local Storage. Passess UpdatedLocally Date back into objects
        /// </summary>
        /// <typeparam name="T">Object inheriting from LocalServerIdentity</typeparam>
        /// <param name="objects">The Objects to be Saved</param>
        /// <exception cref="NotImplementedException"></exception>
         public void SaveToLocalMain<T>(List<T> objects)
        { 
            throw new NotImplementedException();
        }
        /// <summary>
        /// 'Deletes' Objects inhereting from LocalServerIdentity from Local Changes.
        /// </summary>
        /// <typeparam name="T">Object inheriting from LocalServerIdentity</typeparam>
        /// <param name="objects">The Objects to be Saved</param>
        /// <exception cref="NotImplementedException"></exception>
        public void DeleteLocalChanges<T>(List<T> objects)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        // /Loads Objects inhereting from LocalServerIdentity from Main Local Storage.
        /// </summary>
        /// <typeparam name="T">Object inheriting from LocalServerIdentity</typeparam>
        /// <param name="objects">The Objects to be Saved</param>
        /// <exception cref="NotImplementedException"></exception>
        public List<T> LoadFromLocalMain<T>(List<Guid>? ids = null)
        {
            if (ids == null)
            {
                throw new NotImplementedException("Load all data from Local");
            } else
            {
                throw new NotImplementedException("Load data relating to specific ids");
            }

            
        }

        /// <summary>
        /// Gets the Last Successfull Sync Date from Local Storage
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void getLocalSyncSuccessfullDate()
        { 
            throw new NotImplementedException(); 
        }
        
        /// <summary>
        /// Save tehs Last Successufl Sync Date to Local Storage
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void SetLocalSyncSuccessfullDate()
        {
            throw new NotImplementedException();
        }

       


    }
}
