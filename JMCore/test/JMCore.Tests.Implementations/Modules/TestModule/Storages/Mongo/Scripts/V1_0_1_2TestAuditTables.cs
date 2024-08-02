using JMCore.Server.Storages.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

// ReSharper disable InconsistentNaming

namespace JMCore.Tests.Implementations.Modules.TestModule.Storages.Mongo.Scripts;

public class V1_0_1_2TestAuditTables : DbVersionScriptsBase
{
    public override Version Version => new("1.0.0.2");
    
    public override void AfterScriptRunCode<T>(T impl, DbContextOptions options, ILogger<DbContextBase> logger)
    {
        var ext = options.FindExtension<MongoOptionsExtension>() ?? throw new Exception($"{nameof(MongoOptionsExtension)} has not been found in extensions.");
        var connectionString = ext.ConnectionString;
      
        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(ext.DatabaseName);
        db.CreateCollection(TestMongoStorageImpl.TestAttributeCollectionName);
        logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", TestMongoStorageImpl.TestAttributeCollectionName, ext.DatabaseName);
    }

}