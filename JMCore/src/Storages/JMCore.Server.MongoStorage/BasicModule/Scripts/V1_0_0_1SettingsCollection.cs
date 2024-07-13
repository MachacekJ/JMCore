using JMCore.Server.Storages.Base.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace JMCore.Server.MongoStorage.BasicModule.Scripts;

// ReSharper disable once InconsistentNaming
public class V1_0_0_1SettingsCollection : DbVersionScriptsBase
{
  public override Version Version => new("1.0.0.1");

  public override void AfterScriptRunCode<T>(T impl, DbContextOptions options, ILogger<DbContextBase> logger)
  {
    var ext = options.FindExtension<MongoOptionsExtension>() ?? throw new Exception($"{nameof(MongoOptionsExtension)} has not been found in extensions.");
    var connectionString = ext.ConnectionString;
      
    var client = new MongoClient(connectionString);
    var db = client.GetDatabase(ext.DatabaseName);
    
    db.CreateCollection(CollectionNames.SettingsCollectionName);
    logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", CollectionNames.SettingsCollectionName, ext.DatabaseName);
  }
}