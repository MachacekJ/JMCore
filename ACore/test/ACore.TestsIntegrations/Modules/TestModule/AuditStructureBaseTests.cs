using ACore.AppTest.Modules.TestModule;
using ACore.AppTest.Modules.TestModule.Storages.Mongo;
using ACore.AppTest.Modules.TestModule.Storages.PG;
using ACore.Server.Configuration;
using ACore.Server.Modules.AuditModule.Configuration;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Server.Storages.Models;
using ACore.Tests.Server.Modules.TestModule;
using ACore.TestsIntegrations.BaseInfrastructure.Storages;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace ACore.TestsIntegrations.Modules.TestModule;

public abstract class AuditStructureBaseTests : StorageBaseTests
{
  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    sc.AddScoped<IAuditConfiguration, AuditConfiguration>();
    sc.AddSingleton<IAuditUserProvider>(TestAuditUserProvider.CreateDefaultUser());
    sc.AddTestServiceModule(GetTestConfig(TestData.GetDbName()));
  }

  protected override void RegisterAutofacContainer(ServiceCollection services, ContainerBuilder containerBuilder)
  {
    base.RegisterAutofacContainer(services, containerBuilder);
    containerBuilder.RegisterAutofacTestService();
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    await sp.UseTestServiceModule(GetTestConfig(TestData.GetDbName()));
  }

  protected static string GetTestTableName(StorageTypeEnum storageType, string entityName)
  {
    return storageType switch
    {
      StorageTypeEnum.Memory => entityName,
      StorageTypeEnum.Mongo => MongoTestStorageDbNames.ObjectNameMapping[entityName].TableName,
      StorageTypeEnum.Postgres => PGTestStorageDbNames.ObjectNameMapping[entityName].TableName,
      _ => throw new Exception($"Register name of table '{Enum.GetName(storageType.GetType(), storageType)}' for '{entityName}'.")
    };
  }

  protected static string GetTestColumnName(StorageTypeEnum storageType, string entityName, string propName)
  {
    return storageType switch
    {
      StorageTypeEnum.Memory => entityName,
      StorageTypeEnum.Mongo => MongoTestStorageDbNames.ObjectNameMapping[entityName].ColumnNames[propName],
      StorageTypeEnum.Postgres => PGTestStorageDbNames.ObjectNameMapping[entityName].ColumnNames[propName],
      _ => throw new Exception($"Register name of table '{Enum.GetName(storageType.GetType(), storageType)}' for '{entityName}'.")
    };
  }
}