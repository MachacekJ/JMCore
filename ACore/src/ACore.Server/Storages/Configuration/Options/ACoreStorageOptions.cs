namespace ACore.Server.Storages.Configuration.Options;

public class ACoreStorageOptions
{
  public bool UseMemoryStorage { get; set; }
  public ACoreStorageMongoOptions? MongoDb { get; set; }
  public ACoreStoragePGOptions? PGDb { get; set; }
}