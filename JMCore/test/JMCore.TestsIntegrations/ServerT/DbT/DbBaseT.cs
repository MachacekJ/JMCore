using JMCore.Server.Services.JMCache;
using JMCore.Services.JMCache;
using JMCore.Tests.ServerT;
using JMCore.Tests.TestModelsT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JMCore.TestsIntegrations.ServerT.DbT;

/// <summary>
/// Working with postgres database (msssql oboslete).
/// </summary>
public class DbBaseT : ServerTestBaseT
{
  private const string DbNameRemove = "JMCore_TestsIntegrations_ServerT_DbT_";
  private string _dbName = null!;
  
  private MasterDb _masterDb = null!;
  
  protected string ConnectionString { get; set; } = null!;
  private string MasterConnectionString { get; set; } = null!;
  
  protected override void RegisterServices(ServiceCollection sc)
  {
    _dbName = TestData.TestName.Replace(DbNameRemove, string.Empty).ToLower();
    
    base.RegisterServices(sc);
    ConnectionString = string.Format(Configuration["TestSettings:ConnectionString"] ?? throw new InvalidOperationException(), _dbName);
    MasterConnectionString =  string.Format(Configuration["TestSettings:ConnectionString"] ?? throw new InvalidOperationException(), "postgres");
    sc.AddJMMemoryCache<JMCacheServerCategory>();
    //sc.AddDbContext<MasterDb>(opt => opt.UseSqlServer(string.Format(ConnectionString, "master")));
    sc.AddDbContext<MasterDb>(opt => opt.UseNpgsql(MasterConnectionString));
  }

  protected override async Task GetServicesAsync(IServiceProvider sp)
  {
    await base.GetServicesAsync(sp);
    _masterDb = sp.GetService<MasterDb>() ?? throw new ArgumentException($"{nameof(DbBaseT)}.{nameof(MasterDb)} is null.");
    NewPGDatabase();
  }

  protected override async Task FinishedTestAsync()
  {
    DropPGDatabaseAsync();
    await base.FinishedTestAsync();
  }

  private void NewPGDatabase()
  {
    string sql = @"
DROP DATABASE IF EXISTS " + _dbName + @" WITH (FORCE);

CREATE DATABASE " + _dbName + @"
    WITH OWNER = postgres
    ENCODING = 'UTF8'
    CONNECTION LIMIT = -1;
 ";


    _masterDb.Database.ExecuteSqlRaw(sql);


    Log.LogInformation("Database '{Dbname}' has been created", _dbName);
  }

  // ReSharper disable once UnusedMember.Local
  private void NewSqlDatabaseAsync()
  {
    string sql = @"
IF EXISTS 
   (
     SELECT name FROM master.dbo.sysdatabases 
    WHERE name = N'" + _dbName + @"'
    )
BEGIN
   ALTER DATABASE " + _dbName + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
   DROP DATABASE [" + _dbName + @"];
END

CREATE DATABASE " + _dbName + "; ";


    _masterDb.Database.ExecuteSqlRaw(sql);


    Log.LogInformation("Database '{Dbname}' has been created", _dbName);
  }

  private void DropPGDatabaseAsync()
  {
    if (TestData.TestEnvironmentType == TestEnvironmentTypeEnum.Dev) 
      return;
    
    var sql = "DROP DATABASE IF EXISTS " + _dbName + " WITH (FORCE);";
    _masterDb.Database.ExecuteSqlRaw(sql);

    Log.LogInformation("Database '{Dbname}' has been deleted", _dbName);
  }

  // ReSharper disable once UnusedMember.Local
  private void DropSqlDatabaseAsync()
  {
    if (TestData.TestEnvironmentType != TestEnvironmentTypeEnum.Dev)
    {
      var sql = @"
IF EXISTS 
   (
     SELECT name FROM master.dbo.sysdatabases 
    WHERE name = N'" + _dbName + @"'
    )
BEGIN
   ALTER DATABASE " + _dbName + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
   DROP DATABASE [" + _dbName + @"];
END

";
      _masterDb.Database.ExecuteSqlRaw(sql);

      Log.LogInformation("Database '{Dbname}' has been deleted", _dbName);
    }
  }
}

public class MasterDb : DbContext
{
  public MasterDb(DbContextOptions<MasterDb> options) : base(options)
  {
  }
}