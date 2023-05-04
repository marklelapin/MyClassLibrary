namespace MyClassLibrary.LocalServerMethods
{
    public interface ILocalServerEngine<T> where T : LocalServerIdentityUpdate
    {

        List<T> FilterLatest(List<T> updates);
        List<Conflict> FindConflicts(List<T> changesFromServer, List<T> changesFromLocal);
        List<T> GetAllUpdates(Guid id);
        List<T> GetAllUpdates(List<Guid> ids);


        void Save(T updates);
        void Save(List<T> updates);

        void SortByCreated(List<T> updates);
        void SortById(List<T> updates);
        void SortByIdAndCreated(List<T> updates);
        bool TrySync();


    }
}