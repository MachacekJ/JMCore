namespace ACore.Server.Storages.Models;

[Flags]
public enum StorageModeEnum
{
  Read = 1 << 0,
  Write = 1 << 1,
  ReadWrite = Read | Write
}