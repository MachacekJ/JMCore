using JMCore.Server.DataStorages.PG.BasicStructure;
using JMCore.Server.DB.Audit;
using JMCore.Server.DB.DbContexts.AuditStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JMCore.Server.DataStorages.Configuration;

public enum SupportedStoragesEnum
{
  Postgres,
  Mongo,
  MSSQL
}

public class DbContextConfigData(SupportedStoragesEnum storageType, Dictionary<string, DB.Abstract.DbContextBase> otherStructures, string cn)
{
  public SupportedStoragesEnum StorageType { get; } = storageType;
  public Dictionary<string, DbContextBase> OtherStructures { get; } = otherStructures;
  public bool LanguageStructure { get; private set; } = false;
  public bool AuditStructure { get; private set; }
  public string ConnectionString { get; } = cn;
  public IAuditEntitiesConfiguration AuditEntitiesConfiguration { get; private set; } = new AuditEntitiesConfiguration();

  public void EnableAudit(IAuditEntitiesConfiguration auditEntitiesConfiguration)
  {
    AuditStructure = true;
    AuditEntitiesConfiguration = auditEntitiesConfiguration;
  }

  public void EnableLocalization()
  {
    LanguageStructure = true;
  }
}

public class JMStorageConfigurationBuilder(DbContextConfigData primaryDb, params DbContextConfigData[] secondaryDb)
{
  private JMDbContextConfiguration _aa = new();

  public void RegisterServices(IServiceCollection services)
  {
    services.AddMediatR((c) => { c.RegisterServicesFromAssemblyContaining(typeof(IBasicDbContext)); });
    RegisterBasicDbContext(primaryDb, services);
    RegisterAuditStructure(primaryDb, services);
    RegisterLanguageStructure(primaryDb, services);
    foreach (var config in secondaryDb)
    {
      RegisterBasicDbContext(config, services);
      RegisterAuditStructure(config, services);
      RegisterLanguageStructure(config, services);
    }
  }

  public void ConfigureServices(IServiceProvider serviceProvider)
  {
  }

  private void RegisterLanguageStructure(DbContextConfigData config, IServiceCollection services)
  {
    if (!config.LanguageStructure)
      return;
    switch (config.StorageType)
    {
      case SupportedStoragesEnum.Postgres:
        services.AddDbContext<LocalizeDbContext>(opt => opt.UseNpgsql(config.ConnectionString));
        break;
      case SupportedStoragesEnum.Mongo:
      case SupportedStoragesEnum.MSSQL:
      default:
        throw new NotImplementedException($"{nameof(IBasicDbContext)} for {Enum.GetName(typeof(SupportedStoragesEnum), config.StorageType)}.");
    }
  }

  private void RegisterAuditStructure(DbContextConfigData config, IServiceCollection services)
  {
    if (!config.AuditStructure)
      return;
    if (config.AuditEntitiesConfiguration == null)
      throw new ArgumentException($"{nameof(AuditEntitiesConfiguration)} is not set");

    switch (config.StorageType)
    {
      case SupportedStoragesEnum.Postgres:
        services.AddDbContext<AuditDbContext>(opt => opt.UseNpgsql(config.ConnectionString));
        AddToConfig(nameof(IAuditDbContext), typeof(AuditDbContext));
        break;
      case SupportedStoragesEnum.Mongo:
      case SupportedStoragesEnum.MSSQL:
      default:
        throw new NotImplementedException($"{nameof(IBasicDbContext)} for {Enum.GetName(typeof(SupportedStoragesEnum), config.StorageType)}.");
    }

    services.TryAddScoped<IAuditDbService, AuditDbService>();
    services.TryAddSingleton<IAuditUserProvider, AuditEmptyUserProvider>();

    var auditConfiguration = config.AuditEntitiesConfiguration;
    services.TryAddSingleton(auditConfiguration);
  }

  private void RegisterBasicDbContext(DbContextConfigData config, IServiceCollection services)
  {
    switch (config.StorageType)
    {
      case SupportedStoragesEnum.Postgres:
        services.AddDbContext<BasicPGDbContext>(opt => opt.UseNpgsql(primaryDb.ConnectionString));
        AddToConfig(nameof(IBasicDbContext), typeof(BasicPGDbContext));
        break;
      case SupportedStoragesEnum.Mongo:
      case SupportedStoragesEnum.MSSQL:
      default:
        throw new NotImplementedException($"{nameof(IBasicDbContext)} for {Enum.GetName(typeof(SupportedStoragesEnum), primaryDb.StorageType)}.");
    }
  }

  private void AddToConfig(string name, Type db)
  {
    if (_aa.AllContexts.TryGetValue(name, out var rr))
    {
      var exists = rr.Any(r => r.GetType() == db);
      if (!exists)
        rr.Add(db);
    }
    else
    {
      _aa.AllContexts.Add(name, [db]);
    }
  }
}

public delegate IBasicDbContext BasicDbContextResolver(string storageName);