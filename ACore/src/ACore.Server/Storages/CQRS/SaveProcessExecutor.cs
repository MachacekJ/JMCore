using ACore.Server.Storages.Attributes;

namespace ACore.Server.Storages.CQRS;

public class SaveProcessExecutor(object entity, IStorage storage, Task task)
{
  public bool WithHash => entity.GetType().IsHashCheck();
  public Object Entity => entity;
  public Task Task => task;
  public IStorage Storage => storage;
}

public class SaveProcessExecutor<T>(T entity, IStorage storage, Task task) :
  SaveProcessExecutor(entity, storage, task)
  where T : class;