using ACore.Server.Configuration;
using ACore.Server.Modules.AuditModule.UserProvider;
using ACore.Server.Storages.Definitions.Models;
using ACore.Tests.Base.Models;
using ACore.Tests.Server.Storages;
using ACore.Tests.Server.TestImplementations.Server;
using ACore.Tests.Server.TestImplementations.Server.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ACore.Tests.Server.Modules.AuditModule.Mongo;

public class MasterDb(DbContextOptions<MasterDb> options) : DbContext(options);

public class AuditTestsBase : StorageTestsBase
{
  protected IAuditUserProvider UserProvider = TestAuditUserProvider.CreateDefaultUser();
  private ILogger<AuditTestsBase>? _log;
  private string _dbName = string.Empty;
  private string _connectionString = string.Empty;

  protected override void RegisterServices(ServiceCollection sc)
  {
    _dbName = TestData.GetDbName(StorageTypeEnum.Mongo);
    base.RegisterServices(sc);
    _connectionString = Configuration?["TestSettings:ConnectionStringMongo"] ?? throw new InvalidOperationException();
    sc.AddACoreTest(ot =>
    {
      ot.AddTestModule()
        .ACoreServer(o =>
        {
          MongoStorageConfiguration.Invoke(o, _connectionString, _dbName);
          o.AddAuditModule(a => { a.UserProvider(UserProvider); });
        });
    });
  }

  protected override async Task GetServices(IServiceProvider sp)
  {
    await base.GetServices(sp);
    _log = sp.GetService<ILogger<AuditTestsBase>>() ?? throw new ArgumentException($"{nameof(ILogger<AuditTestsBase>)} is null.");
    await NewMongoDatabase();

    await sp.UseACoreTest();
    UserProvider = sp.GetService<IOptions<ACoreServerOptions>>()?.Value.AuditModuleOptions.AuditUserProvider ?? throw new ArgumentException($"{nameof(IAuditUserProvider)} is null.");
  }

  protected string GetTableName(string entityName)
  {
    return TestImplementations.Server.Modules.TestModule.Storages.Mongo.DefaultNames.ObjectNameMapping[entityName].TableName;
  }

  protected string GetColumnName(string entityName, string propertyName)
  {
    return TestImplementations.Server.Modules.TestModule.Storages.Mongo.DefaultNames.ObjectNameMapping[entityName].ColumnNames?[propertyName] ?? throw new InvalidDataException("Define column names for Mongo.");
  }

  protected override async Task FinishedTestAsync()
  {
    await DropDatabase();
  }


  private async Task DropDatabase()
  {
    if (!TestData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Drop))
      return;

    var client = new MongoClient(_connectionString);
    await client.DropDatabaseAsync(_dbName);
    _log?.LogInformation("Mongo database '{Dbname}' has been deleted", _dbName);
  }

  private async Task NewMongoDatabase()
  {
    if (!TestData.DatabaseManipulation.HasFlag(DatabaseManipulationEnum.Create))
      return;

    var client = new MongoClient(_connectionString);
    await client.DropDatabaseAsync(_dbName);
    _log?.LogInformation("Mongo database '{Dbname}' has been created.", _dbName);
  }
}