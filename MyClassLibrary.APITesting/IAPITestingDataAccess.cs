using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyClassLibrary.APITesting
{
    /// <summary>
    /// Provides the method for saving API Tests to a database.
    /// </summary>
    public interface IAPITestingDataAccess
    {
        /// <summary>
        /// Saves a testCollection to database.
        /// </summary>
        public void Save(APITestCollection testCollection);
    }
}
