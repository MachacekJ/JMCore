namespace ACore.Server.Storages.Configuration.Options;

public class ACoreStorageMongoOptions(string readWrite, string dbName, string? readOnly = null)
{
  public string ReadOnly => readOnly ?? readWrite; 
  public string ReadWrite => readWrite;
  public string DbName => dbName;
}