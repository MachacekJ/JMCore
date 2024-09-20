namespace ACore.Server.Storages.CQRS;

public class SaveHandlerData(object entity, IStorage storage, Task task, bool withHash = false)
{
  public bool WithHash => withHash;
  public Object Entity => entity;
  public Task Task => task;
  public IStorage Storage => storage;
}

public class SaveHandlerData<T>(T entity, IStorage storage, Task task, bool withHash = false) :
  SaveHandlerData(entity, storage, task, withHash)
  where T : class;