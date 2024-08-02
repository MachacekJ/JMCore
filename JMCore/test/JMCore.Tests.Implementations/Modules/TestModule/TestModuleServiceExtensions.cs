using JMCore.Tests.Implementations.Modules.TestModule.CQRS;
using JMCore.Tests.Implementations.Modules.TestModule.Storages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.Tests.Implementations.Modules.TestModule;

public static class TestModuleServiceExtensions
{
  public static void AddTestServiceModule(this IServiceCollection services)
  {
    services.AddMediatR((c) =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(ITestStorageModule));
    });
    
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TestModuleBehavior<,>));
  }
}