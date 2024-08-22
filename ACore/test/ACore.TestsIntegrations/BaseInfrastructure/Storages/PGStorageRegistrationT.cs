using ACore.Server.Configuration;
using ACore.Tests.BaseInfrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ACore.TestsIntegrations.BaseInfrastructure.Storages;

public class PGStorageRegistrationT(TestData testData) : IStorageRegistrationT
{
  private ILogger<PGStorageRegistrationT>? _log;
  private MasterDb? _masterDb;
  private readonly string _dbName = testData.GetDbName();
  
  private string? MasterConnectionStringPG { get; set; }

  public void RegisterServices(ServiceCollection sc, StorageModuleConfiguration config)
  {
    MasterConnectionStringPG = string.Format(config.PGDb?.ReadWriteConnectionString ?? throw new InvalidOperationException(), "postgres");
    sc.AddDbContext<MasterDb>(opt => opt.UseNpgsql(MasterConnectionStringPG));
  }

  public void GetServices(IServiceProvider sp)
  {
    _log = sp.GetService<ILogger<PGStorageRegistrationT>>() ?? throw new ArgumentException($"{nameof(ILogger<PGStorageRegistrationT>)} is null.");
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


    _masterDb?.Database.ExecuteSqlRaw(sql);


    _log?.LogInformation("Database '{Dbname}' has been created", _dbName);
  }

  private void DropPGDatabase()
  {
    if (!testData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Drop))
      return;

    var sql = "DROP DATABASE IF EXISTS " + _dbName + " WITH (FORCE);";
    _masterDb?.Database.ExecuteSqlRaw(sql);

    _log?.LogInformation("Database '{Dbname}' has been deleted", _dbName);
  }
}