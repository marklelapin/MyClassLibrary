using MyClassLibrary.LocalServerMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    public interface ITestContent<T> where T : LocalServerIdentityUpdate 
    {
        public List<List<T>> Generate(int quantity, string testType, List<Guid>? overrideIds = null, DateTime? overrideCreated = null);

        public List<Guid> ListIds(List<T> updates);
        // public DateTime CreatedDate { get;}

    }
}
