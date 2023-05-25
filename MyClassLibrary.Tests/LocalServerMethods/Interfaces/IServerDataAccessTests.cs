
using MyClassLibrary.LocalServerMethods.Models;


namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    public interface IServerDataAccessTests<T> where T : LocalServerModelUpdate
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
