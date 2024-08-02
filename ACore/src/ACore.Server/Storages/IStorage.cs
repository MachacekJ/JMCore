using ACore.Server.Storages.Models;

namespace ACore.Server.Storages;

public interface IStorage
{
  StorageTypeDefinition StorageDefinition { get; }
}