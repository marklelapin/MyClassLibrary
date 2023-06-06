using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    /// <summary>
    /// The interface providing the model by which the Local Server system presents collections of updates by Id. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILocalServerModel<T> where T : ILocalServerModelUpdate
    {
        List<T>? Conflicts { get; set; }
        List<T>? History { get; set; }
        Guid Id { get; }
        T? Latest { get; set; }
    }
}