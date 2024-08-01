namespace JMCore.Server.Storages.Models;

[Flags]
public enum StorageTypeEnum
{
  Memory= 1 << 0,
  Postgres = 1 << 1,
  Mongo = 1 << 2,
  
  AllRegistered = Memory | Postgres | Mongo
}

public class StorageTypeDefinition(StorageTypeEnum type)
{
  public bool IsTransactionEnabled => type switch
  {
    StorageTypeEnum.Memory => false,
    StorageTypeEnum.Postgres => true,
    StorageTypeEnum.Mongo => false,
    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
  };
  public StorageTypeEnum Type => type;
}