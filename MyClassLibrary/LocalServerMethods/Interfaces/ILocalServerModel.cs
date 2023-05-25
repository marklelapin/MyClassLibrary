using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    public interface ILocalServerModel<T> where T : LocalServerModelUpdate
    {
        List<T>? Conflicts { get; set; }
        List<T>? History { get; set; }
        Guid Id { get; }
        T? Latest { get; set; }

        Task DeActivate();
        Task ReActivate(T update);
        Task ResolveConflict(T update);
        Task Restore(T update);
        Task Update(T update);
    }
}