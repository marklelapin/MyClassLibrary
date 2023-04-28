using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyClassLibrary.LocalServerMethods;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using MyClassLibrary.Tests.LocalServerMethods.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods
{


    public class ServiceConfiguration : IServiceConfiguration
    {

        private static readonly ConnectionStringDictionary connectionStringDictionary = new ConnectionStringDictionary();

        public ILocalDataAccess LocalDataAccess() { return new LocalSQLConnector(connectionStringDictionary.LocalSQL); }

        public IServerDataAccess ServerDataAccess() { return new ServerSQLConnector(connectionStringDictionary.ServerSQL); }

        public ILocalDataAccessTests LocalDataAccessTests() { return new LocalDataAccessTestsService() ; }

        public IServerDataAccessTests ServerDataAccessTests() { return new ServerDataAccessTestsService() ; }

        public ITestContent TestContent() { return new TestContentService(); }

    }

     
}
