namespace ACore.Server.Storages.Definitions.Models;

[Flags]
public enum StorageTypeEnum
{
  Memory = 1 << 0,
  Postgres = 1 << 1,
  Mongo = 1 << 2,

  All = Memory | Postgres | Mongo
}