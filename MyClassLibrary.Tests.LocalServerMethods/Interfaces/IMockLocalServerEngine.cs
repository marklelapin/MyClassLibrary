using MyClassLibrary.LocalServerMethods.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    internal interface IMockLocalServerEngine<T> : ILocalServerEngine<T> where T : ILocalServerModelUpdate
    {


    }
}
