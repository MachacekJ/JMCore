using ACore.Server.Storages.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ACore.AppTest.Modules.TestModule.Storages.Mongo.Scripts;

// ReSharper disable once InconsistentNaming
public class V1_0_1_1TestTable : DbVersionScriptsBase
{
    public override Version Version => new ("1.0.0.1");

    public override void AfterScriptRunCode<T>(T impl, DbContextOptions options, ILogger<DbContextBase> logger)
    {
        var ext = options.FindExtension<MongoOptionsExtension>() ?? throw new Exception($"{nameof(MongoOptionsExtension)} has not been found in extensions.");
        var connectionString = ext.ConnectionString;
      
        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(ext.DatabaseName);
        db.CreateCollection(EfTestMongoStorageImpl.TestCollectionName);
        logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", EfTestMongoStorageImpl.TestCollectionName, ext.DatabaseName);
    }
}