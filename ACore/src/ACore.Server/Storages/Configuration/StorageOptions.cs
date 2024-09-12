namespace ACore.Server.Storages.Configuration;

public class StorageOptions
{
  public bool UseMemoryStorage { get; init; }
  public StorageMongoOptions? MongoDb { get; init; }
  public StoragePGOptions? PGDb { get; init; }
}