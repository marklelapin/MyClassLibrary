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
        public object[][] SaveTestData();
        public void SaveTest(List<T> testUpdates);


        public object[][] SaveAndGetTestData();
        public void SaveAndGetTest(List<T> testUpdates, List<Guid> getIds, List<T> expected);


        public object[][] GetChangesTestData();
        public void GetChangesTest(List<T> testUpdates, int lastSyncDateAdjustment, List<T> expected);


        public object[][] SaveConflictIdTestData();
        public void SaveConflictIdTest(List<T> testUpdate, List<Conflict> conflicts, List<Conflict> expected);


        //TODO - ADD in Delete to these interfaces
        //public object[][] DeleteTestData();
        //public void DeleteTest(List<T> testUpdatesTpDelete);
    }
}
