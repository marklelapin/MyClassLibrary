using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Services
{


    public class TestServiceConfiguration : IServiceConfiguration
    {

        private ConnectionStringDictionary connectionStringDictionary = new ConnectionStringDictionary();

        public ILocalDataAccess LocalDataAccess() { return new LocalSQLConnector(connectionStringDictionary.LocalSQL); }

        public IServerDataAccess ServerDataAccess() { return new ServerSQLConnector(connectionStringDictionary.ServerSQL); }

        public ILocalDataAccessTests<T> LocalDataAccessTests<T>() where T : LocalServerIdentityUpdate { return new LocalDataAccessTestsService<T>(this); }

        public IServerDataAccessTests<T> ServerDataAccessTests<T>() where T : LocalServerIdentityUpdate { return new ServerDataAccessTestsService<T>(this); }

        public ITestContent<T> TestContent<T>() where T : LocalServerIdentityUpdate { return new TestContentService<T>(); }

        public ILocalServerEngine<T> LocalServerEngine<T>(ILocalDataAccess localDataAccess, IServerDataAccess serverDataAccess) where T : LocalServerIdentityUpdate
        {
            return new LocalServerEngine<T>(serverDataAccess, localDataAccess);
        }
    }
}

