﻿using System;
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
        public DateTime GetLocalLastSyncDate<T>();


        /// <summary>
        /// Save the Last Successufl Sync Date to Local Storage
        /// </summary>
        public void SaveLocalLastSyncDate<T>(DateTime lastSyncDate);

        /// <summary>
        /// Loads Objects inhereting from LocalServerIdentity from Local Storage given a list of Ids.
        /// </summary>
        public List<T> GetFromLocal<T>(List<Guid>? ids = null) where T : LocalServerIdentity;


        /// <summary>
        /// Loads Objects inhereting from LocalServerIdentity from Local Storage with no UpdatedOnServer date.
        /// </summary>
        public List<T> GetChangesFromLocal<T>() where T : LocalServerIdentity;  


        /// <summary>
        /// Saves Objects inhereting from LocalServerIdentity to Local Storage
        /// </summary>
        public void SaveToLocal<T>(List<T> objects) where T : LocalServerIdentity;

        /// <summary>
        /// Sets the UpdatedOnServerDate property for objects inhereting from LocalServerIdentity to LocalStorage
        /// </summary>
        public void SaveUpdatedOnServerDate<T>( List<T> objects,DateTime UpdatedOnServer) where T : LocalServerIdentity;

    }
}
