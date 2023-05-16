namespace MyClassLibrary.LocalServerMethods
{
    public interface IServerAPIControllerService<T> where T : LocalServerIdentityUpdate
    {
        string Get(string ids);
        string GetChanges(DateTime lastSyncDate);
        void PostConflicts(List<Conflict> conflicts);
        DateTime PostUpdates(List<T> updates);
    }
}