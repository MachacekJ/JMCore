using ACore.AppTest.Modules.TestModule.CQRS;
using ACore.AppTest.Modules.TestModule.Storages;
using ACore.AppTest.Modules.TestModule.Storages.EF;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.AppTest.Modules.TestModule;

public static class TestModuleServiceExtensions
{
  public static void AddTestServiceModule(this IServiceCollection services)
  {
    services.AddMediatR((c) =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(IEFTestStorageModule));
    });
    
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TestModuleBehavior<,>));
  }
  
  public static void UseTestServiceModule(this IServiceProvider provider)
  {

  }
}