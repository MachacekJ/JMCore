using ACore.Server.Configuration;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Tests.Base.Models;
using ACore.Tests.Server.Storages;
using ACore.Tests.Server.TestImplementations.Server;
using ACore.Tests.Server.TestImplementations.Server.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ACore.Tests.Server.Modules.AuditModule.PG;

public class MasterDb(DbContextOptions<MasterDb> options) : DbContext(options);

public class AuditTestsBase : StorageTestsBase
{
  protected IAuditUserProvider UserProvider = TestAuditUserProvider.CreateDefaultUser();
  private MasterDb? _masterDb;
  private ILogger<AuditTestsBase>? _log;
  private string _dbName = string.Empty;

  protected override void RegisterServices(ServiceCollection sc)
  {
    _dbName = TestData.GetDbName();
    base.RegisterServices(sc);
    sc.AddDbContext<MasterDb>(opt => opt.UseNpgsql(string.Format(Configuration?["TestSettings:ConnectionStringPG"] ?? throw new InvalidOperationException(), "postgres")));
    sc.AddACoreTest(ot =>
    {
      ot.AddTestModule()
        .ACoreServer(o =>
        {
          PGStorageConfiguration.Invoke(o, string.Format(Configuration?["TestSettings:ConnectionStringPG"] ?? throw new InvalidOperationException(), _dbName));
          o.AddAuditModule(a => { a.UserProvider(UserProvider); });
        });
    });
  }

  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    _masterDb = sp.GetService<MasterDb>() ?? throw new ArgumentException($"{nameof(AuditTestsBase)}.{nameof(MasterDb)} is null.");
    _log = sp.GetService<ILogger<AuditTestsBase>>() ?? throw new ArgumentException($"{nameof(ILogger<AuditTestsBase>)} is null.");
    await NewPGDatabase();

    await sp.UseACoreTest();
    UserProvider = sp.GetService<IOptions<ACoreServerOptions>>()?.Value.AuditModuleOptions.AuditUserProvider ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
  }

  protected string GetTableName(string entityName)
  {
    return TestImplementations.Server.Modules.TestModule.Storages.SQL.PG.DefaultNames.ObjectNameMapping[entityName].TableName;
  }

  protected string GetColumnName(string entityName, string propertyName)
  {
    return TestImplementations.Server.Modules.TestModule.Storages.SQL.PG.DefaultNames.ObjectNameMapping[entityName].ColumnNames?[propertyName] ?? throw new InvalidDataException("Define column names for PG.");
  }

  protected override async Task FinishedTestAsync()
  {
    await DropPGDatabase();
  }


  private async Task NewPGDatabase()
  {
    if (!TestData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Create))
      return;

    string sql = @"
DROP DATABASE IF EXISTS " + _dbName + @" WITH (FORCE);

CREATE DATABASE " + _dbName + @"
    WITH OWNER = 'user'
    ENCODING = 'UTF8'
    CONNECTION LIMIT = -1;
 ";

    if (_masterDb != null)
      await _masterDb.Database.ExecuteSqlRawAsync(sql);

    _log?.LogInformation("Database '{Dbname}' has been created", _dbName);
  }

  private async Task DropPGDatabase()
  {
    if (!TestData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Drop))
      return;

    var sql = "DROP DATABASE IF EXISTS " + _dbName + " WITH (FORCE);";

    if (_masterDb != null)
      await _masterDb.Database.ExecuteSqlRawAsync(sql);

    _log?.LogInformation("Database '{Dbname}' has been deleted", _dbName);
  }
}