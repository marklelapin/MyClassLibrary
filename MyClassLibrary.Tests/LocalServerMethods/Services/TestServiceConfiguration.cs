using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MyClassLibrary.DataAccessMethods;
using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.Tests.LocalServerMethods;
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

        public IConfiguration Config { get; private set; }

        public TestServiceConfiguration()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets<TestServiceConfiguration>();
            Config = builder.Build();
        }

        public ILocalDataAccess LocalDataAccess() { return new LocalSQLConnector(new SqlDataAccess(Config)); }

        public IServerDataAccess ServerDataAccess() { return new ServerSQLConnector(new SqlDataAccess(Config)); }

        public ILocalDataAccessTests<T> LocalDataAccessTests<T>() where T : LocalServerIdentityUpdate { return new LocalDataAccessTestsService<T>(this); }

        public IServerDataAccessTests<T> ServerDataAccessTests<T>() where T : LocalServerIdentityUpdate { return new ServerDataAccessTestsService<T>(this); }

        public ITestContent<T> TestContent<T>() where T : LocalServerIdentityUpdate { return new TestContentService<T>(); }

        public ILocalServerEngine<T> LocalServerEngine<T>(ILocalDataAccess localDataAccess, IServerDataAccess serverDataAccess) where T : LocalServerIdentityUpdate
        {
            return new LocalServerEngine<T>(serverDataAccess, localDataAccess);
        }
    }
}

