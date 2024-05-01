using JMCore.Server.Configuration.Storage;
using JMCore.Server.Services.JMCache;
using JMCore.Server.Storages.Modules.BasicModule;
using JMCore.Services.JMCache;
using JMCore.Tests.ServerT;
using JMCore.Tests.TestModelsT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace JMCore.TestsIntegrations.ServerT.DbT;

/// <summary>
/// Working with postgres database (msssql oboslete).
/// </summary>
public class DbBaseT : ServerTestBaseT
{
  private const string DbNameRemove = "JMCore_TestsIntegrations_ServerT_DbT_";
  
  protected readonly IStorageResolver StorResolver =new StorageResolver()!;
  protected string DbName { get; private set; } = null!;


  private MasterDb _masterDb = null!;

  protected string ConnectionStringPG { get; set; } = null!;
  protected string ConnectionStringMSSQL { get; set; } = null!;
  private string MasterConnectionStringPG { get; set; } = null!;
  private string MasterConnectionStringMSSQL { get; set; } = null!;

  protected string ConnectionStringMongo { get; set; } = null!;

  protected override void RegisterServices(ServiceCollection sc)
  {
    base.RegisterServices(sc);
    
    DbName = TestData.TestName.Replace(DbNameRemove, string.Empty).ToLower();

    sc.AddSingleton(StorResolver);
    sc.AddMediatR((c) => { c.RegisterServicesFromAssemblyContaining(typeof(IBasicStorageModule)); });

    ConnectionStringPG = string.Format(Configuration["TestSettings:ConnectionStringPG"] ?? throw new InvalidOperationException(), DbName);
    MasterConnectionStringPG = string.Format(Configuration["TestSettings:ConnectionStringPG"] ?? throw new InvalidOperationException(), "postgres");
    ConnectionStringMongo = Configuration["TestSettings:ConnectionStringMongo"] ?? throw new InvalidOperationException();

    // ConnectionStringMSSQL = string.Format(Configuration["TestSettings:ConnectionStringMSSQL"] ?? throw new InvalidOperationException(), _dbName);
    //MasterConnectionStringMSSQL =  string.Format(Configuration["TestSettings:ConnectionStringMSSQL"] ?? throw new InvalidOperationException(), "master");
    
    sc.AddJMMemoryCache<JMCacheServerCategory>();
    sc.AddDbContext<MasterDb>(opt => opt.UseNpgsql(MasterConnectionStringPG));
    // sc.AddDbContext<MasterDb>(opt => opt.UseSqlServer(string.Format(ConnectionStringMSSQL, "master")));
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    _masterDb = sp.GetService<MasterDb>() ?? throw new ArgumentException($"{nameof(DbBaseT)}.{nameof(MasterDb)} is null.");
    NewPGDatabase();
    NewMongoDatabase();
    await StorResolver.ConfigureStorages(sp);
    // NewSqlDatabase();
  }

  protected override async Task FinishedTestAsync()
  {
    DropPGDatabase();
    //DropSqlDatabase();
    await base.FinishedTestAsync();
  }

  private void NewMongoDatabase()
  {
    var client = new MongoClient(ConnectionStringMongo);
    client.DropDatabase(DbName);
    var db = client.GetDatabase(DbName);
   // db.CreateCollection("aa");
    // var aa = new BasicMongoDbContext (new DbContextOptionsBuilder<BasicMongoDbContext>()
    //   .UseMongoDB(client, DbName).Options);
  }

  private void NewPGDatabase()
  {
    string sql = @"
DROP DATABASE IF EXISTS " + DbName + @" WITH (FORCE);

CREATE DATABASE " + DbName + @"
    WITH OWNER = 'user'
    ENCODING = 'UTF8'
    CONNECTION LIMIT = -1;
 ";


    _masterDb.Database.ExecuteSqlRaw(sql);


    Log.LogInformation("Database '{Dbname}' has been created", DbName);
  }

  // ReSharper disable once UnusedMember.Local
  private void NewSqlDatabase()
  {
    string sql = @"
IF EXISTS 
   (
     SELECT name FROM master.dbo.sysdatabases 
    WHERE name = N'" + DbName + @"'
    )
BEGIN
   ALTER DATABASE " + DbName + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
   DROP DATABASE [" + DbName + @"];
END

CREATE DATABASE " + DbName + "; ";


    _masterDb.Database.ExecuteSqlRaw(sql);


    Log.LogInformation("Database '{Dbname}' has been created", DbName);
  }

  private void DropPGDatabase()
  {
    if (TestData.TestEnvironmentType == TestEnvironmentTypeEnum.Dev)
      return;

    var sql = "DROP DATABASE IF EXISTS " + DbName + " WITH (FORCE);";
    _masterDb.Database.ExecuteSqlRaw(sql);

    Log.LogInformation("Database '{Dbname}' has been deleted", DbName);
  }

  // ReSharper disable once UnusedMember.Local
  private void DropSqlDatabase()
  {
    if (TestData.TestEnvironmentType != TestEnvironmentTypeEnum.Dev)
    {
      var sql = @"
IF EXISTS 
   (
     SELECT name FROM master.dbo.sysdatabases 
    WHERE name = N'" + DbName + @"'
    )
BEGIN
   ALTER DATABASE " + DbName + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
   DROP DATABASE [" + DbName + @"];
END

";
      _masterDb.Database.ExecuteSqlRaw(sql);

      Log.LogInformation("Database '{Dbname}' has been deleted", DbName);
    }
  }
}

public class MasterDb : DbContext
{
  public MasterDb(DbContextOptions<MasterDb> options) : base(options)
  {
  }
}