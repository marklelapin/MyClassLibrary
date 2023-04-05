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
        public void SaveToLocalChanges<T>(List<T> objects) where T : LocalServerIdentity;/// TODO if this works then add to all other methods

        /// <summary>
        /// Saves Objects inhereting from LocalServerIdentity to the Main Local Storage. Passess UpdatedLocally Date back into objects
        /// </summary>
        public void SaveToLocalMain<T>(List<T> objects);

        /// <summary>
        /// 'Deletes' Objects inhereting from LocalServerIdentity from Local Changes.
        /// </summary>
        public void DeleteLocalChanges<T>(List<T> objects);

        /// <summary>
        /// Loads Objects inhereting from LocalServerIdentity from Main Local Storage.
        /// </summary>
        public List<T> LoadFromLocalMain<T>(List<Guid>? ids = null);

        /// <summary>
        /// Gets the Last Successfull Sync Date from Local Storage
        /// </summary>
        public void GetLocalSyncSuccessfullDate();

        /// <summary>
        /// Save tehs Last Successufl Sync Date to Local Storage
        /// </summary>
        public void SetLocalSyncSuccessfullDate();
 
    }
}
