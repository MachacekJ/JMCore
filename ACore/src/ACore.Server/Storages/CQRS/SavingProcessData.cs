using ACore.Server.Storages.Attributes;

namespace ACore.Server.Storages.CQRS;

public class SavingProcessData(object entity, IStorage storage, Task task)
{
  public bool WithHash => entity.GetType().IsHashCheck();
  public Object Entity => entity;
  public Task Task => task;
  public IStorage Storage => storage;
}

public class SavingProcessData<T>(T entity, IStorage storage, Task task) :
  SavingProcessData(entity, storage, task)
  where T : class;