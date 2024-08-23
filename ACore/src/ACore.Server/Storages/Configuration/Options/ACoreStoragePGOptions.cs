namespace ACore.Server.Storages.Configuration.Options;

public class ACoreStoragePGOptions(string readWriteConnectionString, string? readOnlyConnectionString = null)
{
  public string ReadOnlyConnectionString => readOnlyConnectionString ?? readWriteConnectionString; 
  public string ReadWriteConnectionString => readWriteConnectionString;
}