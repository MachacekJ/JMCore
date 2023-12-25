using JMCore.Server.Services.JMCache;
using JMCore.Services.JMCache;
using JMCore.Tests.ServerT;
using JMCore.Tests.TestModelsT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JMCore.TestsIntegrations.ServerT.DbT;

/// <summary>
/// Working with mssql database
/// </summary>
public class DbBaseT : ServerTestBaseT
{
    private MasterDb _masterDb = null!;

    protected string ConnectionString { get; set; } = null!;
    protected string DevDatabaseName { get; set; } = null!;
    
    protected override void RegisterServices(ServiceCollection sc)
    {
        base.RegisterServices(sc);
        ConnectionString = Configuration["TestSettings:ConnectionString"] ?? throw new InvalidOperationException();
        DevDatabaseName = Configuration["TestSettings:DevDatabaseName"] ?? throw new InvalidOperationException();
        sc.AddJMMemoryCache<JMCacheServerCategory>();
        sc.AddDbContext<MasterDb>(opt => opt.UseSqlServer(string.Format(ConnectionString, "master")));
    }

    protected override async Task GetServicesAsync(IServiceProvider sp)
    {
        await base.GetServicesAsync(sp);
        _masterDb = sp.GetService<MasterDb>() ?? throw new ArgumentException($"{nameof(DbBaseT)}.{nameof(MasterDb)} is null.");
        NewSqlDatabaseAsync();
    }

    protected override async Task FinishedTestAsync()
    {
        DropDatabaseAsync();
        await base.FinishedTestAsync();
    }
    
    private void NewSqlDatabaseAsync()
    {
        string dbname = TestData.TestName;
        string sql = @"
IF EXISTS 
   (
     SELECT name FROM master.dbo.sysdatabases 
    WHERE name = N'" + dbname + @"'
    )
BEGIN
   ALTER DATABASE " + dbname + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
   DROP DATABASE [" + dbname + @"];
END

CREATE DATABASE " + dbname + "; ";


        _masterDb.Database.ExecuteSqlRaw(sql);


        Log.LogInformation("Database '{Dbname}' has been created", dbname);
    }

    private void DropDatabaseAsync()
    {
        if (TestData.TestEnvironmentType != TestEnvironmentTypeEnum.Dev)
        {
            string dbname = TestData.TestName;

            var sql = @"
IF EXISTS 
   (
     SELECT name FROM master.dbo.sysdatabases 
    WHERE name = N'" + dbname + @"'
    )
BEGIN
   ALTER DATABASE " + dbname + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
   DROP DATABASE [" + dbname + @"];
END

";
            _masterDb.Database.ExecuteSqlRaw(sql);

            Log.LogInformation("Database '{Dbname}' has been deleted", dbname);
        }
    }
}

public class MasterDb : DbContext
{
    public MasterDb(DbContextOptions<MasterDb> options) : base(options)
    {
    }
}