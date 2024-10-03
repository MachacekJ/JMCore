using ACore.Server.Storages.Definitions;

namespace ACore.Server.Storages;

public interface IStorage
{
  StorageDefinition StorageDefinition { get; }
  Task UpdateDatabase();
}