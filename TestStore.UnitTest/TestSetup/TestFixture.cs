using AutoMapper;
using GodwinStore.Api.Services.CustomerServices;
using GodwinStore.Api.Services.OrderServices;
using GodwinStore.Api.Services.ProductServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestStore.UnitTest.TestSetup;

public class TestFixture
{
    public ServiceProvider ServiceProvider { get; }
    
    public TestFixture()
    {
        var services = new ServiceCollection();
        ConfigurationManager.SetupConfiguration();

        services.AddSingleton(sp => ConfigurationManager.Configuration);

        services.AddLogging(x => x.AddConsole());
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<IProductServices, ProductServices>();
        services.AddScoped<ICustomerServices, CustomerServices>();
        services.AddScoped<IOrderServices, OrderServices>();
        
        ServiceProvider = services.BuildServiceProvider();
    }
}