
using MyClassLibrary.LocalServerMethods.Models;


namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    public interface ITestContent<T> where T : LocalServerModelUpdate 
    {
        public List<List<T>> Generate(int quantity, string testType, List<Guid>? overrideIds = null, DateTime? overrideCreated = null);

        public List<Guid> ListIds(List<T> updates);
        // public DateTime CreatedDate { get;}

    }
}
