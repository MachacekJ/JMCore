using ACore.Base.CQRS;
using ACore.Server.Configuration;
using ACore.Tests.TestImplementations.Server.Modules.TestModule.Configuration;
using Autofac;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ACore.Tests.TestImplementations.Server.Configuration;

public static class ACoreTestServiceExtensions
{
  public static void AddACoreTest(this IServiceCollection services, Action<ACoreTestOptionsBuilder>? optionsBuilder = null)
  {
    var aCoreTestOptionsBuilder = ACoreTestOptionsBuilder.Empty();
    optionsBuilder?.Invoke(aCoreTestOptionsBuilder);
    var oo = aCoreTestOptionsBuilder.Build();
    AddACoreTest(services, oo);
  }

  private static void AddACoreTest(this IServiceCollection services, ACoreTestOptions aCoreTestOptions)
  {
    ValidateDependencyInConfiguration(aCoreTestOptions);

    services.AddACoreServer(aCoreTestOptions.ACoreServerOptions);

    var myOptionsInstance = Options.Create(aCoreTestOptions);
    services.AddSingleton(myOptionsInstance);

    // Adding CQRS from ACore.Tests assembly.
    services.AddMediatR((c) =>
    {
      c.RegisterServicesFromAssemblyContaining(typeof(ACoreTestServiceExtensions));
      c.ACoreMediatorConfiguration();
    });
    services.AddValidatorsFromAssembly(typeof(ACoreTestServiceExtensions).Assembly, includeInternalTypes: true);

    if (aCoreTestOptions.TestModuleOptions.IsActive)
      services.AddTestModule(aCoreTestOptions.TestModuleOptions);
  }

  public static async Task UseACoreTest(this IServiceProvider provider)
  {
    var opt = provider.GetService<IOptions<ACoreTestOptions>>()?.Value ?? throw new Exception($"{nameof(ACoreTestOptions)} is not registered.");
    await provider.UseACoreServer();

    if (opt.TestModuleOptions.IsActive)
      await provider.UseTestModule();
  }

  public static void AddACoreTest(this ContainerBuilder containerBuilder)
  {
    containerBuilder.ConfigureAutofacTestModule();
  }

  private static void ValidateDependencyInConfiguration(ACoreTestOptions aCoreTestOptions)
  {
    ValidateTestModuleOptions(aCoreTestOptions);
  }

  private static void ValidateTestModuleOptions(ACoreTestOptions aCoreTestOptions)
  {
    if (!aCoreTestOptions.TestModuleOptions.IsActive)
      return;

    if (aCoreTestOptions.ACoreServerOptions.AuditModuleOptions.IsActive == false)
      throw new Exception($"Module {nameof(ACore.Server.Modules.AuditModule)} must be activated.");
  }
}