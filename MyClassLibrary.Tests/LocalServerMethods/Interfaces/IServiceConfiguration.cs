using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.Tests.LocalServerMethods.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Interfaces
{
    public interface IServiceConfiguration
    {
        public ILocalDataAccess LocalDataAccess();

        public IServerDataAccess ServerDataAccess();

        public ILocalDataAccessTests<T> LocalDataAccessTests<T>() where T : LocalServerIdentityUpdate;

        public IServerDataAccessTests<T> ServerDataAccessTests<T>() where T : LocalServerIdentityUpdate;

        public ITestContent<T> TestContent<T>() where T : LocalServerIdentityUpdate;

        public ILocalServerEngine<T> LocalServerEngine<T>  (ILocalDataAccess localDataAccess,IServerDataAccess serverDataAccess) where T : LocalServerIdentityUpdate;


    }
}
