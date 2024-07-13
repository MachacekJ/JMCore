using JMCore.Server.Configuration.Storage;
using JMCore.TestsIntegrations.ServerT.StoragesT.TestStorageConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Serilog;

namespace JMCore.TestsIntegrations.ServerT.StoragesT;

public class MongoStorageRegistrationT(string dbName) : IStorageRegistrationT
{
  private ILogger<MongoStorageRegistrationT> _log = null!;
  private string ConnectionStringMongo { get; set; } = null!;

  public void RegisterServices(ServiceCollection sc, IConfigurationRoot configuration, IEnumerable<string> requiredBaseStorageModules, IStorageResolver storageResolver)
  {
    ConnectionStringMongo = configuration["TestSettings:ConnectionStringMongo"] ?? throw new InvalidOperationException();
    storageResolver.RegisterStorage(sc, new MongoTestStorageConfiguration(ConnectionStringMongo, dbName, requiredBaseStorageModules));
  }

  public void GetServices(IServiceProvider sp)
  {
    _log = sp.GetService<ILogger<MongoStorageRegistrationT>>() ?? throw new ArgumentException($"{nameof(ILogger<MongoStorageRegistrationT>)} is null.");
    NewMongoDatabase();
  }

  public void FinishedTest()
  {
    var client = new MongoClient(ConnectionStringMongo);
    client.DropDatabase(dbName);
    _log.LogInformation("Database '{Dbname}' has been deleted", dbName);
  }

  private void NewMongoDatabase()
  {
    var client = new MongoClient(ConnectionStringMongo);
    client.DropDatabase(dbName);
    _log.LogInformation("Database '{Dbname}' has been created.", dbName);
  }
}