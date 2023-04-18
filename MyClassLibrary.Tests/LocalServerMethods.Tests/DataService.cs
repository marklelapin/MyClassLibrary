using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyClassLibrary.LocalServerMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    

    public class DataService
    {

        private static readonly ConnectionStringDictionary connectionStringDictionary = new ConnectionStringDictionary();

        public ILocalDataAccess localDataAccess { get; set; } = new LocalSQLConnector(connectionStringDictionary.LocalSQL);

        public IServerDataAccess serverDataAccess { get; set; } = new ServerSQLConnector(connectionStringDictionary.ServerSQL);
    
    }

     
}
