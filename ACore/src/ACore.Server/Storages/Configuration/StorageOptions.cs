namespace ACore.Server.Storages.Configuration;

public class StorageOptions
{
  public bool UseMemoryStorage { get; set; }
  public StorageMongoOptions? MongoDb { get; set; }
  public StoragePGOptions? PGDb { get; set; }
}