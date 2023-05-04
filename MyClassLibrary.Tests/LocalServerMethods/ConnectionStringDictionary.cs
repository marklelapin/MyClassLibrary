
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods
{
    public class ConnectionStringDictionary
    {
        private IConfiguration? Config { get; set; }
        


        public string LocalSQL { get; private set; }

        public string ServerSQL { get; private set; }

        public string TestConnectionString { get; private set; }

        public ConnectionStringDictionary()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                Config = builder.Build(); 

            LocalSQL = Config?.GetConnectionString("LocalSQL") ?? String.Empty;
            ServerSQL = Config?.GetConnectionString("ServerSQL") ?? String.Empty;
            TestConnectionString = Config?.GetConnectionString("TestConnectionString") ?? string.Empty;
        } 

    }
}
