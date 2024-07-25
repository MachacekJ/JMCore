using JMCore.Server.MongoStorage.AuditModule.Models;
using JMCore.Server.Storages.Base.EF;
using JMCore.Server.Storages.Modules.AuditModule.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace JMCore.Server.MongoStorage.AuditModule.Scripts;

// ReSharper disable once InconsistentNaming
public class V1_0_0_1AuditCollection : DbVersionScriptsBase
{
  public override Version Version => new("1.0.0.1");
  
  public override void AfterScriptRunCode<T>(T impl, DbContextOptions options, ILogger<DbContextBase> logger)
  {
    var ext = options.FindExtension<MongoOptionsExtension>() ?? throw new Exception($"{nameof(MongoOptionsExtension)} has not been found in extensions.");
    var connectionString = ext.ConnectionString;
      
    var client = new MongoClient(connectionString);
    var db = client.GetDatabase(ext.DatabaseName);
    db.CreateCollection(CollectionNames.AuditCollectionName);
    var col = db.GetCollection<AuditMongoEntity>(CollectionNames.AuditCollectionName);
    
    var index = Builders<AuditMongoEntity>.IndexKeys.Ascending(e => e.ObjectId);
    col.Indexes.CreateOneAsync(new CreateIndexModel<AuditMongoEntity>(index));
    
    //db.GetCollection<AuditEntity>(CollectionNames.AuditCollectionName).Indexes.CreateMany()
    logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", CollectionNames.AuditCollectionName, ext.DatabaseName);
  }
}