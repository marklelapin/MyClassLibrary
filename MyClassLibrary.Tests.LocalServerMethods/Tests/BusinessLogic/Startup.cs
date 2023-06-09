using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyClassLibrary.DataAccessMethods;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using MyClassLibrary.Tests.LocalServerMethods.Services;

namespace MyClassLibrary.Tests.LocalServerMethods.Tests.BusinessLogic
{
    public class Startup
    { 
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILocalServerEngine<TestUpdate>, MockLocalServerEngine_TestUpdate>();
            services.AddTransient<ILocalServerModelFactory<TestModel, TestUpdate>, LocalServerModelFactory<TestModel, TestUpdate>>();
        }
    }
}
