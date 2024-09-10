namespace ACore.Server.Storages.Configuration;

public class StorageOptionBuilder
{
  private bool _isMem;
  private StoragePGOptions? _storagePGOptions;
  private StorageMongoOptions? _storageMongoOptions;

  private StorageOptionBuilder() { }

  public static StorageOptionBuilder Empty() => new();
  
  public StorageOptionBuilder AddMemoryDb()
  {
    _isMem = true;
    return this;
  }

  public StorageOptionBuilder AddPG(string readWriteConnectionString, string? readOnlyConnectionString = null)
  {
    _storagePGOptions = new StoragePGOptions(readWriteConnectionString, readOnlyConnectionString);
    return this;
  }

  public StorageOptionBuilder AddMongo(string readWriteConnectionString, string collectionName, string? readOnlyConnectionString = null)
  {
    _storageMongoOptions = new StorageMongoOptions(readWriteConnectionString, collectionName, readOnlyConnectionString);
    return this;
  }

  public StorageOptions Build()
  {
    CheckAtLeastOneDB();
    return new StorageOptions
    {
      UseMemoryStorage = _isMem,
      PGDb = _storagePGOptions,
      MongoDb = _storageMongoOptions
    };
  }

  private void CheckAtLeastOneDB()
  {
    if (_isMem)
      return;
    if (_storagePGOptions != null)
      return;
    if (_storageMongoOptions != null)
      return;

    throw new Exception("Please register at least one storage.");
  }
}