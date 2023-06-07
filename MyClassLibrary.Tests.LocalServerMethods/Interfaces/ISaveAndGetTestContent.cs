using MyClassLibrary.LocalServerMethods.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    public interface ISaveAndGetTestContent<T> where T : ILocalServerModelUpdate
    {
        List<T> getNewUpdates();

        Guid CopyId { get; }
    }
}
