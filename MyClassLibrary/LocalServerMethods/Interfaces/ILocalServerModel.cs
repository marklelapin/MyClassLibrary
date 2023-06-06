using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Interfaces
{
    /// <summary>
    /// The interface providing the model by which the Local Server system presents collections of updates by Id. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILocalServerModel<T> where T : ILocalServerModelUpdate
    {
        /// <summary>
        /// A list of Updates that conflict with the Latest Update. i.e. that were created on different local machines without the knowledge of each one.
        /// </summary>
        List<T>? Conflicts { get; set; }
        /// <summary>
        /// All of the updates relating to the model.
        /// </summary>
        List<T>? History { get; set; }
        /// <summary>
        /// The unique Id identifying the model.
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// The latest update relating to the model.
        /// </summary>
        T? Latest { get; set; }
    }
}