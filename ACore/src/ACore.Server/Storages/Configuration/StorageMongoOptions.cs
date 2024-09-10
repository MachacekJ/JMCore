namespace ACore.Server.Storages.Configuration;

public class StorageMongoOptions(string readWriteConnectionString, string collectionName, string? readOnlyConnectionString = null)
{
  public string ReadOnlyConnectionString => readOnlyConnectionString ?? readWriteConnectionString; 
  public string ReadWriteConnectionString => readWriteConnectionString;
  public string CollectionName => collectionName;
}