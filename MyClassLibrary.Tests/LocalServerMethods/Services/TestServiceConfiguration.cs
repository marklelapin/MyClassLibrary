using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MyClassLibrary.DataAccessMethods;
using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.Tests.LocalServerMethods;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Services
{


    public class TestServiceConfiguration<T> : IServiceConfiguration<T> where T : LocalServerModelUpdate, new()
    {

        private ConnectionStringDictionary connectionStringDictionary = new ConnectionStringDictionary();

        public IConfiguration Config { get; private set; }

        public TestServiceConfiguration()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets<TestServiceConfiguration<T>>();
            Config = builder.Build();
        }

        public ILocalDataAccess<T> LocalDataAccess() { return new LocalSQLConnector<T>(new SqlDataAccess(Config)); }

        public IServerDataAccess<T> ServerDataAccess() { return new ServerSQLConnector<T>(new SqlDataAccess(Config)); }

        public ILocalDataAccessTests<T> LocalDataAccessTests()  { return new LocalDataAccessTestsService<T>(this); }

        public IServerDataAccessTests<T> ServerDataAccessTests()  { return new ServerDataAccessTestsService<T>(this); }

        public ITestContent<T> TestContent()  { return new TestContentService<T>(); }

        public ILocalServerEngine<T> LocalServerEngine(ILocalDataAccess<T> localDataAccess, IServerDataAccess<T> serverDataAccess) 
        {
            return new LocalServerEngine<T>(serverDataAccess, localDataAccess);
        }
    }
}

