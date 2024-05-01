namespace JMCore.Server.Configuration.Storage.Models;


[Flags]
public enum StorageTypeEnum
{
  Memory= 1 << 0,
  Postgres = 1 << 1,
  Mongo = 1 << 2,
  
  AllRegistered = Memory | Postgres | Mongo
}