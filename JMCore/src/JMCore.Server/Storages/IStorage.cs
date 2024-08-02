using JMCore.Server.Storages.Models;

namespace JMCore.Server.Storages;

public interface IStorage
{
  StorageTypeDefinition StorageDefinition { get; }
}