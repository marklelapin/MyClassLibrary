using MyClassLibrary.LocalServerMethods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    /// <summary>
    /// Supplies the sample data for resetting sample data on LocalServerModel databases.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISampleDataProvider<T>
    {
        /// <summary>
        /// Gets static Sample Data for testing to Local Storage
        /// </summary>
        /// <returns></returns>
        public List<T> GetLocalStartingSampleData();

        /// <summary>
        /// Get static Sample Data for testing to Server Storage
        /// </summary>
        /// <returns></returns>
        public List<T> GetServerStartingSampleData();

        /// <summary>
        /// Get static Sample Data for testing to ServerSyncLog 
        /// </summary>
        /// <returns></returns>
        public List<ServerSyncLog> GetServerSyncLogSampleData();

    }
}
