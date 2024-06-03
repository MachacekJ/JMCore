//  MSSQL is not supported for now 
// ----------------------------------












// namespace JMCore.TestsIntegrations.ServerT.StoragesT;
//
// public class MSSQLStorageT
// {
//protected string ConnectionStringMSSQL { get; set; } = null!;

//private string MasterConnectionStringMSSQL { get; set; } = null!;
//   // sc.AddDbContext<MasterDb>(opt => opt.UseSqlServer(string.Format(ConnectionStringMSSQL, "master")));
//   // ConnectionStringMSSQL = string.Format(Configuration["TestSettings:ConnectionStringMSSQL"] ?? throw new InvalidOperationException(), _dbName);
//   //MasterConnectionStringMSSQL =  string.Format(Configuration["TestSettings:ConnectionStringMSSQL"] ?? throw new InvalidOperationException(), "master");
//
//   private void NewSqlDatabase()
//   {
//     string sql = @"
// IF EXISTS 
//    (
//      SELECT name FROM master.dbo.sysdatabases 
//     WHERE name = N'" + DbName + @"'
//     )
// BEGIN
//    ALTER DATABASE " + DbName + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
//    DROP DATABASE [" + DbName + @"];
// END
//
// CREATE DATABASE " + DbName + "; ";
//
//
//     _masterDb.Database.ExecuteSqlRaw(sql);
//
//
//     Log.LogInformation("Database '{Dbname}' has been created", DbName);
//   }
//
//   
//   // ReSharper disable once UnusedMember.Local
//   private void DropSqlDatabase()
//   {
//     if (TestData.TestEnvironmentType != TestEnvironmentTypeEnum.Dev)
//     {
//       var sql = @"
// IF EXISTS 
//    (
//      SELECT name FROM master.dbo.sysdatabases 
//     WHERE name = N'" + DbName + @"'
//     )
// BEGIN
//    ALTER DATABASE " + DbName + @" SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
//    DROP DATABASE [" + DbName + @"];
// END
//
// ";
//       _masterDb.Database.ExecuteSqlRaw(sql);
//
//       Log.LogInformation("Database '{Dbname}' has been deleted", DbName);
//     }
//   }
// }