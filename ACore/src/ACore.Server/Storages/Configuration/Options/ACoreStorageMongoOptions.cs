namespace ACore.Server.Storages.Configuration.Options;

public class ACoreStorageMongoOptions(string readWriteConnectionString, string collectionName, string? readOnlyConnectionString = null)
{
  public string ReadOnlyConnectionString => readOnlyConnectionString ?? readWriteConnectionString; 
  public string ReadWriteConnectionString => readWriteConnectionString;
  public string CollectionName => collectionName;
}