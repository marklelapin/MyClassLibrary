
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    internal class ConnectionString

    {
        private IConfiguration? _config;

        private IConfiguration? Config
        {
            get { return _config ; }
            set {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                _config = builder.Build(); 
            }
        }

        private string? _localSQL;

        public string LocalSql
        {
            get { return _localSQL ?? string.Empty; }
            set { _localSQL = Config?.GetConnectionString("LocalSQL") ?? String.Empty; }
        }

        private string? _serverSQL;

        public string ServerSQL
        {
            get { return _serverSQL ?? string.Empty; }
            set { _serverSQL = Config?.GetConnectionString("ServerSQL") ?? String.Empty; }
        }


    }
}
