using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    public interface ILocalServerModelFactory<T, U>
        where T : LocalServerModel<U>, new()
        where U : LocalServerModelUpdate, new()
    {
        Task<T> CreateModel(Guid? Id = null);
        Task<List<T>> CreateModelsList(List<Guid>? ids = null);
    }


}