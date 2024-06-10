using JMCore.Server.Configuration.Storage;
using JMCore.Server.Storages.Modules.AuditModule;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.TestsIntegrations.ServerT.StoragesT.TestStorageConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JMCore.TestsIntegrations.ServerT.StoragesT;

public class PGStorageRegistrationT(string dbName): IStorageRegistrationT
{
  private ILogger<PGStorageRegistrationT> _log = null!;
  private MasterDb _masterDb = null!;
  private string ConnectionStringPG { get; set; } = null!;
  private string MasterConnectionStringPG { get; set; } = null!;

  public void RegisterServices(ServiceCollection sc, IConfigurationRoot configuration, IEnumerable<string> requiredBaseStorageModules, IStorageResolver storageResolver)
  {
    ConnectionStringPG = string.Format(configuration["TestSettings:ConnectionStringPG"] ?? throw new InvalidOperationException(), dbName);
    MasterConnectionStringPG = string.Format(configuration["TestSettings:ConnectionStringPG"] ?? throw new InvalidOperationException(), "postgres");
    sc.AddDbContext<MasterDb>(opt => opt.UseNpgsql(MasterConnectionStringPG));
    storageResolver.RegisterStorage(sc, new PGTestStorageConfiguration(ConnectionStringPG, requiredBaseStorageModules));
  }

  public void GetServices(IServiceProvider sp)
  {
    _log =  sp.GetService<ILogger<PGStorageRegistrationT>>() ?? throw new ArgumentException($"{nameof(ILogger<PGStorageRegistrationT>)} is null.");
    _masterDb = sp.GetService<MasterDb>() ?? throw new ArgumentException($"{nameof(PGStorageRegistrationT)}.{nameof(MasterDb)} is null.");
    NewPGDatabase();
  }

  public void FinishedTest()
  {
    DropPGDatabase();
  }

  private void NewPGDatabase()
  {
    string sql = @"
DROP DATABASE IF EXISTS " + dbName + @" WITH (FORCE);

CREATE DATABASE " + dbName + @"
    WITH OWNER = 'user'
    ENCODING = 'UTF8'
    CONNECTION LIMIT = -1;
 ";


    _masterDb.Database.ExecuteSqlRaw(sql);


    _log.LogInformation("Database '{Dbname}' has been created", dbName);
  }
  
  private void DropPGDatabase()
  {
    // if (TestData.TestEnvironmentType == TestEnvironmentTypeEnum.Dev)
    //   return;

    var sql = "DROP DATABASE IF EXISTS " + dbName + " WITH (FORCE);";
    _masterDb.Database.ExecuteSqlRaw(sql);

    _log.LogInformation("Database '{Dbname}' has been deleted", dbName);
  }

}