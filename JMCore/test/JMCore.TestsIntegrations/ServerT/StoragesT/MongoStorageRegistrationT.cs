using JMCore.Server.Configuration.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace JMCore.TestsIntegrations.ServerT.StoragesT;

public class MongoStorageRegistrationT(string dbName): IStorageRegistrationT
{
  private ILogger<MongoStorageRegistrationT> _log = null!;
  private string ConnectionStringMongo { get; set; } = null!;

  public void RegisterServices(ServiceCollection sc, IConfigurationRoot configuration, IStorageResolver storageResolver)
  {
    ConnectionStringMongo = configuration["TestSettings:ConnectionStringMongo"] ?? throw new InvalidOperationException();
  }
  public void GetServices(IServiceProvider sp)
  {
    _log =  sp.GetService<ILogger<MongoStorageRegistrationT>>() ?? throw new ArgumentException($"{nameof(ILogger<MongoStorageRegistrationT>)} is null.");
    NewMongoDatabase();
  }
  
  public void FinishedTest()
  {
    var client = new MongoClient(ConnectionStringMongo);
    client.DropDatabase(dbName);
  }
  
  private void NewMongoDatabase()
  {
    var client = new MongoClient(ConnectionStringMongo);
    client.DropDatabase(dbName);
    var db = client.GetDatabase(dbName);
    // db.CreateCollection("aa");
    // var aa = new BasicMongoDbContext (new DbContextOptionsBuilder<BasicMongoDbContext>()
    //   .UseMongoDB(client, DbName).Options);
  }
}