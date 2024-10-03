namespace ACore.Server.Storages.Definitions.EF.PGStorage.Configuration;

public class StoragePGOptions(string readWriteConnectionString, string? readOnlyConnectionString = null)
{
  public string ReadOnlyConnectionString => readOnlyConnectionString ?? readWriteConnectionString; 
  public string ReadWriteConnectionString => readWriteConnectionString;
}