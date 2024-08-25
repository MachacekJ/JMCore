using ACore.AppTest.Modules.TestModule.Storages.Mongo.Models;
using ACore.Server.Storages.EF;
using ACore.Server.Storages.Scripts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

// ReSharper disable InconsistentNaming

namespace ACore.AppTest.Modules.TestModule.Storages.Mongo.Scripts;

public class V1_0_1_2TestAuditTables : DbVersionScriptsBase
{
    public override Version Version => new("1.0.0.2");
    
    public override void AfterScriptRunCode<T>(T impl, DbContextOptions options, ILogger<DbContextBase> logger)
    {
        var ext = options.FindExtension<MongoOptionsExtension>() ?? throw new Exception($"{nameof(MongoOptionsExtension)} has not been found in extensions.");
        var connectionString = ext.ConnectionString;
      
        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(ext.DatabaseName);

        var collectionName = DefaultNames.ObjectNameMapping[nameof(TestAttributeAuditPKMongoEntity)].TableName;
        
        db.CreateCollection(collectionName);
        logger.LogInformation("Collection '{collectionName}' in database '{DatabaseName}' has been created.", collectionName, ext.DatabaseName);
    }

}