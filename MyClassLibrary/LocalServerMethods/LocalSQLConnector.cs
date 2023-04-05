using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.LocalServerMethods
{
    public class LocalSQLConnector : ILocalDataAccess
    {
        public void DeleteLocalChanges<T>(List<T> objects)
        {
            throw new NotImplementedException();
        }

        public void GetLocalSyncSuccessfullDate()
        {
            throw new NotImplementedException();
        }

        public List<T> LoadFromLocalMain<T>(List<Guid>? ids = null)
        {
            throw new NotImplementedException();
        }

        public void SaveToLocalChanges<T>(List<T> objects) where T : LocalServerIdentity
        {
            throw new NotImplementedException();
        }

        public void SaveToLocalMain<T>(List<T> objects)
        {
            throw new NotImplementedException();
        }

        public void SetLocalSyncSuccessfullDate()
        {
            throw new NotImplementedException();
        }
    }
}
