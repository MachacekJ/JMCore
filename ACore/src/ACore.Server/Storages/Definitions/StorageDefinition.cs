using ACore.Server.Storages.Definitions.Models;

namespace ACore.Server.Storages.Definitions;

public abstract class StorageDefinition
{
  public abstract StorageTypeEnum Type { get; }
}