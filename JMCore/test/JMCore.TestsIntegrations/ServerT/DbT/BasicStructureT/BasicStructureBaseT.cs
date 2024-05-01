using JMCore.Server.MongoStorage;
using JMCore.Server.PGStorage;
using JMCore.Server.Storages.Modules;
using JMCore.Server.Storages.Modules.BasicModule;
using Microsoft.Extensions.DependencyInjection;

namespace JMCore.TestsIntegrations.ServerT.DbT.BasicStructureT;

public class BasicStructureBaseT : DbBaseT
{
  protected List< IBasicStorageModule> AllDbStorages = null!;


  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    StorResolver.RegisterStorage(sc, new PGStorageConfiguration(ConnectionStringPG, StorageNativeModuleTypeEnum.BasicModule));
    //StorResolver.RegisterStorage(sc, new MemoryStorageConfiguration("test"));
    //StorResolver.RegisterStorage(sc, new MongoStorageConfiguration(ConnectionStringMongo, DbName));
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    AllDbStorages = StorResolver.StorageModuleImplementations<IBasicStorageModule>();
  }
}