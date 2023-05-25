using Microsoft.Extensions.Configuration;
using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.Tests.LocalServerMethods.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    public interface IServiceConfiguration<T> where T : LocalServerModelUpdate
    {
        public IConfiguration Config { get; }
        
        public ILocalDataAccess<T> LocalDataAccess();

        public IServerDataAccess<T> ServerDataAccess();

        public ILocalDataAccessTests<T> LocalDataAccessTests();

        public IServerDataAccessTests<T> ServerDataAccessTests();

        public ITestContent<T> TestContent();

        public ILocalServerEngine<T> LocalServerEngine(ILocalDataAccess<T> localDataAccess,IServerDataAccess<T> serverDataAccess);


    }
}
