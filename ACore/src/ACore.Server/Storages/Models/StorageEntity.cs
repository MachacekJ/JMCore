namespace ACore.Server.Storages.Models;

public class IntStorageEntity : StorageEntity<int>
{
}

public class LongStorageEntity : StorageEntity<long>
{
}

public class GuidStorageEntity : StorageEntity<Guid>
{
}

public class StringStorageEntity : StorageEntity<string>
{
}

public class StorageEntity<T>
{
  public T Id { get; set; }
}