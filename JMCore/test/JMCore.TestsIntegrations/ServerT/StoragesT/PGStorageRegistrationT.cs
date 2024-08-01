using JMCore.Server.Storages;
using JMCore.Server.Storages.Configuration;
using JMCore.Tests.TestModelsT;
using JMCore.TestsIntegrations.ServerT.StoragesT.TestStorageConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JMCore.TestsIntegrations.ServerT.StoragesT;

public class PGStorageRegistrationT(TestData testData) : IStorageRegistrationT
{
  private ILogger<PGStorageRegistrationT> _log = null!;
  private MasterDb _masterDb = null!;
  private string _dbName = testData.GetDbName();
  
  private string ConnectionStringPG { get; set; } = null!;
  private string MasterConnectionStringPG { get; set; } = null!;

  public void RegisterServices(ServiceCollection sc, IConfigurationRoot configuration, IEnumerable<string> requiredBaseStorageModules, IStorageResolver storageResolver)
  {
    ConnectionStringPG = string.Format(configuration["TestSettings:ConnectionStringPG"] ?? throw new InvalidOperationException(), _dbName);
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
    if (!testData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Create))
      return;
    
    string sql = @"
DROP DATABASE IF EXISTS " + _dbName + @" WITH (FORCE);

CREATE DATABASE " + _dbName + @"
    WITH OWNER = 'user'
    ENCODING = 'UTF8'
    CONNECTION LIMIT = -1;
 ";


    _masterDb.Database.ExecuteSqlRaw(sql);


    _log.LogInformation("Database '{Dbname}' has been created", _dbName);
  }
  
  private void DropPGDatabase()
  {
    if (!testData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Drop))
      return;
    
    var sql = "DROP DATABASE IF EXISTS " + _dbName + " WITH (FORCE);";
    _masterDb.Database.ExecuteSqlRaw(sql);

    _log.LogInformation("Database '{Dbname}' has been deleted", _dbName);
  }

}