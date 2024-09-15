namespace ACore.Server.Storages.CQRS;


public class SaveHandlerData(object entity, IStorage storage, Task task)
{
  public Object Entity => entity;
  public Task Task => task;
  public IStorage Storage => storage;
}

public class SaveHandlerData<T>(T entity, IStorage storage, Task task)
  where T : class
{
  public T Entity => entity;
  public Task Task => task;
  public IStorage Storage => storage;
}