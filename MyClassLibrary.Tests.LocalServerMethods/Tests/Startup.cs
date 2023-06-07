using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyClassLibrary.DataAccessMethods;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using MyClassLibrary.Tests.LocalServerMethods.Services;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests
{
    public class Startup 
    {

        //public void ConfigureHost(IHostBuilder hostBuilder) =>
        //    hostBuilder
        ////hostBuilder.ConfigureWebHost(webHostBuilder => webHostBuilder
        //    .UseTestServer(options => options.PreserveExecutionContext = true
        //    )
        //    .UseStartup<AspNetCoreStartup>());

        public void ConfigureHost(IHostBuilder hostBuilder) =>
           hostBuilder
           .ConfigureHostConfiguration(builder =>
           {

               builder.SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json")
                                 .AddUserSecrets<Startup>();
           });
        
      

        public void ConfigureServices(IServiceCollection services)
                    {
                        services.AddTransient<ISqlDataAccess,SqlDataAccess>();
                        services.AddTransient(typeof(ILocalDataAccess<TestUpdate>),typeof(LocalSQLConnector<TestUpdate>));
                        services.AddTransient(typeof(IServerDataAccess<TestUpdate>), typeof(ServerSQLConnector<TestUpdate>));
                        services.AddTransient(typeof(ILocalServerEngine<TestUpdate>), typeof(LocalServerEngine<TestUpdate>));
                        services.AddTransient(typeof(ISaveAndGetUpdateTypeTests<TestUpdate>),typeof(SaveAndGetUpdateTypeTestService<TestUpdate>));
                        services.AddTransient(typeof(ISaveAndGetTestContent<TestUpdate>),typeof(SaveAndGetTestUpdateContent));            
        }

        

    }
}
