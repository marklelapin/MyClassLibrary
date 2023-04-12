
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    internal class ConnectionStringDictionary
    {
        private IConfiguration? Config { get; set; }
        


        internal string LocalSQL { get; private set; }

        internal string ServerSQL { get; private set; }

        internal string TestConnectionString { get; private set; }

        internal ConnectionStringDictionary()
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
