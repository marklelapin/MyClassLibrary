using MyClassLibrary.LocalServerMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    public interface IServerDataAccessTests<T> where T : LocalServerIdentityUpdate
    {
        public void SaveTest(List<T> testUpdates);
        public void SaveAndGetTest(List<T> testUpdates, List<T> expected);
        public void GetChangesTest(List<T> testUpdates, int lastSyncDateAdjustment, List<T> expected);
        public void SaveConflictIdTest(List<T> testUpdate, List<Conflict> conflicts);
        public void DeleteTest(List<T> testUpdatesTpDelete);
    }
}
